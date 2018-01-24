using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SmartFreeze.Configurations;
using System.Security.Authentication;

namespace SmartFreeze.Context
{
    public class SmartFreezeContext
    {
        public readonly IMongoClient Client;
        public readonly IMongoDatabase Database;

        public SmartFreezeContext(IOptions<ContextSettings> contextSettings)
        {
            var settings = MongoClientSettings.FromUrl(new MongoUrl(contextSettings.Value.DefaultConnectionString));
            settings.SslSettings = new SslSettings { EnabledSslProtocols = SslProtocols.Tls12 };

            this.Client = new MongoClient(settings);
            this.Database = Client.GetDatabase(contextSettings.Value.DefaultDbName);
        }
    }
}
