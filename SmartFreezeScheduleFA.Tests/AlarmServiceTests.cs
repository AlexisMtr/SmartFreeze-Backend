using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartFreezeScheduleFA.Repositories;
using SmartFreezeScheduleFA.Services;
using SmartFreezeScheduleFA.Models;
using Moq;

namespace SmartFreezeScheduleFA.Tests
{
    [TestClass]
    public class AlarmServiceTests
    {
        [TestMethod]
        public void TestCreateCommunicationAlarm1h()
        {
            //GIVEN
            string deviceId = "1";
            string siteId = "2";
            DateTime lastCom = new DateTime(2018, 1, 20, 14, 30, 0);
            Alarm.Gravity gravity = Alarm.Gravity.Information;
            Mock<IDeviceRepository> deviceRepo = new Mock<IDeviceRepository>();
            
            //WHEN
            AlarmService service = new AlarmService(deviceRepo.Object);
            Alarm alarm = service.CreateCommunicationAlarm(deviceId, siteId, lastCom, gravity);

            //THEN
            deviceRepo.Verify(o => o.AddAlarm("1", It.Is<Alarm>(e =>
                e.AlarmGravity == Alarm.Gravity.Information &&
                e.AlarmType == Alarm.Type.CommuniationFailure &&
                e.Description == "Le capteur n'a pas communiqué depuis le 20/01/2018 14:30" &&
                e.ShortDescription == "Erreur de communication")), Times.Once);

        }

        [TestMethod]
        public void TestCreateCommunicationAlarm4h()
        {
            //GIVEN
            string deviceId = "1";
            string siteId = "2";
            DateTime lastCom = new DateTime(2018, 1, 30, 5, 50, 0);
            Alarm.Gravity gravity = Alarm.Gravity.Serious;
            Mock<IDeviceRepository> deviceRepo = new Mock<IDeviceRepository>();

            //WHEN
            AlarmService service = new AlarmService(deviceRepo.Object);
            service.CreateCommunicationAlarm(deviceId, siteId, lastCom, gravity);

            //THEN
            deviceRepo.Verify(o => o.AddAlarm("1", It.Is<Alarm>(e =>
                e.AlarmGravity == Alarm.Gravity.Serious &&
                e.AlarmType == Alarm.Type.CommuniationFailure &&
                e.Description == "Le capteur n'a pas communiqué depuis le 30/01/2018 05:50" &&
                e.ShortDescription == "Erreur de communication")), Times.Once);

        }

        [TestMethod]
        public void TestCreateCommunicationAlarm7h()
        {
            //GIVEN
            string deviceId = "1";
            string siteId = "2";
            DateTime lastCom = new DateTime(2018, 1, 1, 12, 30, 0);
            Alarm.Gravity gravity = Alarm.Gravity.Critical;
            Mock<IDeviceRepository> deviceRepo = new Mock<IDeviceRepository>();

            //WHEN
            AlarmService service = new AlarmService(deviceRepo.Object);
            service.CreateCommunicationAlarm(deviceId, siteId, lastCom, gravity);

            //THEN
            deviceRepo.Verify(o => o.AddAlarm("1", It.Is<Alarm>(e =>
                e.AlarmGravity == Alarm.Gravity.Critical &&
                e.AlarmType == Alarm.Type.CommuniationFailure &&
                e.Description == "Le capteur n'a pas communiqué depuis le 01/01/2018 12:30" &&
                e.ShortDescription == "Erreur de communication")), Times.Once);

        }
    }
}
