using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.ServiceBus;
using MongoDB.Bson;
using Newtonsoft.Json;
using System.Configuration;
using TempEventHubProcessingFA.Configurations;

namespace TempEventHubProcessingFA
{
    public static class TempEventHubProcessingFunction
    {
        [FunctionName("TempEventHubProcessingFunction")]
        public static void Run([EventHubTrigger("EventHubProcessing", Connection = "EventHubConnectionString")]string myEventHubMessage, TraceWriter log)
        {
            log.Info($"C# Event Hub trigger function processed a message: {myEventHubMessage}");

            var context = new DbContext(ConfigurationManager.ConnectionStrings["DefaultConnectionString"].ConnectionString, ConfigurationManager.AppSettings["DatabaseName"]);

            dynamic message = JsonConvert.DeserializeObject<dynamic>(myEventHubMessage);

            context.Database.GetCollection<BsonDocument>("EventHubMessages").InsertOne(message);
        }
    }
}
