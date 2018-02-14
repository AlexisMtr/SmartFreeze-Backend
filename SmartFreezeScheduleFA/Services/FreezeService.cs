using SmartFreezeScheduleFA.Models;
using SmartFreezeScheduleFA.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using static WeatherLibrary.Algorithmes.Freeze.FreezeForecast;

namespace SmartFreezeScheduleFA.Services
{
    public class FreezeService
    {
        private readonly IFreezeRepository freezeRepository;

        public FreezeService(IFreezeRepository freezeRepository)
        {
            this.freezeRepository = freezeRepository;
        }

        public void CreateFreezeAndThawByDevice(string deviceId, Dictionary<DateTime, FreezingProbability> dicoPredictionBy12h)
        {
            IList<Freeze> freezeList = new List<Freeze>();
            foreach (var prediction in dicoPredictionBy12h)
            {
                freezeList.Add(new Freeze
                {
                    DeviceId = deviceId,
                    Date = prediction.Key,
                    TrustIndication = (int)prediction.Value
                });
            }

            freezeRepository.AddFreeze(freezeList.OrderBy(e => e.Date));
        }
    }
}
