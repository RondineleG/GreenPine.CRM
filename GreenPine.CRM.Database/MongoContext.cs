using GreenPine.CRM.Model.Database;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using System.Collections.Generic;

namespace GreenPine.CRM.Database
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
            Companies = Database.GetCollection<Organization>("organizations");
            Types = Database.GetCollection<InstanceType>("types");
            Instances = Database.GetCollection<Dictionary<string, string>>("instances");
        }

        //Define the collections which are accessible
        public IMongoCollection<User> Users { get; set; }
        public IMongoCollection<Ping> Pings { get; set; }
        public IMongoCollection<Organization> Companies { get; set; }
        public IMongoCollection<InstanceType> Types { get; set; }
        public IMongoCollection<Dictionary<string, string>> Instances { get; set; }
    }
}
