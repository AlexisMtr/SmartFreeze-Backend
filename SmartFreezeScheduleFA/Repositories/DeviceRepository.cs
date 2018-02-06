using MongoDB.Driver;
using MongoDB.Driver.Linq;
using SmartFreezeScheduleFA.Extensions;
using SmartFreezeScheduleFA.Configurations;
using SmartFreezeScheduleFA.Filters;
using SmartFreezeScheduleFA.Models;
using SmartFreezeScheduleFA.Repositories;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Linq.Expressions;

namespace SmartFreezeScheduleFA.Repositories
{
    public class DeviceRepository
    {
        private readonly IMongoCollection<Site> collection;

        public DeviceRepository(DbContext context)
        {
            this.collection = context.Database
                .GetCollection<Site>(nameof(Site));
        }

        public Device Get(string deviceId)
        {
            return collection.AsQueryable()
                .SelectMany(e => e.Devices)
                .FirstOrDefault(e => e.Id == deviceId);
        }

        public IEnumerable<Device> GetFailsCommunicationBetween(int minBundaryMin, int? maxBoundaryMin = null)
        {
            DateTime maxDate = DateTime.Now.AddMinutes(-minBundaryMin);

            Expression<Func<Device, bool>> expression;
            if(maxBoundaryMin.HasValue)
            {
                DateTime minDate = DateTime.Now.AddMinutes(-maxBoundaryMin.Value);
                expression = e => e.LastCommunication < maxDate && e.LastCommunication > minDate;
            }
            else
            {
                expression = e => e.LastCommunication < maxDate;
            }

            return collection.AsQueryable()
                .SelectMany(e => e.Devices)
                .Where(expression);
        }


    }
}
