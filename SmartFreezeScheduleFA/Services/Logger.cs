using Microsoft.Azure.WebJobs.Host;
using System;
using WeatherLibrary.Abstraction;

namespace SmartFreezeScheduleFA.Services
{
    public class Logger : ILogger
    {
        private readonly TraceWriter logger;

        public Logger(TraceWriter logger)
        {
            this.logger = logger;
        }

        public void Error(string message, Exception exception = null)
        {
            logger.Error(message, exception, null);
        }

        public void Info(string message)
        {
            logger.Info(message);
        }

        public void Warning(string message)
        {
            logger.Warning(message);
        }
    }
}
