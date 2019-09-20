using Microsoft.Extensions.Configuration;
using UintaPine.CRM.Model.Database;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace UintaPine.CRM.Database
{
    public class MongoContext
    {
        private MongoClient _client { get; set; }


        /// <summary>
        /// Options from Startup, used to setup db connection
        /// </summary>
        /// <param name="settings"></param>
        public MongoContext(IConfiguration configuration)
        {
            //Connect to the database
            _client = new MongoClient(configuration.GetConnectionString("MongoDBConnectionString"));

            Setup(configuration["DatabaseName"]);
        }
              
        private void Setup(string database)
        {
            ConventionPack pack = new ConventionPack()
            {
                new IgnoreExtraElementsConvention(true)
            };
            ConventionRegistry.Register("IgnoreExtraElements", pack, t => true);

            IMongoDatabase Database = _client.GetDatabase(database);

            //Link the accessible collections to actual DB collections
            Users = Database.GetCollection<User>("users");
            Pings = Database.GetCollection<Ping>("pings");
        }

        //Define the collections which are accessible
        public IMongoCollection<User> Users { get; set; }
        public IMongoCollection<Ping> Pings { get; set; }
    }
}
