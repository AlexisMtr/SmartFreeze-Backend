using System;
using Autofac;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using SmartFreezeScheduleFA.Configurations;
using SmartFreezeScheduleFA.Services;

namespace SmartFreezeScheduleFA
{
    public static class ScheduleHours
    {
        [FunctionName("ScheduleHours")]
        public static void Run([TimerTrigger("0 */5 */1 * * *")]TimerInfo myTimer, TraceWriter log)
        {
            log.Info($"C# Timer trigger function executed at: {DateTime.Now}");
            DependencyInjection.ConfigureInjection();

            using (var scope = DependencyInjection.Container.BeginLifetimeScope())
            {
                CommunicationStateService service = scope.Resolve<CommunicationStateService>();
                service.Run(1, 2, Models.Alarm.Gravity.Information);
            }

        }
    }
}
