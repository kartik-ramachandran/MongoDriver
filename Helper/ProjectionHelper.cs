using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;

namespace MongoDriver.Helper
{
    public class ProjectionHelper
    {
        #region Projection
        public static ProjectionDefinition<BsonDocument> ProjectionInclude<T>(string field)
        {
            FieldDefinition<BsonDocument> fieldDefinition = field;

            return Builders<BsonDocument>.Projection.Include(fieldDefinition);
        }

        public static ProjectionDefinition<BsonDocument> ProjectionExclude<T>(string field)
        {
            FieldDefinition<BsonDocument> fieldDefinition = field;

            return Builders<BsonDocument>.Projection.Include(fieldDefinition);
        }

        public static ProjectionDefinition<BsonDocument> ProjectionCombine<T>
            (IEnumerable<ProjectionDefinition<BsonDocument>> listProjectDefinition)
        {
            return Builders<BsonDocument>.Projection.Combine(listProjectDefinition);
        }
        #endregion
    }
}
