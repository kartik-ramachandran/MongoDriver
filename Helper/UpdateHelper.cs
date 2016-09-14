using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MongoDriver.Helper
{
    public class UpdateHelper 
    {
        public static async Task<bool> UpdateSingleDocument<TFilter, TValue>
        (string filterField, TFilter filterValue, string field, TValue value, string tableName)
        {
            return await UpdateOne(CommonHelper.GetFilterDefinition(filterField, filterValue), UpdateDefinition(field, value), tableName);
        }

        public static bool UpdateSingleDocumentSync<TFilter, TValue>
       (string filterField, TFilter filterValue, string field, TValue value, string tableName)
        {
            return UpdateOneSync(CommonHelper.GetFilterDefinition(filterField, filterValue), UpdateDefinition(field, value), tableName);
        }


        public static async Task<bool> UpdateMultipleDocument<T, Tkey, TValue>
            (Dictionary<Tkey, TValue> fields, string field, T value, string tableName)
        {
            return await UpdateMany(CommonHelper.BuildComplexQuery(fields), UpdateDefinition(field, value), tableName);
        }

        public static bool UpdateMultipleDocumentSync<T, Tkey, TValue>
           (Dictionary<Tkey, TValue> fields, string field, T value, string tableName)
        {
            return UpdateManySync(CommonHelper.BuildComplexQuery(fields), UpdateDefinition(field, value), tableName);
        }

        public static UpdateDefinition<BsonDocument> UpdateDefinition<T>(string field, T value)
        {
            return Builders<BsonDocument>.Update.Set(field, value);
        }

        private static async Task<bool> UpdateOne(FilterDefinition<BsonDocument> function, UpdateDefinition<BsonDocument> updateFunction, string tableName)
        {
            var collection = CommonHelper.GetMongoCollection(tableName);
            var filter = function;
            var update = updateFunction;

            var result = await collection.UpdateOneAsync(filter, update);

            if (result.IsModifiedCountAvailable)
                return true;

            return false;
        }

        private static bool UpdateOneSync(FilterDefinition<BsonDocument> function, UpdateDefinition<BsonDocument> updateFunction, string tableName)
        {
            var collection = CommonHelper.GetMongoCollection(tableName);
            var filter = function;
            var update = updateFunction;

            var result = collection.UpdateOne(filter, update);

            if (result.IsModifiedCountAvailable)
                return true;

            return false;
        }

        private static async Task<bool> UpdateMany(FilterDefinition<BsonDocument> function, UpdateDefinition<BsonDocument> updateFunction, string tableName)
        {
            var collection = CommonHelper.GetMongoCollection(tableName);
            var filter = function;
            var update = updateFunction;

            var result = await collection.UpdateManyAsync(filter, update);

            if (result.IsModifiedCountAvailable)
                return true;

            return false;
        }

        private static bool UpdateManySync(FilterDefinition<BsonDocument> function, UpdateDefinition<BsonDocument> updateFunction, string tableName)
        {
            var collection = CommonHelper.GetMongoCollection(tableName);
            var filter = function;
            var update = updateFunction;

            var result = collection.UpdateMany(filter, update);

            if (result.IsModifiedCountAvailable)
                return true;

            return false;
        }
    }
}
