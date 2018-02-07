using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.ServiceBus;
using SmartFreezeFA.Configurations;
using SmartFreezeFA.Models;
using SmartFreezeFA.Parsers;
using System.Collections.Generic;

namespace SmartFreezeFA
{
    public static class TelemetryFunction
    {
        [FunctionName("TelemetryFunction")]
        public static void Run([EventHubTrigger("device-data", Connection = "EventHubConnectionString")]string myEventHubMessage, TraceWriter log)
        {
            log.Info($"C# Event Hub trigger function processed a message: {myEventHubMessage}");
            DependencyInjection.ConfigureInjection();

            IEnumerable<Telemetry> telemetries = FrameParser.Parse(myEventHubMessage);
        }
    }
}
