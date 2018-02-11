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
            Alarm.Gravity gravity = Alarm.Gravity.Information;
            Mock<IDeviceRepository> deviceRepo = new Mock<IDeviceRepository>();
            
            //WHEN
            AlarmService service = new AlarmService(deviceRepo.Object);
            service.CreateCommunicationAlarm(deviceId, gravity);

            //THEN
            deviceRepo.Verify(o => o.AddAlarm("1", It.Is<Alarm>(e =>
                e.AlarmGravity == Alarm.Gravity.Information &&
                e.AlarmType == Alarm.Type.CommuniationFailure &&
                e.Description == "Le capteur n'a pas communiqué depuis plus d'une heure" &&
                e.ShortDescription == "Erreur de communication")), Times.Once);

        }

        [TestMethod]
        public void TestCreateCommunicationAlarm4h()
        {
            //GIVEN
            string deviceId = "1";
            Alarm.Gravity gravity = Alarm.Gravity.Serious;
            Mock<IDeviceRepository> deviceRepo = new Mock<IDeviceRepository>();

            //WHEN
            AlarmService service = new AlarmService(deviceRepo.Object);
            service.CreateCommunicationAlarm(deviceId, gravity);

            //THEN
            deviceRepo.Verify(o => o.AddAlarm("1", It.Is<Alarm>(e =>
                e.AlarmGravity == Alarm.Gravity.Serious &&
                e.AlarmType == Alarm.Type.CommuniationFailure &&
                e.Description == "Le capteur n'a pas communiqué depuis plus de 4 heures" &&
                e.ShortDescription == "Erreur de communication")), Times.Once);

        }

        [TestMethod]
        public void TestCreateCommunicationAlarm7h()
        {
            //GIVEN
            string deviceId = "1";
            Alarm.Gravity gravity = Alarm.Gravity.Critical;
            Mock<IDeviceRepository> deviceRepo = new Mock<IDeviceRepository>();

            //WHEN
            AlarmService service = new AlarmService(deviceRepo.Object);
            service.CreateCommunicationAlarm(deviceId, gravity);

            //THEN
            deviceRepo.Verify(o => o.AddAlarm("1", It.Is<Alarm>(e =>
                e.AlarmGravity == Alarm.Gravity.Critical &&
                e.AlarmType == Alarm.Type.CommuniationFailure &&
                e.Description == "Le capteur n'a pas communiqué depuis plus de 7 heures" &&
                e.ShortDescription == "Erreur de communication")), Times.Once);

        }
    }
}
