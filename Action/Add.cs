using MongoDB.Bson;
using MongoDriver.Helper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MongoDriver.Action
{
    public class Add
    {      
        public async static Task Insert(string tableName, BsonDocument data)
        {
            await InsertHelper.Insert(data, tableName);
        }

        public static void InsertSync(string tableName, BsonDocument data)
        {
            InsertHelper.InsertSync(data, tableName);
        }

        public async static Task InsertMany(string tableName, List<BsonDocument> data)
        {
            await InsertHelper.InsertManyDocument(data, tableName);
        }

        public async static Task<bool> CheckIfFieldExists(string tableName, string idField, string value)
        {
            var doesFieldExist =  await GetHelper.Get<string, string>(idField, value, tableName);

            if (doesFieldExist != null) return true;

            return false;            
        }

        public async static Task UpsertMany(string tableName, string idField, List<string> values, List<BsonDocument> data)
        {
            foreach(var value in values)
            {
                var doesFieldExist = await CheckIfFieldExists(idField, value, tableName);

                if (doesFieldExist)
                {
                    //do nothing
                }

                await InsertMany(tableName, data);
            }              
        }

        //check before inserting row in the table
        public async static Task Upsert(string tableName, string idField, string value, BsonDocument data)
        {
            var doesFieldExist = await CheckIfFieldExists(idField, value, tableName);

            if (doesFieldExist) return;

            await Insert(tableName, data);
        }
    }
}
