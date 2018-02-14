using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartFreezeScheduleFA.Repositories;
using SmartFreezeScheduleFA.Services;
using SmartFreezeScheduleFA.Models;
using Moq;
using System.Collections.Generic;
using WeatherLibrary.Algorithmes.Freeze;
using static WeatherLibrary.Algorithmes.Freeze.FreezeForecast;

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
            Mock<IFreezeRepository> freezeRepo = new Mock<IFreezeRepository>();

            //WHEN
            AlarmService service = new AlarmService(deviceRepo.Object, freezeRepo.Object);
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
            Mock<IFreezeRepository> freezeRepo = new Mock<IFreezeRepository>();

            //WHEN
            AlarmService service = new AlarmService(deviceRepo.Object, freezeRepo.Object);
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
            Mock<IFreezeRepository> freezeRepo = new Mock<IFreezeRepository>();

            //WHEN
            AlarmService service = new AlarmService(deviceRepo.Object, freezeRepo.Object);
            service.CreateCommunicationAlarm(deviceId, siteId, lastCom, gravity);

            //THEN
            deviceRepo.Verify(o => o.AddAlarm("1", It.Is<Alarm>(e =>
                e.AlarmGravity == Alarm.Gravity.Critical &&
                e.AlarmType == Alarm.Type.CommuniationFailure &&
                e.Description == "Le capteur n'a pas communiqué depuis le 01/01/2018 12:30" &&
                e.ShortDescription == "Erreur de communication")), Times.Once);

        }

        [TestMethod]
        public void TestcreateFreezeAlarm022()
        {
            //GIVEN
            string deviceId = "1";
            string siteId = "1";
            DateTime dateRef = new DateTime(2018, 02, 14, 6, 0, 0);
            Mock<IDeviceRepository> deviceRepo = new Mock<IDeviceRepository>();
            Mock<IFreezeRepository> freezeRepo = new Mock<IFreezeRepository>();
            Dictionary<DateTime, FreezingProbability> dico = new Dictionary<DateTime, FreezingProbability>();
            // 022
            dico.Add(dateRef, FreezingProbability.ZERO);
            dico.Add(dateRef.AddHours(12), FreezingProbability.MEDIUM);
            dico.Add(dateRef.AddHours(24), FreezingProbability.MEDIUM);

            //WHEN
            AlarmService alarmService = new AlarmService(deviceRepo.Object, freezeRepo.Object);
            alarmService.CreateFreezeAlarm(deviceId, siteId, dico);

            //THEN
            deviceRepo.Verify(o => o.AddAlarm("1", It.Is<Alarm>(e =>
                e.AlarmGravity == Alarm.Gravity.Critical &&
                e.AlarmType == Alarm.Type.FreezeWarning &&
                e.Description == "gel prévu du 14/02/2018 18:00:00 au 15/02/2018 06:00:00" &&
                e.ShortDescription == "gel prévu" &&
                e.Start == new DateTime(2018,02,14,18,0,0) &&
                e.End == new DateTime(2018, 02, 15, 06, 0, 0))), Times.Once);
        }

        [TestMethod]
        public void TestcreateFreezeAlarm2200123()
        {
            //GIVEN
            string deviceId = "1";
            string siteId = "1";
            DateTime dateRef = new DateTime(2018, 02, 14, 6, 0, 0);
            Mock<IDeviceRepository> deviceRepo = new Mock<IDeviceRepository>();
            Mock<IFreezeRepository> freezeRepo = new Mock<IFreezeRepository>();
            Dictionary<DateTime, FreezingProbability> dico = new Dictionary<DateTime, FreezingProbability>();
            // 2200123
            dico.Add(dateRef, FreezingProbability.MEDIUM);
            dico.Add(dateRef.AddHours(12), FreezingProbability.MEDIUM);
            dico.Add(dateRef.AddHours(24), FreezingProbability.ZERO);
            dico.Add(dateRef.AddHours(36), FreezingProbability.ZERO);
            dico.Add(dateRef.AddHours(48), FreezingProbability.MINIMUM);
            dico.Add(dateRef.AddHours(60), FreezingProbability.MEDIUM);
            dico.Add(dateRef.AddHours(72), FreezingProbability.HIGH);



            //WHEN
            AlarmService alarmService = new AlarmService(deviceRepo.Object, freezeRepo.Object);
            alarmService.CreateFreezeAlarm(deviceId, siteId, dico);

            //THEN
            deviceRepo.Verify(o => o.AddAlarm("1", It.Is<Alarm>(e =>
                e.AlarmGravity == Alarm.Gravity.Critical &&
                e.AlarmType == Alarm.Type.FreezeWarning &&
                e.Description == "gel prévu du 14/02/2018 06:00:00 au 14/02/2018 18:00:00" &&
                e.ShortDescription == "gel prévu" &&
                e.Start == new DateTime(2018, 02, 14, 06, 0, 0) &&
                e.End == new DateTime(2018, 02, 14, 18, 0, 0))), Times.Once);

            deviceRepo.Verify(o => o.AddAlarm("1", It.Is<Alarm>(e =>
                e.AlarmGravity == Alarm.Gravity.Critical &&
                e.AlarmType == Alarm.Type.FreezeWarning &&
                e.Description == "gel prévu du 16/02/2018 18:00:00 au 17/02/2018 06:00:00" &&
                e.ShortDescription == "gel prévu" &&
                e.Start == new DateTime(2018, 02, 16, 18, 0, 0) &&
                e.End == new DateTime(2018, 02, 17, 06, 0, 0))), Times.Once);

            deviceRepo.Verify(o => o.AddAlarm("1", It.Is<Alarm>(e =>
                e.AlarmGravity == Alarm.Gravity.Critical &&
                e.AlarmType == Alarm.Type.FreezeWarning &&
                e.Description == "degel prévu le 15/02/2018 06:00:00" &&
                e.ShortDescription == "degel prévu" &&
                e.Start == new DateTime(2018, 02, 15, 06, 0, 0) &&
                e.End == null)), Times.Once);
        }

        [TestMethod]
        public void TestcreateFreezeAlarm020()
        {
            //GIVEN
            string deviceId = "1";
            string siteId = "1";
            DateTime dateRef = new DateTime(2018, 02, 14, 6, 0, 0);
            Mock<IDeviceRepository> deviceRepo = new Mock<IDeviceRepository>();
            Mock<IFreezeRepository> freezeRepo = new Mock<IFreezeRepository>();
            Dictionary<DateTime, FreezingProbability> dico = new Dictionary<DateTime, FreezingProbability>();
            // 020
            dico.Add(dateRef, FreezingProbability.ZERO);
            dico.Add(dateRef.AddHours(12), FreezingProbability.MEDIUM);
            dico.Add(dateRef.AddHours(24), FreezingProbability.ZERO);

            //WHEN
            AlarmService alarmService = new AlarmService(deviceRepo.Object, freezeRepo.Object);
            alarmService.CreateFreezeAlarm(deviceId, siteId, dico);

            //THEN
            deviceRepo.Verify(o => o.AddAlarm("1", It.Is<Alarm>(e =>
                e.AlarmGravity == Alarm.Gravity.Critical &&
                e.AlarmType == Alarm.Type.FreezeWarning &&
                e.Description == "gel prévu du 14/02/2018 18:00:00 au 14/02/2018 18:00:00" &&
                e.ShortDescription == "gel prévu" &&
                e.Start == new DateTime(2018, 02, 14, 18, 0, 0) &&
                e.End == new DateTime(2018, 02, 14, 18, 0, 0))), Times.Once);

            deviceRepo.Verify(o => o.AddAlarm("1", It.Is<Alarm>(e =>
                e.AlarmGravity == Alarm.Gravity.Critical &&
                e.AlarmType == Alarm.Type.FreezeWarning &&
                e.Description == "degel prévu le 15/02/2018 06:00:00" &&
                e.ShortDescription == "degel prévu" &&
                e.Start == new DateTime(2018, 02, 15, 06, 0, 0) &&
                e.End == null)), Times.Once);
        }
    }
}
