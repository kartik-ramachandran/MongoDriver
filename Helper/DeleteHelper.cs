using MongoDB.Bson;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace MongoDriver.Helper
{
    public class DeleteHelper 
    {
        public static async Task<bool> RemoveDocument<T>(string field, T value, string tableName)
        {
            var collection = CommonHelper.GetMongoCollection(tableName);
            var filter = CommonHelper.GetFilterDefinition(field, value);
            var result = await collection.DeleteOneAsync(filter);

            return result.IsAcknowledged;
        }

        public static async Task<bool> RemoveManyDocument<T>(string field, T value, string tableName)
        {
            var collection = CommonHelper.GetMongoCollection(tableName);
            var filter = CommonHelper.GetFilterDefinition(field, value);
            var result = await collection.DeleteOneAsync(filter);

            return result.IsAcknowledged;
        }

        public static async Task<bool> RemoveCollection(string tableName)
        {
            var collection = CommonHelper.GetMongoCollection(tableName);
            var filter = new BsonDocument();
            var result = await collection.DeleteOneAsync(filter);

            return result.IsAcknowledged;
        }

        public static async Task<bool> RemoveManyCollection(string tableName)
        {
            var collection = CommonHelper.GetMongoCollection(tableName);
            var filter = new BsonDocument();
            var result = await collection.DeleteOneAsync(filter);

            return result.IsAcknowledged;
        }
    }
}
