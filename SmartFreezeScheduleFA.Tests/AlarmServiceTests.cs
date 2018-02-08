using Moq;
using SmartFreezeScheduleFA.Repositories;
using SmartFreezeScheduleFA.Services;
using SmartFreezeScheduleFA.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NFluent;
using System.Collections.Generic;
using System.Linq;

namespace SmartFreezeScheduleFA.Tests
{
    [TestClass]
    public class AlarmServiceTests
    {
        [TestMethod]
        public void SetGravityCommunicationInformationTest()
        {
            Mock<IDeviceRepository> deviceRepo = new Mock<IDeviceRepository>();
            AlarmService service = new AlarmService(deviceRepo.Object);

            Alarm alarm = new Alarm
            {
                AlarmGravity = Alarm.Gravity.Information,
             };
            
            var tuple = service.SetGravityCommunicationDescription(alarm.AlarmGravity);

            Check.That(tuple.desc).IsEqualTo("Pas de reception de mesures depuis plus d'une heure (entre 1 et 2 heures)");
            Check.That(tuple.shortDesc).IsEqualTo("echec communication 1h");
        }

        [TestMethod]
        public void SetGravityCommunicationSeriousTest()
        {
            Mock<IDeviceRepository> deviceRepo = new Mock<IDeviceRepository>();
            AlarmService service = new AlarmService(deviceRepo.Object);

            Alarm alarm = new Alarm
            {
                AlarmGravity = Alarm.Gravity.Serious,
            };

            var tuple = service.SetGravityCommunicationDescription(alarm.AlarmGravity);

            Check.That(tuple.desc).IsEqualTo("Pas de reception de mesures depuis plus de 4 heures (entre 4 et 5 heures)");
            Check.That(tuple.shortDesc).IsEqualTo("echec communication 4h");
        }

        [TestMethod]
        public void SetGravityCommunicationCriticalTest()
        {
            Mock<IDeviceRepository> deviceRepo = new Mock<IDeviceRepository>();
            AlarmService service = new AlarmService(deviceRepo.Object);

            Alarm alarm = new Alarm
            {
                AlarmGravity = Alarm.Gravity.Critical,
            };

            var tuple = service.SetGravityCommunicationDescription(alarm.AlarmGravity);

            Check.That(tuple.desc).IsEqualTo("Pas de reception de mesures depuis plus de 7 heures (entre 7 et 8 heures)");
            Check.That(tuple.shortDesc).IsEqualTo("echec communication 7h");
        }

        [TestMethod]
        public void CreateAlarmsTest()
        {

            Mock<IDeviceRepository> deviceRepo = new Mock<IDeviceRepository>();
            AlarmService service = new AlarmService(deviceRepo.Object);

            IEnumerable<Device> devicesList = Enumerable.Repeat(new Device(), 3);
            Alarm alarm = new Alarm
            {
                AlarmGravity = Alarm.Gravity.Critical,
                AlarmType = Alarm.Type.BatteryWarning
            };

            service.CreateAlarms(devicesList, Alarm.Gravity.Critical, Alarm.Type.BatteryWarning);

            deviceRepo.Verify(o => o.AddAlarm(It.IsAny<string>(), It.IsAny<Alarm>()), Times.Exactly(3));
        }
    }
}
