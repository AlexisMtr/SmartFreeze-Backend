using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SmartFreezeFA.Repositories;
using SmartFreezeFA.Services;
using SmartFreezeFA.Models;

namespace SmartFreezeFA.Tests
{
    [TestClass]
    public class AlarmServiceTest
    {
        [TestMethod]
        public void TestCreateBatteryAlarm()
        {
            //GIVEN
            Telemetry telemetry = new Telemetry
            {
                Id = "1",
                DeviceId = "1",
                OccuredAt = DateTime.UtcNow,
                BatteryVoltage = 0.2,
                Pressure = 98500,
                Humidity = 40,
                Temperature = 20
            };

            Mock<IAlarmRepository> alarmRepo = new Mock<IAlarmRepository>();
            Mock<ITelemetryRepository> telemetryRepo = new Mock<ITelemetryRepository>();
            Mock<IDeviceRepository> deviceRepo = new Mock<IDeviceRepository>();

            //WHEN
            AlarmService service = new AlarmService(telemetryRepo.Object, alarmRepo.Object, deviceRepo.Object);
            service.CreateBatteryAlarm(telemetry);

            //THEN
            deviceRepo.Verify(o => o.AddAlarm("1", It.Is<Alarm>(e => 
                e.AlarmGravity == Alarm.Gravity.Critical &&
                e.AlarmType == Alarm.Type.BatteryWarning &&
                e.Description == "Batterie très faible pour le capteur (moins de 15%)" &&
                e.ShortDescription == "batterie < 15%")), Times.Once);
        }
    }
}
