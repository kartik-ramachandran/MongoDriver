using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MongoDriver.Helper
{
    public class AggregationHelper
    {
        public static async Task<List<BsonDocument>> GroupDocumentsByField(string field, string fieldName, string tableName)
        {
            var collection = CommonHelper.GetMongoCollection(tableName);
            var aggregate = collection.Aggregate().Group(
                new BsonDocument {
                    { field, fieldName },
                    { "count", new BsonDocument("$sum", 1) }
                });

            return await aggregate.ToListAsync();
        }

        public static async Task<List<BsonDocument>> FilterAndGroup(IEnumerable<BsonElement> elements, string field, string fieldName, string tableName)
        {
            var collection = CommonHelper.GetMongoCollection(tableName);

            var filter = new BsonDocument(elements);

            var aggregate = collection.Aggregate()
                .Match(filter)
                .Group(new BsonDocument {
                    { field, fieldName },
                    { "count", new BsonDocument("$sum", 1) }
                });

            return await aggregate.ToListAsync();
        }
    }
}
