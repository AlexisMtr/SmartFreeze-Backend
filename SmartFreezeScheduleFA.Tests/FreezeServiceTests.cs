using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SmartFreezeScheduleFA.Repositories;
using SmartFreezeScheduleFA.Services;
using WeatherLibrary.Algorithmes.Freeze;

namespace SmartFreezeScheduleFA.Tests
{
    [TestClass]
    public class FreezeServiceTests
    {
        [TestMethod]
        public void TestCreateFreezeAndThawByDevice()
        {
            //GIVEN
            string deviceId = "1";
            DateTime date = new DateTime(2018, 02, 13, 0, 0, 0);
            DateTime date2 = date.AddHours(12);
            Dictionary<DateTime, FreezeForecast.FreezingProbability> dicoPredictionBy12h = new Dictionary<DateTime, FreezeForecast.FreezingProbability>
            {
                {date, FreezeForecast.FreezingProbability.IMMINENT},
                {date2, FreezeForecast.FreezingProbability.ZERO}
            };
            Mock<IFreezeRepository> freezeRepo = new Mock<IFreezeRepository>();

            //WHEN
            FreezeService service = new FreezeService(freezeRepo.Object);
            service.CreateFreezeAndThawByDevice(deviceId, dicoPredictionBy12h);

            //THEN
            freezeRepo.Verify(o => o.AddFreeze("1", new DateTime(2018, 02, 13, 0, 0, 0), (int)FreezeForecast.FreezingProbability.IMMINENT));
            freezeRepo.Verify(o => o.AddFreeze("1", new DateTime(2018, 02, 13, 12, 0, 0), (int)FreezeForecast.FreezingProbability.ZERO));
        }
    }
}
