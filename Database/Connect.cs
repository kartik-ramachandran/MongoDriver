using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using MongoDriver.Constant;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MongoDriver.Database
{
    public class Connect
    {
        private static string DatabaseName
        {
            get
            {
                return MongoConstant.DatabaseName;
            }
        }

        private static IMongoClient MongoClient
        {
            get
            {
                return CreateMongoClient.GetClientFromConnString();
            }
        }

        public static IMongoDatabase GetDatabase()
        {
            return MongoClient.GetDatabase(DatabaseName);
        }

        public static string GetDatabaseName()
        {
            return DatabaseName;
        }

        public static GridFSBucket GetGridFs()
        {            
            //TODO
            GridFSBucket gridFs = new GridFSBucket(MongoClient.GetDatabase(DatabaseName), new GridFSBucketOptions
            {
                BucketName = "Docs",
                ChunkSizeBytes = 1048576, // 1MB
                WriteConcern = WriteConcern.WMajority,
                ReadPreference = ReadPreference.Secondary
            });

            return gridFs;
        }

        public static async void DropDatabase()
        { 
            await MongoClient.DropDatabaseAsync(DatabaseName);
        }

        public static async Task<List<BsonDocument>> ListDatabase()
        {
            using (var cursor = await MongoClient.ListDatabasesAsync())
            {
                return await cursor.ToListAsync();                
            }
        }       
    }
}
