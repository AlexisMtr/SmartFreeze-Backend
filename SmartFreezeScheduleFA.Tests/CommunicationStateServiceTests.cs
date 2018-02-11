using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NFluent;
using SmartFreezeScheduleFA.Models;
using SmartFreezeScheduleFA.Repositories;
using SmartFreezeScheduleFA.Services;

namespace SmartFreezeScheduleFA.Tests
{
    [TestClass]
    public class CommunicationStateServiceTests
    {
        [TestMethod]
        public void GetFailCommunicationTest()
        {
            Mock<IDeviceRepository> deviceRepo = new Mock<IDeviceRepository>();
            Mock<ITelemetryRepository> telemetryRepo = new Mock<ITelemetryRepository>();
            DeviceService service = new DeviceService(deviceRepo.Object, telemetryRepo.Object);

            deviceRepo.Setup(o => o.GetFailsCommunicationBetween(7, It.IsAny<int>()))
                .Returns(new List<Device>
                {
                    new Device
                    {
                        Id = "1",
                        IsFavorite = true,
                        Name = "Test",
                        Alarms = new List<Alarm>(),
                        LastCommunication = DateTime.Now.AddMinutes(-(7 * 60 + 30))
                    }
                });

            IEnumerable<Device> devices = service.CheckDeviceCommunication(7, 8);

            Check.That(devices).ContainsOnlyElementsThatMatch(e => e.LastCommunication < DateTime.Now.AddMinutes(-(7 * 60)));
            //THEN
            deviceRepo.Verify(a => a.GetFailsCommunicationBetween(7, 8), Times.Once);
        }

    }
}
