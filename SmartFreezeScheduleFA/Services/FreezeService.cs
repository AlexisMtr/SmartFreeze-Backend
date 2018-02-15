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

        public void CreateFreezeAndThawByDevice(string deviceId, Dictionary<DateTime, FreezingProbability> freezeProbability)
        {
            foreach (var prediction in freezeProbability)
            {
                freezeRepository.AddOrUpdateFreeze(deviceId, prediction.Key, (int)prediction.Value);
            }
        }

        public Dictionary<DateTime, FreezingProbability> CalculAverageFreezePrediction12h(Dictionary<DateTime, FreezingProbability> freezingProbabilityList)
        {
            Dictionary<DateTime, FreezingProbability> averageFreezePrediction12h = new Dictionary<DateTime, FreezingProbability>();

            DateTime start = new DateTime();

            if (freezingProbabilityList.First().Key.Hour < 12)
            {
                start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0, DateTimeKind.Utc); //AM
            }
            else
            {
                start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 12, 0, 0, DateTimeKind.Utc); //PM
            }
            DateTime end = start.AddHours(12);

            while (start <= freezingProbabilityList.Last().Key)
            {
                //prédictions pour la demie-journée en cours
                Dictionary<DateTime, int> predictionsForOneHalfDay = freezingProbabilityList
                    .Where(e => e.Key >= start && e.Key < end)
                    .ToDictionary(k => k.Key, v => (int)v.Value);

                //calcule la moyenne des prédictions sur cette demie-journée
                double avg = predictionsForOneHalfDay.Values.Average();
                double avgRounded = Math.Round(avg);

                averageFreezePrediction12h.Add(start, (FreezingProbability)(avgRounded));

                start = start.AddHours(12);
                end = end.AddHours(12);
            }
            return averageFreezePrediction12h;
        }
    }
}
