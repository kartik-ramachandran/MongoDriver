using MongoDriver.Helper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MongoDriver.Indexes
{  
    public class Indexes
    {
        public static async Task<string> CreateSingleFieldIndex(string field, string tableName)
        {
            var chkIfIndexExists = await CheckIfIndexExists(field, tableName);

            if (chkIfIndexExists) return string.Empty;

            return await IndexHelper.CreateSingleFieldIndex(field, tableName);
        }

        public static async Task<string> CreateComplexFieldIndex(string field, string tableName)
        {
            return await IndexHelper.CreateSingleFieldIndex(field, tableName);
        }

        public static async Task<string> CreateComplexFieldIndex(List<string> field, string tableName)
        {
            return await IndexHelper.CreateComplexFieldIndex(field, tableName);
        }

        public static async Task<List<string>> GetIndexes(string tableName)
        {
            var indexList = new List<string>();

            var allIndexes = await IndexHelper.GetIndexes(tableName);

            foreach (var index in allIndexes)
            {
                indexList.Add(index["name"].ToString());
            }

            return indexList;
        }

        public static async Task<bool> CheckIfIndexExists(string fieldName, string tableName)
        {
            var indexList = new List<string>();

            var allIndexes = await GetIndexes(tableName);

            return allIndexes.Exists(i => i == fieldName);
        }
    }
}
