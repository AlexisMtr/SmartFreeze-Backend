using SmartFreeze.Filters;
using SmartFreeze.Models;
using SmartFreeze.Repositories;
using System;

namespace SmartFreeze.Services
{
    public class AlarmService
    {
        private readonly AlarmRepository alarmRepository;

        public AlarmService(AlarmRepository alarmRepository)
        {
            this.alarmRepository = alarmRepository;
        }

        public PaginatedItems<Alarm> GetAll(IMongoFilter<Device, Alarm> filter, int rowsPerPage, int pageNumber)
        {
            DeviceAlarmFilter alarmFilter = new DeviceAlarmFilter
            {
                AlarmType = (filter as AlarmFilter).AlarmType,
                Gravity = (filter as AlarmFilter).Gravity,
                DeviceId = string.Empty
            };

            var totalCount = alarmRepository.Count(alarmFilter);
            var pageCount = rowsPerPage == 0 ? 1 : (int)Math.Ceiling((double)totalCount / rowsPerPage);

            return new PaginatedItems<Alarm>
            {
                PageCount = pageCount,
                TotalItemsCount = totalCount,
                Items = alarmRepository.Get(alarmFilter, rowsPerPage, pageNumber)
            };
        }
        
        public PaginatedItems<Alarm> GetByDevice(string deviceId, IMongoFilter<Device, Alarm> filter, int rowsPerPage, int pageNumber)
        {
            DeviceAlarmFilter alarmFilter = new DeviceAlarmFilter
            {
                AlarmType = (filter as AlarmFilter).AlarmType,
                Gravity = (filter as AlarmFilter).Gravity,
                DeviceId = deviceId
            };
            
            var totalCount = alarmRepository.CountByDevice(deviceId, alarmFilter);
            var pageCount = rowsPerPage == 0 ? 1 : (int)Math.Ceiling((double)totalCount / rowsPerPage);

            return new PaginatedItems<Alarm>
            {
                PageCount = pageCount,
                TotalItemsCount = totalCount,
                Items = alarmRepository.GetByDevice(deviceId, alarmFilter, rowsPerPage, pageNumber)
            };
        }
    }
}
