using System;
using Autofac;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using SmartFreezeScheduleFA.Configurations;
using SmartFreezeScheduleFA.Services;

namespace SmartFreezeScheduleFA
{
    public static class Schedule7Hours
    {
        [FunctionName("Schedule7Hours")]
        public static void Run([TimerTrigger("0 */5 */7 * * *")]TimerInfo myTimer, TraceWriter log)
        {
            log.Info($"C# Timer trigger function executed at: {DateTime.Now}");
            DependencyInjection.ConfigureInjection();

            using (var scope = DependencyInjection.Container.BeginLifetimeScope())
            {
                CommunicationStateService service = scope.Resolve<CommunicationStateService>();
                service.Run(7, 8, Models.Alarm.Gravity.Critical);
            }

        }
    }
}
