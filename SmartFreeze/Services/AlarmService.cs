﻿using SmartFreeze.Filters;
using SmartFreeze.Models;
using SmartFreeze.Repositories;
using System;

namespace SmartFreeze.Services
{
    public class AlarmService
    {
        private readonly IAlarmRepository alarmRepository;


        public AlarmService(IAlarmRepository alarmRepository)
        {
            this.alarmRepository = alarmRepository;
        }

        public PaginatedItems<Alarm> GetAll(IMongoFilter<Device, Alarm> filter, int rowsPerPage, int pageNumber)
        {
            DeviceAlarmFilter alarmFilter = new DeviceAlarmFilter
            {
                Context = (filter as AlarmFilter).Context,
                AlarmType = (filter as AlarmFilter).AlarmType,
                Gravity = (filter as AlarmFilter).Gravity,
                DeviceId = string.Empty,
                IsRead = (filter as AlarmFilter).IsRead,
                IsActive = (filter as AlarmFilter).IsActive
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

        public int CountAll(IMongoFilter<Device, Alarm> filter)
        {
            DeviceAlarmFilter alarmFilter = new DeviceAlarmFilter
            {
                Context = (filter as AlarmFilter).Context,
                AlarmType = (filter as AlarmFilter).AlarmType,
                Gravity = (filter as AlarmFilter).Gravity,
                DeviceId = string.Empty,
                IsRead = (filter as AlarmFilter).IsRead,
                IsActive = (filter as AlarmFilter).IsActive
            };
            return alarmRepository.Count(alarmFilter);
        }

        public PaginatedItems<Alarm> GetByDevice(string deviceId, IMongoFilter<Device, Alarm> filter, int rowsPerPage, int pageNumber)
        {
            DeviceAlarmFilter alarmFilter = new DeviceAlarmFilter
            {
                Context = (filter as AlarmFilter).Context,
                AlarmType = (filter as AlarmFilter).AlarmType,
                Gravity = (filter as AlarmFilter).Gravity,
                DeviceId = deviceId,
                IsRead = (filter as AlarmFilter).IsRead,
                IsActive = (filter as AlarmFilter).IsActive
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

        public bool Ack(string alarmId)
        {
            return alarmRepository.SetAlarmToRead(alarmId);
        }
    }
}
