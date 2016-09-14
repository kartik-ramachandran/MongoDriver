using MongoDriver.Helper;
using System.Threading.Tasks;

namespace MongoDriver.Action
{    
    public class Delete
    {
        public static async Task<bool> RemoveDocument<T>(string field, T value, string tableName)
        {
            return await DeleteHelper.RemoveDocument(field, value, tableName);
        }

        public static async Task<bool> RemoveCollection(string tableName)
        {
            return await DeleteHelper.RemoveCollection(tableName);
        }
    }
}
