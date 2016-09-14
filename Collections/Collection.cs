using MongoDB.Bson;
using MongoDB.Driver;
using MongoDriver.Database;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MongoDriver.Collections
{
    public class Collection
    {
        public static IMongoDatabase MongoDatabase
        {
            get { return Connect.GetDatabase(); }
        }

        public static async Task<bool> CheckIfCollectionExists(string collectionName)
        {
            var collectionList = await ListCollection();

            foreach (var item in collectionList)
            {
                if (item.GetValue(0).ToString().Equals(collectionName))
                {
                    return true;
                }                   
            }

            return false;
        }

        public static IMongoCollection<BsonDocument> GetCollection(string collectionName)
        {
            return MongoDatabase.GetCollection<BsonDocument>(collectionName);
        }

        public static async Task CreateCollection(string collectionName)
        {
            var options = new CreateCollectionOptions
            {
                Capped = false               
            };

            await MongoDatabase.CreateCollectionAsync(collectionName, options);
        }

        public static async Task DeleteCollection(string collectionName)
        {
            await MongoDatabase.DropCollectionAsync(collectionName);
        }
        public static async Task<List<BsonDocument>> ListCollection()
        {
            using (var cursor = await MongoDatabase.ListCollectionsAsync())
            {
                return cursor.ToList();              
            }
        }

        public static async Task RenameCollection(string oldCollectionName, string newCollectionName)
        {
            await MongoDatabase.RenameCollectionAsync(oldCollectionName, newCollectionName);
        }
    }
}
