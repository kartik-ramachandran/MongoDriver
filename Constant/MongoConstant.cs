using System.Configuration;

namespace MongoDriver.Constant
{
    public class MongoConstant
    {
        public static string MongoUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["MongoUrl"].ToString();                
            }
        }

        public static string MongoConnectionString
        {
            get
            {
                return ConfigurationManager.AppSettings["MongoConnectionString"].ToString();
            }
        }

        public static string UserName
        {
            get
            {
                return ConfigurationManager.AppSettings["Username"].ToString();
            }
        }

        public static string Password
        {
            get
            {
                return ConfigurationManager.AppSettings["Password"].ToString();
            }
        }

        public static string CollectionName
        {
            get
            {
                return ConfigurationManager.AppSettings["CollectionName"].ToString();
            }
        }

        public static string DatabaseName
        {
            get
            {
                return ConfigurationManager.AppSettings["DatabaseName"].ToString();
            }
        }
    }
}
