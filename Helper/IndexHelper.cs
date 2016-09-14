using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MongoDriver.Helper
{
    public class IndexHelper
    {
        public static IndexKeysDefinition<BsonDocument> AscendingIndexKeys(string field)
        {
            return Builders<BsonDocument>.IndexKeys.Ascending(field);
        }

        public static async Task<string> CreateSingleFieldIndex(string field, string tableName)
        {
            var collection = CommonHelper.GetMongoCollection(tableName);
            var keys = Builders<BsonDocument>.IndexKeys.ExtendIndexKeysCreation(field);
            return await collection.Indexes.CreateOneAsync(keys);
        }

        public static async Task<string> CreateComplexFieldIndex(List<string> field, string tableName)
        {
            var collection = CommonHelper.GetMongoCollection(tableName);
            var keys = Builders<BsonDocument>.IndexKeys.ExtendIndexKeysCreation(field);
            return await collection.Indexes.CreateOneAsync(keys);
        }

        public static async Task<List<BsonDocument>> GetIndexes(string tableName)
        {
            var collection = CommonHelper.GetMongoCollection(tableName);

            using (var cursor = await collection.Indexes.ListAsync())
            {
                return await cursor.ToListAsync();
            }
        }
    }
}
