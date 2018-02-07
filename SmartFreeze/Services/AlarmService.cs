using SmartFreeze.Filters;
using SmartFreeze.Models;
using SmartFreeze.Repositories;

namespace SmartFreeze.Services
{
    public class AlarmService
    {
        private readonly AlarmRepository alarmRepository;

        public AlarmService(AlarmRepository alarmRepository)
        {
            this.alarmRepository = alarmRepository;
        }

        public PaginatedItems<Alarm> GetBySite(string siteId, IMongoFilter<Site, Alarm> filter, int rowsPerPage, int pageNumber)
        {
            return alarmRepository.GetBySite(siteId, filter, rowsPerPage, pageNumber);
        }

        public PaginatedItems<Alarm> GetByDevice(string deviceId, IMongoFilter<Device, Alarm> filter, int rowsPerPage, int pageNumber)
        {
            return alarmRepository.GetByDevice(deviceId, filter, rowsPerPage, pageNumber);
        }
    }
}
