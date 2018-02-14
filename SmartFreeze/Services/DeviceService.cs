using SmartFreeze.Filters;
using SmartFreeze.Models;
using SmartFreeze.Repositories;
using System;

namespace SmartFreeze.Services
{
    public class DeviceService
    {
        private readonly IDeviceRepository deviceRepository;

        public DeviceService(IDeviceRepository deviceRepository)
        {
            this.deviceRepository = deviceRepository;
        }

        public Device Get(string deviceId)
        {
            return deviceRepository.Get(deviceId);
        }

        public PaginatedItems<Device> GetAll(DeviceFilter filter, int rowsPerPage, int pageNumber)
        {
            return deviceRepository.GetAllPaginated(filter, rowsPerPage, pageNumber);
        }

        public Device Create(Device device, string siteId)
        {
            return deviceRepository.Create(device, siteId);
        }

        public bool Update(string deviceId, Device device)
        {
            return deviceRepository.Update(deviceId, device);
        }


        public bool Delete(string deviceId)
        {
            return deviceRepository.Delete(deviceId);
        }


        public void AddAlarm(string deviceId, Alarm alarm)
        {
            deviceRepository.AddAlarm(deviceId, alarm);
        }


        public bool Managefavorite(string deviceId, bool isFavorite)
        {
            return deviceRepository.Managefavorite(deviceId,isFavorite);
        }


        public bool UpdateLastCommunication(string deviceId, DateTime lastDate)
        {
            return deviceRepository.UpdateLastCommunication(deviceId, lastDate);
        }

    }
}
