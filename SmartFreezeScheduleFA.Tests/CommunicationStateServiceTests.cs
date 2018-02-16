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
        private List<Alarm> alarmsList;
        private List<Device> deviceList;

        [TestInitialize]
        public void Setup()
        {


            deviceList = new List<Device>
            {
                new Device
                {
                        Id = "1",
                        IsFavorite = true,
                        Name = "Test",
                        Alarms = new List<Alarm>(),
                        LastCommunication = DateTime.Now.AddMinutes(-(7 * 60 + 30))
                }
             };
        }

        [TestMethod]
        public void GetFailCommunicationTest()
        {
            Mock<IDeviceRepository> deviceRepo = new Mock<IDeviceRepository>();
            Mock<ITelemetryRepository> telemetryRepo = new Mock<ITelemetryRepository>();
            DeviceService service = new DeviceService(deviceRepo.Object, telemetryRepo.Object);

            deviceRepo.Setup(o => o.GetFailsCommunicationBetween(7, It.IsAny<int>()))
                .Returns(deviceList);

            IEnumerable<Device> devices = service.CheckDeviceCommunication(7, 8);

            Check.That(devices).ContainsOnlyElementsThatMatch(e => e.LastCommunication < DateTime.Now.AddMinutes(-(7 * 60)));
            //THEN
            deviceRepo.Verify(a => a.GetFailsCommunicationBetween(7, 8), Times.Once);
        }


        [TestMethod]
        public void RunAddWithUpdateStatusAlarmTest()
        {
            //initialize
            Mock<IDeviceRepository> deviceRepoMock = new Mock<IDeviceRepository>();
            Mock<ITelemetryRepository> telemetryRepoMock = new Mock<ITelemetryRepository>();
            Mock<IFreezeRepository> freezeReop = new Mock<IFreezeRepository>();

            alarmsList = new List<Alarm>
            {
                new Alarm
                {
                    Id = "1",
                    AlarmType = Alarm.Type.CommuniationFailure,
                    AlarmGravity = Alarm.Gravity.Information,
                    Description ="",
                    IsActive = true,
                    DeviceId = "1",
                    OccuredAt = DateTime.UtcNow,
                    SiteId = "1"
                },
            };
            deviceList[0].Alarms = alarmsList;


            int minMin = 7 * 60 + 5;
            deviceRepoMock.Setup(o => o.GetFailsCommunicationBetween(minMin, null)).Returns(deviceList);

            DeviceService deviceService = new DeviceService(deviceRepoMock.Object, telemetryRepoMock.Object);
            AlarmService alarmService = new AlarmService(deviceRepoMock.Object, freezeReop.Object);
            NotificationService notificationService = new NotificationService(deviceRepoMock.Object);

            CommunicationStateService communicationStateService = new CommunicationStateService(deviceService, alarmService, notificationService);

            //execute 
            communicationStateService.Run(7, null, Alarm.Gravity.Serious);

            //tests

            // TODO : check tests (error)
            deviceRepoMock.Verify(o => o.UpdateStatusAlarm("1", It.IsAny<Alarm>()), Times.Once);
            deviceRepoMock.Verify(o => o.AddAlarm("1", It.IsAny<Alarm>()), Times.Once);
        }


        [TestMethod]
        public void RunAddAlarmTest()
        {
            //initialize
            Mock<IDeviceRepository> deviceRepoMock = new Mock<IDeviceRepository>();
            Mock<ITelemetryRepository> telemetryRepoMock = new Mock<ITelemetryRepository>();
            Mock<IFreezeRepository> freezeReop = new Mock<IFreezeRepository>();


            int minMin = 1 * 60 + 5;
            int minMax = 2 * 60 + 5;
            deviceRepoMock.Setup(o => o.GetFailsCommunicationBetween(minMin, minMax)).Returns(deviceList);

            DeviceService deviceService = new DeviceService(deviceRepoMock.Object, telemetryRepoMock.Object);
            AlarmService alarmService = new AlarmService(deviceRepoMock.Object, freezeReop.Object);
            NotificationService notificationService = new NotificationService(deviceRepoMock.Object);

            CommunicationStateService communicationStateService = new CommunicationStateService(deviceService, alarmService, notificationService);

            //execute 
            communicationStateService.Run(1, 2, Alarm.Gravity.Information);

            //tests

            // TODO : check tests (error)
            deviceRepoMock.Verify(o => o.UpdateStatusAlarm("1", It.IsAny<Alarm>()), Times.Never);
            deviceRepoMock.Verify(o => o.AddAlarm("1", It.IsAny<Alarm>()), Times.Once);
        }

        [TestMethod]
        public void RunWithoutAddAlarmTest()
        {
            //initialize
            Mock<IDeviceRepository> deviceRepoMock = new Mock<IDeviceRepository>();
            Mock<ITelemetryRepository> telemetryRepoMock = new Mock<ITelemetryRepository>();
            Mock<IFreezeRepository> freezeReop = new Mock<IFreezeRepository>();

            alarmsList = new List<Alarm>
            {
                new Alarm
                {
                    Id = "1",
                    AlarmType = Alarm.Type.CommuniationFailure,
                    AlarmGravity = Alarm.Gravity.Information,
                    Description ="",
                    IsActive = true,
                    DeviceId = "1",
                    OccuredAt = DateTime.UtcNow,
                    SiteId = "1"
                },
            };
            deviceList[0].Alarms = alarmsList;


            int minMin = 1 * 60 + 5;
            int minMax = 2 * 60 + 5;
            deviceRepoMock.Setup(o => o.GetFailsCommunicationBetween(minMin, minMax)).Returns(deviceList);

            DeviceService deviceService = new DeviceService(deviceRepoMock.Object, telemetryRepoMock.Object);
            AlarmService alarmService = new AlarmService(deviceRepoMock.Object, freezeReop.Object);
            NotificationService notificationService = new NotificationService(deviceRepoMock.Object);

            CommunicationStateService communicationStateService = new CommunicationStateService(deviceService, alarmService, notificationService);

            //execute 
            communicationStateService.Run(1, 2, Alarm.Gravity.Information);

            //tests
            deviceRepoMock.Verify(o => o.UpdateStatusAlarm("1", It.IsAny<Alarm>()), Times.Never);
            deviceRepoMock.Verify(o => o.AddAlarm("1", It.IsAny<Alarm>()), Times.Never);

        }

    }
}
