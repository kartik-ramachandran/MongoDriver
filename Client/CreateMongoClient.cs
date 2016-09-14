using MongoDB.Driver;
using MongoDriver.Constant;

namespace MongoDriver
{
    public class CreateMongoClient 
    {
        static MongoClient client = null;

        public CreateMongoClient()
        {
            client = new MongoClient();
        }        

        public static IMongoClient Get()
        {
            return client;
        }

        public static IMongoClient GetClient()
        {
            return new MongoClient(); 
        }

        public static IMongoClient GetClientFromUrl()
        {
            var mongoUrl = MongoConstant.MongoUrl;

            return new MongoClient(mongoUrl);
        }

        public static IMongoClient GetClientFromConnString()
        {
            var mongoConnString = MongoConstant.MongoConnectionString;

            return new MongoClient(mongoConnString);
        }

        public static IMongoClient GetClientWithSettings()
        {
            var collection = MongoConstant.CollectionName;
            var userName = MongoConstant.UserName;
            var password = MongoConstant.Password;

            var adminCredentials = MongoCredential.CreateCredential(collection, userName, password);            

            var clientSettings = new MongoClientSettings
            {
                Credentials = new[] { adminCredentials }
            };

            // use the below when admin module is ready
           // clientSettings.ClusterConfigurator = builder => builder.Subscribe(new Monitor());   

            return new MongoClient(clientSettings);
        }    
   }
}
