using System;
using Autofac;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using SmartFreezeScheduleFA.Configurations;
using SmartFreezeScheduleFA.Services;

namespace SmartFreezeScheduleFA
{
    public static class Schedule8Hours
    {
        [FunctionName("Schedule8Hours")]
        public static void Run([TimerTrigger("0 5 */8 * * *")]TimerInfo myTimer, TraceWriter log)
        {
            log.Info($"C# Timer trigger function executed at: {DateTime.Now}");
            DependencyInjection.ConfigureInjection(log);

            using (var scope = DependencyInjection.Container.BeginLifetimeScope())
            {
                CommunicationStateService service = scope.Resolve<CommunicationStateService>();
                service.Run(7, null, Models.Alarm.Gravity.Critical);
            }

        }
    }
}
