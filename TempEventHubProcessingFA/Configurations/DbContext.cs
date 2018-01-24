using MongoDB.Driver;
using System.Security.Authentication;

namespace TempEventHubProcessingFA.Configurations
{
    public class DbContext
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
    }
}
