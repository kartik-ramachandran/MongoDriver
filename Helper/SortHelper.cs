using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;

namespace MongoDriver.Helper
{
    public class SortHelper
    {
        #region Sort
        public static SortDefinition<BsonDocument> SortAscending(string field)
        {
            FieldDefinition<BsonDocument> fieldDefinition = field;

            return Builders<BsonDocument>.Sort.Ascending(fieldDefinition);
        }

        public static SortDefinition<BsonDocument> SortDescending(string field)
        {
            FieldDefinition<BsonDocument> fieldDefinition = field;

            return Builders<BsonDocument>.Sort.Descending(fieldDefinition);
        }

        public static SortDefinition<BsonDocument> SortDescending<T>(IEnumerable<SortDefinition<BsonDocument>> listSortDefinition)
        {
            return Builders<BsonDocument>.Sort.Combine(listSortDefinition);
        }
        #endregion
    }
}
