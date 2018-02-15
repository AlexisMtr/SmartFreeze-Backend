using SmartFreeze.Filters;
using SmartFreeze.Models;
using System;

namespace SmartFreeze.Repositories
{
    public interface IDeviceRepository
    {
        Device Get(string deviceId);

        PaginatedItems<Device> GetAllPaginated(IMongoFilter<Site, Device> filter, int rowsPerPage, int pageNumber);

        Device Create(Device device, string siteId);

        bool Update(string deviceId, Device device);


        bool Delete(string deviceId);


        void AddAlarm(string deviceId, Alarm alarm);


        bool Managefavorite(string deviceId, bool isFavorite);


        bool UpdateLastCommunication(string deviceId, DateTime lastDate);
    }
}
