using MongoDB.Bson;
using MongoDriver.Helper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MongoDriver.Action
{    
    public class Get 
    {       
        public static async Task<List<T>> GetAll<T>(string tableName)
        {
            return await GetHelper.GetAll<T>(tableName);
        }

        public static async Task<List<T>> QueryByEmbeddedField<T, TValue>(string field, TValue value, string tableName)
        {
            return await GetHelper.Get<T, TValue>(field, value, tableName);            
        }
              
        public static List<BsonDocument> QueryByEmbeddedFieldSync<TValue>(string field, TValue value, string tableName)
        {
            return GetHelper.GetSync<TValue>(field, value, tableName);
        }

        public static async Task<List<T>> QueryByArray<T, TValue>(string field, TValue value, string tableName)
        {            
            return await GetHelper.Get<T, TValue>(field, value, tableName);
        }

        public static async Task<List<T>> QueryByGreaterThan<T, TValue>(string field, TValue value, string tableName)
        {            
            return await GetHelper.GetGreaterThan<T, TValue>(field, value, tableName);
        }

        public static async Task<List<T>> QueryByLessThan<T, TValue>(string field, TValue value, string tableName)
        {         
            return await GetHelper.GetGreaterThan<T, TValue>(field, value, tableName);
        }

        public static async Task<List<BsonDocument>> GetAllPaging(int? skip, int? limit, string tableName)
        {
            return await CommonHelper.QueryAllPaging(skip, limit, tableName);
        }

        public static async Task<BsonDocument> GetFirstRecord(string field, string tableName)
        {
            return await GetHelper.GetFirstRecord(field, tableName);
        }

        public static async Task<BsonDocument> GetLastRecord(string field, string tableName)
        {
            return await GetHelper.GetLastRecord(field, tableName);
        }

        public static BsonDocument GetLastRecordSync(string field, string tableName)
        {
            return GetHelper.GetLastRecordSync(field, tableName);
        }

        public static BsonDocument FindAndModify(string field, string tableName)
        {
            return GetHelper.FindAndModify(field, tableName);
        }
    }
}
