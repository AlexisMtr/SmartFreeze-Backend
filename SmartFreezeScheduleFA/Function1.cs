using System;
using Autofac;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using SmartFreezeScheduleFA.Configurations;
using SmartFreezeScheduleFA.Services;

namespace SmartFreezeScheduleFA
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static void Run([TimerTrigger("0 0 */3 * * *")]TimerInfo myTimer, TraceWriter log)
        {
            log.Info($"C# Timer trigger function executed at: {DateTime.Now}");
            ServiceLocator.ConfigureDI();

            using (var scope = ServiceLocator.Container.BeginLifetimeScope())
            {
                CommunicationStateService service = scope.Resolve<CommunicationStateService>();
                int minMin = 3 * 60 + 5;
                int minMax = 4 * 60 + 5;
                service.CheckDeviceCommunication(minMin, minMax);
            }
        }
    }
}
