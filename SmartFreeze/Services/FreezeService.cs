using SmartFreeze.Models;
using SmartFreeze.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartFreeze.Services
{
    public class FreezeService
    {
        private readonly IFreezeRepository freezeRepository;
        private readonly ISiteRepository siteRepository;

        public FreezeService(IFreezeRepository freezeRepository, ISiteRepository siteRepository)
        {
            this.freezeRepository = freezeRepository;
            this.siteRepository = siteRepository;
        }

        public IEnumerable<Freeze> GetFreezeOnSite(string siteId)
        {
            DateTime from;
            if(DateTime.UtcNow.Hour < 12)
            {
                from = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, 0, 0, 0, DateTimeKind.Utc);
            }
            else
            {
                from = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, 12, 0, 0, DateTimeKind.Utc);
            }

            Site site = siteRepository.Get(siteId);
            Dictionary<string, IEnumerable<Freeze>> freezeByDevice = freezeRepository.GetByDevice(site.Devices.Select(e => e.Id), from);
            if (freezeByDevice == null) return new List<Freeze>();

            IList<Freeze> freezeForecast = new List<Freeze>();
            IEnumerable<DateTime> dates = freezeByDevice.Values.SelectMany(e => e.Select(i => i.Date)).Distinct();

            foreach (DateTime date in dates)
            {
                IEnumerable<Freeze> freezeAtDate = freezeByDevice.Values.SelectMany(e => e.Where(i => i.Date == date));
                freezeForecast.Add(new Freeze
                {
                    Date = date,
                    TrustIndication = freezeAtDate.Max(e => e.TrustIndication),
                    DeviceId = freezeAtDate.First(e => e.TrustIndication == freezeAtDate.Max(i => i.TrustIndication)).DeviceId
                });
            }

            return freezeForecast;
        }

        public IEnumerable<Freeze> GetFreezeOnDevice(string deviceId)
        {
            DateTime from;
            if (DateTime.UtcNow.Hour < 12)
            {
                from = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, 0, 0, 0);
            }
            else
            {
                from = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, 12, 0, 0);
            }
            return freezeRepository.GetByDevice(deviceId, from);
        }
    }
}
