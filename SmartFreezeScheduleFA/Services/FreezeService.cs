using SmartFreezeScheduleFA.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherLibrary.Algorithmes.Freeze;

namespace SmartFreezeScheduleFA.Services
{
    public class FreezeService
    {
        private readonly IFreezeRepository freezeRepository;

        public FreezeService(IFreezeRepository freezeRepository)
        {
            this.freezeRepository = freezeRepository;
        }

        public void CreateFreezeAndThawByDevice(string deviceId, Dictionary<DateTime, FreezeForecast.FreezingProbability> dicoPredictionBy12h)
        {
            foreach (var prediction in dicoPredictionBy12h)
            {
                freezeRepository.AddFreeze(deviceId, prediction.Key, (int)prediction.Value);
            }
        }
    }
}
