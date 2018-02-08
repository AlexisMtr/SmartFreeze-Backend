using Autofac.Core;
using MongoDB.Driver;
using System.Security.Authentication;

namespace SmartFreezeScheduleFA.Configurations
{
    public class DbContext : Service
    {
        public readonly IMongoClient Client;
        public readonly IMongoDatabase Database;



        public DbContext(string connectionString, string dbName)
        {
            var settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));
            settings.SslSettings = new SslSettings { EnabledSslProtocols = SslProtocols.Tls12 };

            this.Client = new MongoClient(settings);
            this.Database = Client.GetDatabase(dbName);
        }

        public override string Description => "Mongo Database Context";
    }
}
