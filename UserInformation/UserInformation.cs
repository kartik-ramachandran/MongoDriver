using MongoDB.Bson;
using MongoDB.Driver;
using MongoDriver.Database;
using System;
using System.Threading.Tasks;

namespace MongoDriver.UserInformation
{
    public class UserInformation 
    {
        public static IMongoDatabase MongoDatabase
        {
            get { return Connect.GetDatabase(); }
        }

        public static string MongoDatabaseName
        {
            get { return Connect.GetDatabaseName(); }
        } 

        public static async Task<BsonDocument> UserList(string userName = null, string databaseName = null, bool showPriveleges = false, bool showCredentials = false)
        {
            BsonDocument command = null;         

            var userInfo = new BsonArray { new BsonDocument
                {
                    { "user", userName == null ? "" :  userName},
                    { "db", databaseName == null ? MongoDatabaseName : "" }
                }};


            if ((userName == null && databaseName == null) || (userName == null && databaseName != null))
            {
                //get all users
                command = new BsonDocument("usersInfo", 1); //gets all user
            }

            if (userName != null && databaseName == null)
            {
                command = new BsonDocument("usersInfo", userInfo); 
            }

            if (userName != null && databaseName != null)
            {
                if (!showPriveleges && !showCredentials)
                {
                    command = new BsonDocument { { "usersInfo", userInfo } };
                }                

                if (showPriveleges && !showCredentials)
                {
                    command = new BsonDocument { { "usersInfo", userInfo }, { "showPrivileges", true } };
                }

                if (!showPriveleges && showCredentials)
                {
                    command = new BsonDocument { { "usersInfo", userInfo }, { "showCredentials", true } };
                }

                if (showPriveleges && showCredentials)
                {
                    command = new BsonDocument { { "usersInfo", userInfo }, { "showPrivileges", true }, { "showCredentials", true } };
                }
            }

            return await MongoDatabase.RunCommandAsync<BsonDocument>(command);
        }

        //Creates user with read write -- pass BsonDocument to BsonArray
        public static async void CreateUser(string userName, string password, BsonArray role, string database)
        {
            try
            {
                // Construct the write concern
                var writeConcern = WriteConcern.WMajority.With(wTimeout: TimeSpan.FromMilliseconds(5000));

                // Construct the createUser command.
                var command = new BsonDocument {
                    { "createUser", userName },
                    { "pwd", password },
                    { "roles", role },
                    { "db", database}
                };

                // Run the command. If it fails, an exception will be thrown.
                await MongoDatabase.RunCommandAsync<BsonDocument>(command);
            }

            catch (Exception) { }
        }

        public static async void CreateUserWithCustomRole(string userName, string password, string roleName, string database)
        {
            try
            {
                // Construct the write concern
                var writeConcern = WriteConcern.WMajority.With(wTimeout: TimeSpan.FromMilliseconds(5000));

                var roleDocument = await CreateCustomRoles(roleName);

                // Construct the createUser command.
                var command = new BsonDocument {
                    { "createUser", userName },
                    { "pwd", password },
                    { "roles", roleDocument },
                    { "db", database}
                };

                // Run the command. If it fails, an exception will be thrown.
                await MongoDatabase.RunCommandAsync<BsonDocument>(command);
            }

            catch (Exception) { }
        }


        public static async Task<BsonDocument> UpdateUser(string userName, string password, BsonArray role, string database)
        {
            var command = new BsonDocument
            {
                { "auth", userName },
                { "pwd", password }
            };

            var userPresent = await MongoDatabase.RunCommandAsync<int>(command);

            if (userPresent == 0) return null;

            // Construct the updateUser command.
            command = new BsonDocument
            {
                { "updateUser", userName },                                
                { "roles", role }
            };

            return await MongoDatabase.RunCommandAsync<BsonDocument>(command);
        }

        public static async Task<BsonDocument> DroppingUsers(string userName)
        {
            // Construct the updateUser command.
            var command = "{dropUser: "+ userName + "," +
                            "writeConcern: { w: 'majority', wtimeout: 5000 }}";

            return await MongoDatabase.RunCommandAsync<BsonDocument>(command);
        }

        public static async Task<BsonDocument> CreateCustomRoles(string roleName)
        {
            // Construct the updateUser command.
            var command = new BsonDocument
            {
                { "createRole", roleName },
                { "privileges", new BsonArray
                {
                     new BsonDocument
                        {
                            //{ "w", "majority" },
                            //{ "wtimeout", "2000 " }
                        },
                }},
                { "roles", new BsonArray
                {
                     new BsonDocument
                        {
                           { "role", "read" },
                           { "db", "admin" }
                        },
                }},
                { "writeConcern", new BsonArray
                {
                    new BsonDocument
                    {
                        { "w", "majority" },
                        { "wtimeout", "2000 " }
                    },
                }},
            };

            return await MongoDatabase.RunCommandAsync<BsonDocument>(command);
        }

        public static async Task<BsonDocument> GrantRoles(string userName, BsonArray roles)
        {
            // Construct the updateUser command.
            var command = new BsonDocument
            {
                { "grantRolesToUser", userName },                                
                { "roles", roles },
                { "writeConcern", new BsonArray
                    {
                        new BsonDocument
                        {
                            { "w", "majority" },
                            { "wtimeout", "2000 " }
                        },
                    } },
            };

            return await MongoDatabase.RunCommandAsync<BsonDocument>(command);
        }

        public static async Task<BsonDocument> RevokeRoles(string userName, BsonArray roles)
        {
            var command = new BsonDocument
            {
                { "revokeRolesFromUser", userName },
                { "roles", roles },
                { "writeConcern", new BsonArray
                    {
                        new BsonDocument
                        {
                            { "w", "majority" },
                            { "wtimeout", "2000 " }
                        },
                    } },
            };

            return await MongoDatabase.RunCommandAsync<BsonDocument>(command);
        }
    }
}
