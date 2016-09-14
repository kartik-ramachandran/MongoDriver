using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDriver.Helper
{
    public class GetHelper 
    {
        public static async Task<List<T>> GetAll<T>(string tableName)
        {
            return await CommonHelper.QueryAll<T>(tableName);
        }

        public static async Task<List<T>> Get<T, TValue>(string field, TValue value, string tableName)
        {
            return await CommonHelper.GenericQuery<T>(CommonHelper.GetFilterDefinition(field, value), tableName);
        }

        public static List<BsonDocument> GetSync<TValue>(string field, TValue value, string tableName)
        {
            return CommonHelper.GenericQuerySync(CommonHelper.GetFilterDefinition(field, value), tableName);
        }

        public static async Task<List<T>> GetGreaterThan<T, TValue>(string field, TValue value, string tableName)
        {
            return await CommonHelper.GenericQuery<T>(CommonHelper.GetFilterDefinition(field, value), tableName);
        }

        public static async Task<List<T>> GetLessThan<T, TValue>(string field, TValue value, string tableName)
        {
            return await CommonHelper.GenericQuery<T>(CommonHelper.GetFilterDefinition(field, value), tableName);
        }

        public static BsonDocument FindAndModify(string field, string tableName)
        {
            var collection = CommonHelper.GetMongoCollection(tableName);

            return CommonHelper.FindAndModifySync(tableName, field);
        }

        public static async Task<BsonDocument> GetLastRecord(string field, string tableName)
        {
            var collection = CommonHelper.GetMongoCollection(tableName);

            var filter = new BsonDocument();

            var sort = SortHelper.SortDescending(field);

            var dataList = await collection.Find(filter).ToListAsync();

            if (dataList.Count == 0)
            {
                return null;
            }

            return dataList.Last();
        }

        public static BsonDocument GetLastRecordSync(string field, string tableName)
        {
            var collection = CommonHelper.GetMongoCollection(tableName);

            var filter = new BsonDocument();

            var sort = SortHelper.SortDescending(field);

            var dataList = collection.Find(filter).ToList();

            if (dataList.Count == 0)
            {
                return null;
            }

            return collection.Find(filter).ToList().Last();
        }

        public static async Task<BsonDocument> GetFirstRecord(string field, string tableName)
        {
            var collection = CommonHelper.GetMongoCollection(tableName);

            var filter = new BsonDocument();

            var sort = SortHelper.SortAscending(field);

            return await collection.Find(filter).Sort("{'_id': 1 }").FirstOrDefaultAsync();
        }

    }
}
