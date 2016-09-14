using MongoDB.Bson;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MongoDriver.Helper
{
    public class InsertHelper 
    {      
        public static async Task Insert(BsonDocument document, string tableName)
        {
            var collection = CommonHelper.GetMongoCollection(tableName);
            await collection.InsertOneAsync(document);
        }

        public static void InsertSync(BsonDocument document, string tableName)
        {
            var collection = CommonHelper.GetMongoCollection(tableName);
            collection.InsertOne(document);
        }


        public static async Task InsertManyDocument(List<BsonDocument> document, string tableName)
        {
            var collection = CommonHelper.GetMongoCollection(tableName);
            await collection.InsertManyAsync(document);
        }

        public static void InsertManyDocumentSync(List<BsonDocument> document, string tableName)
        {
            var collection = CommonHelper.GetMongoCollection(tableName);
            collection.InsertMany(document);
        }
    }
}
