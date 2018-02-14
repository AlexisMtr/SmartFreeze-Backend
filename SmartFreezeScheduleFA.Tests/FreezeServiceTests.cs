using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SmartFreezeScheduleFA.Models;
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
            freezeRepo.Setup(o => o.AddFreeze(It.IsAny<IEnumerable<Freeze>>())).Verifiable();

            //WHEN
            FreezeService service = new FreezeService(freezeRepo.Object);
            service.CreateFreezeAndThawByDevice(deviceId, dicoPredictionBy12h);

            //THEN
            IEnumerable<Freeze> freezeList = new List<Freeze>
            {
                new Freeze { DeviceId = "1", Date = date, TrustIndication = (int)FreezeForecast.FreezingProbability.IMMINENT },
                new Freeze { DeviceId = "1", Date = date2, TrustIndication = (int)FreezeForecast.FreezingProbability.ZERO }
            };

            freezeRepo.Verify(o => o.AddFreeze(It.Is<IEnumerable<Freeze>>(e => e.Count() == 2 && e.Any(i => i.Date == date) && e.Any(i => i.Date == date2))), Times.Once);
        }
    }
}
