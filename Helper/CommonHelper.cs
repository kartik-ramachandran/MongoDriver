using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using MongoDriver.Database;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MongoDriver.Helper
{
    public class CommonHelper
    {
        public static IMongoDatabase MongoDatabase
        {
            get { return Connect.GetDatabase(); }
        }

        public static IMongoCollection<BsonDocument> GetMongoCollection(string name)
        {
            return MongoDatabase.GetCollection<BsonDocument>(name);
        }

        public static GridFSBucket MongoGridFs
        {
            get { return Connect.GetGridFs(); }
        }            
        
        public static FilterDefinition<BsonDocument> BuildComplexQuery<Tkey, TValue>(Dictionary<Tkey, TValue> fields)
        {
            var count = 0;

            FilterDefinition<BsonDocument> filter = null;
            var builder = Builders<BsonDocument>.Filter;

            foreach (var field in fields)
            {
                if (count == 0)
                {
                    filter = builder.Eq(field.Key.ToString(), field.Value);
                }
                else
                {
                    filter = filter & builder.Eq(field.Key.ToString(), field.Value);
                }
            }
            return filter;
        }      

        public static async Task<List<BsonDocument>> GetResultCollection(IMongoCollection<BsonDocument> collection, FilterDefinition<BsonDocument> filterDefinition)
        {
            return await collection.Find(filterDefinition).ToListAsync();
        }
        public static FilterDefinition<BsonDocument> GetFilterDefinition<T>(string field, T value)
        {
            return Builders<BsonDocument>.Filter.Eq(field, value);
        }

        public static FilterDefinition<BsonDocument> GetGreaterThanDefinition<T>(string field, T value)
        {
            return Builders<BsonDocument>.Filter.Gt(field, value);
        }

        public static FilterDefinition<BsonDocument> GetLessThanDefinition<T>(string field, T value)
        {
            return Builders<BsonDocument>.Filter.Lt(field, value);
        }

        public static async Task<List<T>> QueryAll<T>(string tableName)
        {
            var collection = GetMongoCollection(tableName);
            var filter = Builders<BsonDocument>.Filter.Empty; //Same as new BsonDocument()
            List<T> returnDocument = new List<T>();

            using (var cursor = await collection.FindAsync<T>(filter))
            {
                while (await cursor.MoveNextAsync())
                {
                    var batch = cursor.Current;
                    foreach (var document in batch)
                    {
                        returnDocument.Add(document);
                    }
                }
            }
            return returnDocument;
        }

        public static async Task<List<T>> GenericQuery<T>(FilterDefinition<BsonDocument> function, string tableName)
        {
            var collection = GetMongoCollection(tableName);
            var filter = function;
            List<T> returnDocument = new List<T>();

            using (var cursor = await collection.FindAsync<T>(filter))
            {                
                while (await cursor.MoveNextAsync())
                {
                    var batch = cursor.Current;
                    foreach (var document in batch)
                    {
                        returnDocument.Add(document);
                    }
                }
            }
            return returnDocument;
        }

        public static List<BsonDocument> GenericQuerySync(FilterDefinition<BsonDocument> function, string tableName)
        {
            var collection = GetMongoCollection(tableName);
            var filter = function;                        
            return collection.Find(filter).ToList();            
        }

        public static async Task<List<BsonDocument>> QueryAllPaging(int? skip, int? limit, string tableName)
        {
            var collection = GetMongoCollection(tableName);

            var filter = Builders<BsonDocument>.Filter.Empty;             

            var resultList = await collection.Find(filter).Skip(skip).Limit(limit).ToListAsync();         

            return resultList;
        }

        public static async Task<BsonDocument> FindAndModify(string tableName, string field)
        {
            var collection = GetMongoCollection(tableName);

            var builder = Builders<BsonDocument>.Filter;

            var query = builder.Eq("_id", field);

            var updateBuilder = Builders<BsonDocument>.Update.Inc(field, 1);

            var options = new FindOneAndUpdateOptions<BsonDocument>
            {
                IsUpsert = true
            };

            return await collection.FindOneAndUpdateAsync(query, updateBuilder, options);
        }

        public static BsonDocument FindAndModifySync(string tableName, string field)
        {
            var collection = GetMongoCollection(tableName);

            var builder = Builders<BsonDocument>.Filter;

            var query = builder.Eq("_id", field);

            var updateBuilder = Builders<BsonDocument>.Update.Inc(field, 1);

            var options = new FindOneAndUpdateOptions<BsonDocument>
            {
                IsUpsert = true,
                ReturnDocument = ReturnDocument.After
            };

            var returnValue = collection.FindOneAndUpdate(query, updateBuilder, options);

            return returnValue;
        }
    }

    public static class Extensions
    {
        public static IndexKeysDefinition<BsonDocument> ExtendIndexKeysCreation(this IndexKeysDefinitionBuilder<BsonDocument> Key, string field)
        {
            return Builders<BsonDocument>.IndexKeys.Ascending(field);
        }

        public static IndexKeysDefinition<BsonDocument> ExtendIndexKeysCreation(this IndexKeysDefinitionBuilder<BsonDocument> Key, List<string> fields)
        {
            var indexBuilder = Builders<BsonDocument>.IndexKeys;

            IndexKeysDefinition<BsonDocument> keyDefinition = null;

            var count = 0;

            foreach (var field in fields)
            {
                if (count == 0)
                {
                    keyDefinition = indexBuilder.Ascending(field);
                }
                else
                {
                    keyDefinition = keyDefinition.Ascending(field);

                }
                count++;
            }

            return keyDefinition;
        }
    }
}
