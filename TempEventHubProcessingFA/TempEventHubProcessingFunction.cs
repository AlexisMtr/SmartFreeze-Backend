using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.ServiceBus;
using System;
using System.Collections.Generic;
using System.Configuration;
using TempEventHubProcessingFA.Configurations;
using TempEventHubProcessingFA.Models;
using TempEventHubProcessingFA.Parsers;

namespace TempEventHubProcessingFA
{
    public static class TempEventHubProcessingFunction
    {
        [FunctionName("TempEventHubProcessingFunction")]
        public static void Run([EventHubTrigger("EventHubProcessing", Connection = "EventHubConnectionString")]string myEventHubMessage, TraceWriter log)
        {
            log.Info($"C# Event Hub trigger function processed a message: {myEventHubMessage}");
            var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnectionString"].ConnectionString;
            var dbName = ConfigurationManager.AppSettings["DatabaseName"];
            var context = new DbContext(connectionString, dbName);

            try
            {
                IEnumerable<Telemetry> message = FrameParser.Parse(myEventHubMessage);

                foreach (var msg in message)
                {
                    context.Database.GetCollection<Telemetry>(nameof(Telemetry)).InsertOne(msg);
                }
            }
            catch(FormatException formatEx)
            {
                log.Error(formatEx.Message);
            }
        }
    }
}
