using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SmartFreezeFA.Repositories;
using SmartFreezeFA.Services;
using SmartFreezeFA.Models;
using System.Collections.Generic;
using WeatherLibrary.Algorithmes.Freeze;
using NFluent;

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
            Mock<IAlarmRepository> alarmRepo = new Mock<IAlarmRepository>();

            //WHEN
            AlarmService service = new AlarmService(deviceRepo.Object, alarmRepo.Object);
            service.CreateBatteryAlarm(telemetry, "site1");

            //THEN
            deviceRepo.Verify(o => o.AddAlarm("1", It.Is<Alarm>(e => 
                e.AlarmGravity == Alarm.Gravity.Critical &&
                e.AlarmType == Alarm.Type.DeviceFailure &&
                e.Description.ToLower().Contains("batterie") &&
                e.ShortDescription.ToLower().Contains("15%"))), Times.Once);
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
            Mock<IAlarmRepository> alarmRepo = new Mock<IAlarmRepository>();

            //WHEN
            AlarmService service = new AlarmService(deviceRepo.Object, alarmRepo.Object);
            service.CreateBatteryAlarm(telemetry, "site1");

            //THEN
            deviceRepo.Verify(o => o.AddAlarm("1", It.Is<Alarm>(e =>
                e.AlarmGravity == Alarm.Gravity.Serious &&
                e.AlarmType == Alarm.Type.DeviceFailure &&
                e.Description.ToLower().Contains("batterie") &&
                e.ShortDescription.ToLower().Contains("30%"))), Times.Once);
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
            Mock<IAlarmRepository> alarmRepo = new Mock<IAlarmRepository>();

            //WHEN
            AlarmService service = new AlarmService(deviceRepo.Object, alarmRepo.Object);
            service.CreateBatteryAlarm(telemetry, "site1");

            //THEN
            deviceRepo.Verify(o => o.AddAlarm("1", It.Is<Alarm>(e =>
                e.AlarmGravity == Alarm.Gravity.Information &&
                e.AlarmType == Alarm.Type.DeviceFailure &&
                e.Description.ToLower().Contains("batterie") &&
                e.ShortDescription.ToLower().Contains("50%"))), Times.Once);
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
            Mock<IAlarmRepository> alarmRepo = new Mock<IAlarmRepository>();

            //WHEN
            AlarmService service = new AlarmService(deviceRepo.Object, alarmRepo.Object);
            service.CreateHumidityAlarm(telemetry, "site1");

            //THEN
            deviceRepo.Verify(o => o.AddAlarm("2", It.Is<Alarm>(e =>
                e.AlarmGravity == Alarm.Gravity.Critical &&
                e.AlarmType == Alarm.Type.DeviceFailure &&
                e.Description.ToLower().Contains("humidité") &&
                e.ShortDescription.ToLower().Contains("humidité") &&
                e.ShortDescription.ToLower().Contains("100%"))), Times.Once);
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
            Mock<IAlarmRepository> alarmRepo = new Mock<IAlarmRepository>();

            //WHEN
            AlarmService service = new AlarmService(deviceRepo.Object, alarmRepo.Object);
            service.CreateHumidityAlarm(telemetry, "site1");

            //THEN
            deviceRepo.Verify(o => o.AddAlarm("2", It.Is<Alarm>(e =>
                e.AlarmGravity == Alarm.Gravity.Critical &&
                e.AlarmType == Alarm.Type.DeviceFailure &&
                e.Description.ToLower().Contains("humidité") &&
                e.ShortDescription.ToLower().Contains("humidité") &&
                e.ShortDescription.ToLower().Contains("0%"))), Times.Once);
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
            Mock<IAlarmRepository> alarmRepo = new Mock<IAlarmRepository>();

            //WHEN
            AlarmService service = new AlarmService(deviceRepo.Object, alarmRepo.Object);
            service.CreateTemperatureAlarm(telemetry, "site1");

            //THEN
            deviceRepo.Verify(o => o.AddAlarm("2", It.Is<Alarm>(e =>
                e.AlarmGravity == Alarm.Gravity.Critical &&
                e.AlarmType == Alarm.Type.DeviceFailure &&
                e.Description.ToLower().Contains("température") &&
                e.ShortDescription.ToLower().Contains("température") &&
                e.ShortDescription.ToLower().Contains("100%"))), Times.Once);
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
            Mock<IAlarmRepository> alarmRepo = new Mock<IAlarmRepository>();

            //WHEN
            AlarmService service = new AlarmService(deviceRepo.Object, alarmRepo.Object);
            service.CreateTemperatureAlarm(telemetry, "site1");

            //THEN
            deviceRepo.Verify(o => o.AddAlarm("2", It.Is<Alarm>(e =>
                e.AlarmGravity == Alarm.Gravity.Critical &&
                e.AlarmType == Alarm.Type.DeviceFailure &&
                e.Description.ToLower().Contains("température") &&
                e.ShortDescription.ToLower().Contains("température") &&
                e.ShortDescription.ToLower().Contains("-300"))), Times.Once);
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
            Mock<IAlarmRepository> alarmRepo = new Mock<IAlarmRepository>();

            //WHEN
            AlarmService service = new AlarmService(deviceRepo.Object, alarmRepo.Object);
            service.CreateTemperatureAlarm(telemetry, "site1");

            //THEN
            deviceRepo.Verify(o => o.AddAlarm("2", It.Is<Alarm>(e =>
                e.AlarmGravity == Alarm.Gravity.Serious &&
                e.AlarmType == Alarm.Type.DeviceFailure &&
                e.Description.ToLower().Contains("température") &&
                e.ShortDescription.ToLower().Contains("température") &&
                e.ShortDescription.ToLower().Contains("-100"))), Times.Once);
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
            Mock<IAlarmRepository> alarmRepo = new Mock<IAlarmRepository>();

            //WHEN
            AlarmService service = new AlarmService(deviceRepo.Object, alarmRepo.Object);
            service.CreateTemperatureAlarm(telemetry, "site1");

            //THEN
            deviceRepo.Verify(o => o.AddAlarm("2", It.Is<Alarm>(e =>
                e.AlarmGravity == Alarm.Gravity.Serious &&
                e.AlarmType == Alarm.Type.DeviceFailure &&
                e.Description.ToLower().Contains("température") &&
                e.ShortDescription.ToLower().Contains("température") &&
                e.ShortDescription.ToLower().Contains("80"))), Times.Once);
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
            Mock<IAlarmRepository> alarmRepo = new Mock<IAlarmRepository>();

            //WHEN
            AlarmService service = new AlarmService(deviceRepo.Object, alarmRepo.Object);
            service.CreateTemperatureAlarm(telemetry, "site1");

            //THEN
            deviceRepo.Verify(o => o.AddAlarm("2", It.Is<Alarm>(e =>
                e.AlarmGravity == Alarm.Gravity.Information &&
                e.AlarmType == Alarm.Type.DeviceFailure &&
                e.Description.ToLower().Contains("température") &&
                e.ShortDescription.ToLower().Contains("température") &&
                e.ShortDescription.ToLower().Contains("-50"))), Times.Once);
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
            Mock<IAlarmRepository> alarmRepo = new Mock<IAlarmRepository>();

            //WHEN
            AlarmService service = new AlarmService(deviceRepo.Object, alarmRepo.Object);
            service.CreateTemperatureAlarm(telemetry, "site1");

            //THEN
            deviceRepo.Verify(o => o.AddAlarm("2", It.Is<Alarm>(e =>
                e.AlarmGravity == Alarm.Gravity.Information &&
                e.AlarmType == Alarm.Type.DeviceFailure &&
                e.Description.ToLower().Contains("température") &&
                e.ShortDescription.ToLower().Contains("température") &&
                e.ShortDescription.ToLower().Contains("50"))), Times.Once);
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
            Mock<IAlarmRepository> alarmRepo = new Mock<IAlarmRepository>();

            //WHEN
            AlarmService service = new AlarmService(deviceRepo.Object, alarmRepo.Object);
            service.CreateFreezingAlarm(telemetry, "site1", dateStart, dateEnd);

            deviceRepo.Verify(o => o.AddAlarm("2", It.Is<Alarm>(e =>
               e.AlarmGravity == Alarm.Gravity.Critical &&
               e.AlarmType == Alarm.Type.FreezeWarning &&
               e.Description.ToLower().Contains("gel") &&
               e.ShortDescription.ToLower().Contains("gel"))), Times.Once);
        }

        [TestMethod]
        public void TestCalculAverageFreezePrediction12h()
        {
            //GIVEN
            Mock<IDeviceRepository> deviceRepo = new Mock<IDeviceRepository>();
            Mock<IAlarmRepository> alarmRepo = new Mock<IAlarmRepository>();

            DateTime dateRef = new DateTime(2018, 02, 13, 6, 0, 0);
            Dictionary<DateTime, FreezeForecast.FreezingProbability> previsions3h = new Dictionary<DateTime, FreezeForecast.FreezingProbability>();
            // 13/02 AM: risque ZERO
            previsions3h.Add(dateRef, FreezeForecast.FreezingProbability.ZERO);//6h
            previsions3h.Add(dateRef.AddHours(3), FreezeForecast.FreezingProbability.ZERO);//9h
            // 13/02 PM : risque IMMINENT(=4)
            previsions3h.Add(dateRef.AddHours(6), FreezeForecast.FreezingProbability.IMMINENT);//12h
            previsions3h.Add(dateRef.AddHours(9), FreezeForecast.FreezingProbability.IMMINENT);//15h
            previsions3h.Add(dateRef.AddHours(12), FreezeForecast.FreezingProbability.HIGH);//18h
            previsions3h.Add(dateRef.AddHours(15), FreezeForecast.FreezingProbability.IMMINENT);//21h

            // 14/02/2018 AM : risque MEDIUM(=2)
            previsions3h.Add(dateRef.AddHours(18), FreezeForecast.FreezingProbability.MEDIUM);//00:00
            previsions3h.Add(dateRef.AddHours(21), FreezeForecast.FreezingProbability.HIGH);//3h
            previsions3h.Add(dateRef.AddHours(24), FreezeForecast.FreezingProbability.MEDIUM);//6h
            previsions3h.Add(dateRef.AddHours(27), FreezeForecast.FreezingProbability.MEDIUM);//9h

            // 14/02/2018 PM : risque HIGH(=3) 
            previsions3h.Add(dateRef.AddHours(30), FreezeForecast.FreezingProbability.HIGH);//12h
            previsions3h.Add(dateRef.AddHours(33), FreezeForecast.FreezingProbability.HIGH);//15h
            previsions3h.Add(dateRef.AddHours(36), FreezeForecast.FreezingProbability.MEDIUM);//18h
            previsions3h.Add(dateRef.AddHours(39), FreezeForecast.FreezingProbability.HIGH);//21h

            //WHEN
            AlarmService service = new AlarmService(deviceRepo.Object, alarmRepo.Object);
            Dictionary<DateTime, FreezeForecast.FreezingProbability> prevsions12h = service.CalculAverageFreezePrediction12h(previsions3h);

            //THEN
            Check.That(prevsions12h).Contains(new Dictionary<DateTime, FreezeForecast.FreezingProbability>
            {
                { new DateTime(2018, 02, 13, 0, 0, 0),FreezeForecast.FreezingProbability.ZERO},
                { new DateTime(2018, 02, 13, 12, 0, 0), FreezeForecast.FreezingProbability.IMMINENT},
                { new DateTime(2018, 02, 14, 00, 0, 0), FreezeForecast.FreezingProbability.MEDIUM},
                { new DateTime(2018, 02, 14, 12, 0, 0), FreezeForecast.FreezingProbability.HIGH}
            });
        }


        [TestMethod]
        public void TestCalculAverageFreezePrediction12hV2()
        {
            //GIVEN
            Mock<IDeviceRepository> deviceRepo = new Mock<IDeviceRepository>();
            Mock<IAlarmRepository> alarmRepo = new Mock<IAlarmRepository>();

            DateTime dateRef = new DateTime(2018, 02, 13, 6, 0, 0);
            Dictionary<DateTime, FreezeForecast.FreezingProbability> previsions3h = new Dictionary<DateTime, FreezeForecast.FreezingProbability>();
            // 13/02 AM: risque ZERO
            previsions3h.Add(dateRef, FreezeForecast.FreezingProbability.ZERO);//6h
            previsions3h.Add(dateRef.AddHours(3), FreezeForecast.FreezingProbability.ZERO);//9h
            // 13/02 PM : risque MEDIUM(=2)
            previsions3h.Add(dateRef.AddHours(6), FreezeForecast.FreezingProbability.MINIMUM);//12h
            previsions3h.Add(dateRef.AddHours(9), FreezeForecast.FreezingProbability.MEDIUM);//15h
            previsions3h.Add(dateRef.AddHours(12), FreezeForecast.FreezingProbability.HIGH);//18h
            previsions3h.Add(dateRef.AddHours(15), FreezeForecast.FreezingProbability.IMMINENT);//21h

            // 14/02/2018 AM : risque HIGH(=3)
            previsions3h.Add(dateRef.AddHours(18), FreezeForecast.FreezingProbability.MINIMUM);//00:00
            previsions3h.Add(dateRef.AddHours(21), FreezeForecast.FreezingProbability.MEDIUM);//3h
            previsions3h.Add(dateRef.AddHours(24), FreezeForecast.FreezingProbability.IMMINENT);//6h
            previsions3h.Add(dateRef.AddHours(27), FreezeForecast.FreezingProbability.IMMINENT);//9h

            // 14/02/2018 PM : risque HIGH(=3) 
            previsions3h.Add(dateRef.AddHours(30), FreezeForecast.FreezingProbability.HIGH);//12h
            previsions3h.Add(dateRef.AddHours(33), FreezeForecast.FreezingProbability.IMMINENT);//15h
            previsions3h.Add(dateRef.AddHours(36), FreezeForecast.FreezingProbability.MEDIUM);//18h
            previsions3h.Add(dateRef.AddHours(39), FreezeForecast.FreezingProbability.HIGH);//21h

            // 15/02/2018 AM
            previsions3h.Add(dateRef.AddHours(42), FreezeForecast.FreezingProbability.ZERO);//00:00h

            //WHEN
            AlarmService service = new AlarmService(deviceRepo.Object, alarmRepo.Object);
            Dictionary<DateTime, FreezeForecast.FreezingProbability> prevsions12h = service.CalculAverageFreezePrediction12h(previsions3h);

            //THEN
            Check.That(prevsions12h).Contains(new Dictionary<DateTime, FreezeForecast.FreezingProbability>
            {
                { new DateTime(2018, 02, 13, 0, 0, 0),FreezeForecast.FreezingProbability.ZERO},
                { new DateTime(2018, 02, 13, 12, 0, 0), FreezeForecast.FreezingProbability.MEDIUM},
                { new DateTime(2018, 02, 14, 00, 0, 0), FreezeForecast.FreezingProbability.HIGH},
                { new DateTime(2018, 02, 14, 12, 0, 0), FreezeForecast.FreezingProbability.HIGH},
                { new DateTime(2018, 02, 15, 00, 0, 0), FreezeForecast.FreezingProbability.ZERO}
            });
        }

    }
}
