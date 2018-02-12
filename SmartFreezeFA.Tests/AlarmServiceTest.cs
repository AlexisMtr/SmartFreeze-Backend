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
        public void TestCreateBatteryAlarmVeryLow()
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
            
            Mock<IDeviceRepository> deviceRepo = new Mock<IDeviceRepository>();

            //WHEN
            AlarmService service = new AlarmService(deviceRepo.Object);
            service.CreateBatteryAlarm(telemetry);

            //THEN
            deviceRepo.Verify(o => o.AddAlarm("1", It.Is<Alarm>(e => 
                e.AlarmGravity == Alarm.Gravity.Critical &&
                e.AlarmType == Alarm.Type.BatteryWarning &&
                e.Description == "Batterie très faible pour le capteur (moins de 15%)" &&
                e.ShortDescription == "batterie < 15%")), Times.Once);
        }

        [TestMethod]
        public void TestCreateBatteryAlarmMediumLow()
        {
            //GIVEN
            Telemetry telemetry = new Telemetry
            {
                Id = "1",
                DeviceId = "1",
                OccuredAt = DateTime.UtcNow,
                BatteryVoltage = 1.0,
                Pressure = 98500,
                Humidity = 40,
                Temperature = 20
            };

            Mock<IDeviceRepository> deviceRepo = new Mock<IDeviceRepository>();

            //WHEN
            AlarmService service = new AlarmService(deviceRepo.Object);
            service.CreateBatteryAlarm(telemetry);

            //THEN
            deviceRepo.Verify(o => o.AddAlarm("1", It.Is<Alarm>(e =>
                e.AlarmGravity == Alarm.Gravity.Serious &&
                e.AlarmType == Alarm.Type.BatteryWarning &&
                e.Description == "Batterie faible pour le capteur (moins de 30%)" &&
                e.ShortDescription == "batterie < 30%")), Times.Once);
        }

        [TestMethod]
        public void TestCreateBatteryAlarmLow()
        {
            //GIVEN
            Telemetry telemetry = new Telemetry
            {
                Id = "1",
                DeviceId = "1",
                OccuredAt = DateTime.UtcNow,
                BatteryVoltage = 1.6,
                Pressure = 98500,
                Humidity = 40,
                Temperature = 20
            };

            Mock<IDeviceRepository> deviceRepo = new Mock<IDeviceRepository>();

            //WHEN
            AlarmService service = new AlarmService(deviceRepo.Object);
            service.CreateBatteryAlarm(telemetry);

            //THEN
            deviceRepo.Verify(o => o.AddAlarm("1", It.Is<Alarm>(e =>
                e.AlarmGravity == Alarm.Gravity.Information &&
                e.AlarmType == Alarm.Type.BatteryWarning &&
                e.Description == "Batterie à 50% pour le capteur" &&
                e.ShortDescription == "batterie < 50%")), Times.Once);
        }

        [TestMethod]
        public void CreateHumidityAlarmPlus100()
        {
            //GIVEN
            Telemetry telemetry = new Telemetry
            {
                Id = "2",
                DeviceId = "2",
                OccuredAt = DateTime.UtcNow,
                BatteryVoltage = 0.9,
                Pressure = 99800,
                Humidity = 180,
                Temperature = 15
            };

            Mock<IDeviceRepository> deviceRepo = new Mock<IDeviceRepository>();

            //WHEN
            AlarmService serviceHumidity = new AlarmService(deviceRepo.Object);
            serviceHumidity.CreateHumidityAlarm(telemetry);

            //THEN
            deviceRepo.Verify(o => o.AddAlarm("2", It.Is<Alarm>(e =>
                e.AlarmGravity == Alarm.Gravity.Critical &&
                e.AlarmType == Alarm.Type.DeviceFailure &&
                e.Description == "L'humidité est anormalement élevée" &&
                e.ShortDescription == "humidité > 100")), Times.Once);
        }

        [TestMethod]
        public void CreateHumidityAlarmMoins0()
        {
            //GIVEN
            Telemetry telemetry = new Telemetry
            {
                Id = "2",
                DeviceId = "2",
                OccuredAt = DateTime.UtcNow,
                BatteryVoltage = 0.9,
                Pressure = 99800,
                Humidity = 0,
                Temperature = 15
            };
            
            Mock<IDeviceRepository> deviceRepo = new Mock<IDeviceRepository>();

            //WHEN
            AlarmService serviceHumidity = new AlarmService(deviceRepo.Object);
            serviceHumidity.CreateHumidityAlarm(telemetry);

            //THEN
            deviceRepo.Verify(o => o.AddAlarm("2", It.Is<Alarm>(e =>
                e.AlarmGravity == Alarm.Gravity.Critical &&
                e.AlarmType == Alarm.Type.DeviceFailure &&
                e.Description == "L'humidité est anormalement basse" &&
                e.ShortDescription == "humidité <=0")), Times.Once);
        }

        [TestMethod]
        public void CreateTemperatureAlarmPlus100()
        {
            //GIVEN
            Telemetry telemetry = new Telemetry
            {
                Id = "2",
                DeviceId = "2",
                OccuredAt = DateTime.UtcNow,
                BatteryVoltage = 0.9,
                Pressure = 99800,
                Humidity = 42,
                Temperature = 115
            };
            
            Mock<IDeviceRepository> deviceRepo = new Mock<IDeviceRepository>();

            //WHEN
            AlarmService serviceTempérature = new AlarmService(deviceRepo.Object);
            serviceTempérature.CreateTemperatureAlarm(telemetry);

            //THEN
            deviceRepo.Verify(o => o.AddAlarm("2", It.Is<Alarm>(e =>
                e.AlarmGravity == Alarm.Gravity.Critical &&
                e.AlarmType == Alarm.Type.DeviceFailure &&
                e.Description == "La température  est anormalement hausse" &&
                e.ShortDescription == "température > 100")), Times.Once);
        }

        [TestMethod]
        public void CreateTemperatureAlarmmoins300()
        {
            //GIVEN
            Telemetry telemetry = new Telemetry
            {
                Id = "2",
                DeviceId = "2",
                OccuredAt = DateTime.UtcNow,
                BatteryVoltage = 0.9,
                Pressure = 99800,
                Humidity = 42,
                Temperature = -400
            };
            
            Mock<IDeviceRepository> deviceRepo = new Mock<IDeviceRepository>();

            //WHEN
            AlarmService serviceTempérature = new AlarmService(deviceRepo.Object);
            serviceTempérature.CreateTemperatureAlarm(telemetry);

            //THEN
            deviceRepo.Verify(o => o.AddAlarm("2", It.Is<Alarm>(e =>
                e.AlarmGravity == Alarm.Gravity.Critical &&
                e.AlarmType == Alarm.Type.DeviceFailure &&
                e.Description == "La température  est anormalement basse" &&
                e.ShortDescription == "température <-300")), Times.Once);
        }


        [TestMethod]
        public void CreateTemperatureAlarmmoins300100()
        {
            //GIVEN
            Telemetry telemetry = new Telemetry
            {
                Id = "2",
                DeviceId = "2",
                OccuredAt = DateTime.UtcNow,
                BatteryVoltage = 0.9,
                Pressure = 99800,
                Humidity = 42,
                Temperature = -200
            };
            
            Mock<IDeviceRepository> deviceRepo = new Mock<IDeviceRepository>();

            //WHEN
            AlarmService serviceTempérature = new AlarmService(deviceRepo.Object);
            serviceTempérature.CreateTemperatureAlarm(telemetry);

            //THEN
            deviceRepo.Verify(o => o.AddAlarm("2", It.Is<Alarm>(e =>
                e.AlarmGravity == Alarm.Gravity.Serious &&
                e.AlarmType == Alarm.Type.DeviceFailure &&
                e.Description == "La température  est critique" &&
                e.ShortDescription == "température entre -300 et -100")), Times.Once);
        }

        [TestMethod]
        public void CreateTemperatureAlarmmoins80100()
        {
            //GIVEN
            Telemetry telemetry = new Telemetry
            {
                Id = "2",
                DeviceId = "2",
                OccuredAt = DateTime.UtcNow,
                BatteryVoltage = 0.9,
                Pressure = 99800,
                Humidity = 42,
                Temperature = 95
            };
            
            Mock<IDeviceRepository> deviceRepo = new Mock<IDeviceRepository>();

            //WHEN
            AlarmService serviceTempérature = new AlarmService(deviceRepo.Object);
            serviceTempérature.CreateTemperatureAlarm(telemetry);

            //THEN
            deviceRepo.Verify(o => o.AddAlarm("2", It.Is<Alarm>(e =>
                e.AlarmGravity == Alarm.Gravity.Serious &&
                e.AlarmType == Alarm.Type.DeviceFailure &&
                e.Description == "La température  est critique" &&
                e.ShortDescription == "température entre 80 et 100")), Times.Once);
        }

        [TestMethod]
        public void CreateTemperatureAlarmmoins9950()
        {
            //GIVEN
            Telemetry telemetry = new Telemetry
            {
                Id = "2",
                DeviceId = "2",
                OccuredAt = DateTime.UtcNow,
                BatteryVoltage = 0.9,
                Pressure = 99800,
                Humidity = 42,
                Temperature = -88
            };
            
            Mock<IDeviceRepository> deviceRepo = new Mock<IDeviceRepository>();

            //WHEN
            AlarmService serviceTempérature = new AlarmService(deviceRepo.Object);
            serviceTempérature.CreateTemperatureAlarm(telemetry);

            //THEN
            deviceRepo.Verify(o => o.AddAlarm("2", It.Is<Alarm>(e =>
                e.AlarmGravity == Alarm.Gravity.Information &&
                e.AlarmType == Alarm.Type.DeviceFailure &&
                e.Description == "La température  est anormale" &&
                e.ShortDescription == "température entre -99 et -50")), Times.Once);
        }

        [TestMethod]
        public void CreateTemperatureAlarm5079()
        {
            //GIVEN
            Telemetry telemetry = new Telemetry
            {
                Id = "2",
                DeviceId = "2",
                OccuredAt = DateTime.UtcNow,
                BatteryVoltage = 0.9,
                Pressure = 99800,
                Humidity = 42,
                Temperature = 60
            };
            
            Mock<IDeviceRepository> deviceRepo = new Mock<IDeviceRepository>();

            //WHEN
            AlarmService serviceTempérature = new AlarmService(deviceRepo.Object);
            serviceTempérature.CreateTemperatureAlarm(telemetry);

            //THEN
            deviceRepo.Verify(o => o.AddAlarm("2", It.Is<Alarm>(e =>
                e.AlarmGravity == Alarm.Gravity.Information &&
                e.AlarmType == Alarm.Type.DeviceFailure &&
                e.Description == "La température  est anormale" &&
                e.ShortDescription == "température entre 50 et 79")), Times.Once);
        }

        [TestMethod]
        public void CreateFreezingAlarmTest()
        {

            //GIVEN
            Telemetry telemetry = new Telemetry
            {
                Id = "2",
                DeviceId = "2",
                OccuredAt = DateTime.UtcNow,
                BatteryVoltage = 0.9,
                Pressure = 99800,
                Humidity = 42,
                Temperature = 0
            };

            DateTime dateStart = DateTime.UtcNow;
            DateTime dateEnd = new DateTime();

            Mock<IDeviceRepository> deviceRepo = new Mock<IDeviceRepository>();

            //WHEN
            AlarmService serviceGel = new AlarmService(deviceRepo.Object);
            serviceGel.CreateFreezingAlarm(telemetry, dateStart, dateEnd);

            deviceRepo.Verify(o => o.AddAlarm("2", It.Is<Alarm>(e =>
               e.AlarmGravity == Alarm.Gravity.Critical &&
               e.AlarmType == Alarm.Type.FreezeWarning &&
               e.Description == "Le capteur detecte du gel" &&
               e.ShortDescription == "Gel!")), Times.Once);
        }
    }
}
