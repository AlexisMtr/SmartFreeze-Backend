using SmartFreeze.Filters;
using SmartFreeze.Models;
using SmartFreeze.Repositories;
using System.Collections.Generic;

namespace SmartFreeze.Services
{
    public class DeviceService
    {
        private readonly DeviceRepository deviceRepository;

        public DeviceService(DeviceRepository deviceRepository)
        {
            this.deviceRepository = deviceRepository;
        }

        public Device Get(string deviceId)
        {
            return deviceRepository.Get(deviceId);
        }

        public PaginatedItems<Device> GetAll(DeviceFilter filter, int rowsPerPage = 20, int pageNumber = 1)
        {
            return deviceRepository.GetAllPaginated(filter, rowsPerPage, pageNumber);
        }
    }
}
