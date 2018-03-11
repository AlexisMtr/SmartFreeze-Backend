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
        public void TestcreateFreezeAlarm022_sansFreeze()
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
                e.Description.Contains("14/02/2018") &&
                e.Description.Contains("15/02/2018") &&
                e.Description.Contains("gel") &&
                e.Description.Contains("1") &&
                e.ShortDescription.Contains("Gel") &&
                e.ShortDescription.Contains("14/02/2018") &&
                e.ShortDescription.Contains("15/02/2018") &&
                e.Start == new DateTime(2018,02,14,18,0,0) &&
                e.End == new DateTime(2018, 02, 15, 06, 0, 0))), Times.Once);
        }

        [TestMethod]
        public void TestcreateFreezeAlarm2200123_sansFreeze()
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
                e.Description.Contains("14/02/2018") &&
                e.Description.Contains("gel") &&
                e.Description.Contains("1") &&
                e.ShortDescription.Contains("Gel") &&
                e.ShortDescription.Contains("14/02/2018") &&
                e.Start == new DateTime(2018, 02, 14, 06, 0, 0) &&
                e.End == new DateTime(2018, 02, 14, 18, 0, 0))), Times.Once);

            deviceRepo.Verify(o => o.AddAlarm("1", It.Is<Alarm>(e =>
                e.AlarmGravity == Alarm.Gravity.Critical &&
                e.AlarmType == Alarm.Type.FreezeWarning &&
                e.Description.Contains("16/02/2018") &&
                e.Description.Contains("17/02/2018") &&
                e.Description.Contains("gel") &&
                e.Description.Contains("1") &&
                e.ShortDescription.Contains("Gel") &&
                e.ShortDescription.Contains("16/02/2018") &&
                e.ShortDescription.Contains("17/02/2018") &&
                e.Start == new DateTime(2018, 02, 16, 18, 0, 0) &&
                e.End == new DateTime(2018, 02, 17, 06, 0, 0))), Times.Once);

            deviceRepo.Verify(o => o.AddAlarm("1", It.Is<Alarm>(e =>
                e.AlarmGravity == Alarm.Gravity.Critical &&
                e.AlarmType == Alarm.Type.FreezeWarning &&
                e.Description.Contains("15/02/2018") &&
                e.Description.Contains("dégel") &&
                e.Description.Contains("1") &&
                e.ShortDescription.Contains("Dégel") &&
                e.ShortDescription.Contains("15/02/2018") &&
                e.Start == new DateTime(2018, 02, 15, 06, 0, 0) &&
                e.End == null)), Times.Once);
        }

        [TestMethod]
        public void TestcreateFreezeAlarm020_sansFreeze()
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
                e.Description.Contains("14/02/2018") &&
                e.Description.Contains("gel") &&
                e.Description.Contains("1") &&
                e.ShortDescription.Contains("Gel") &&
                e.ShortDescription.Contains("14/02/2018") &&
                e.Start == new DateTime(2018, 02, 14, 18, 0, 0) &&
                e.End == new DateTime(2018, 02, 14, 18, 0, 0))), Times.Once);

            deviceRepo.Verify(o => o.AddAlarm("1", It.Is<Alarm>(e =>
                e.AlarmGravity == Alarm.Gravity.Critical &&
                e.AlarmType == Alarm.Type.FreezeWarning &&
                e.Description.Contains("15/02/2018") &&
                e.Description.Contains("dégel") &&
                e.Description.Contains("1") &&
                e.ShortDescription.Contains("Dégel") &&
                e.ShortDescription.Contains("15/02/2018") &&
                e.Start == new DateTime(2018, 02, 15, 06, 0, 0) &&
                e.End == null)), Times.Once);
        }

        [TestMethod]
        public void TestcreateFreezeAlarm22_sansFreeze()
        {
            //GIVEN
            string deviceId = "1";
            string siteId = "1";
            DateTime dateRef = new DateTime(2018, 02, 14, 6, 0, 0);
            Mock<IDeviceRepository> deviceRepo = new Mock<IDeviceRepository>();
            Mock<IFreezeRepository> freezeRepo = new Mock<IFreezeRepository>();
            Dictionary<DateTime, FreezingProbability> dico = new Dictionary<DateTime, FreezingProbability>();
            // 22
            dico.Add(dateRef, FreezingProbability.MEDIUM);
            dico.Add(dateRef.AddHours(12), FreezingProbability.MEDIUM);

            //WHEN
            AlarmService alarmService = new AlarmService(deviceRepo.Object, freezeRepo.Object);
            alarmService.CreateFreezeAlarm(deviceId, siteId, dico);

            //THEN
            deviceRepo.Verify(o => o.AddAlarm("1", It.Is<Alarm>(e =>
                e.AlarmGravity == Alarm.Gravity.Critical &&
                e.AlarmType == Alarm.Type.FreezeWarning &&
                e.Description.Contains("14/02/2018") &&
                e.Description.Contains("gel") &&
                e.Description.Contains("1") &&
                e.ShortDescription.Contains("Gel") &&
                e.ShortDescription.Contains("14/02/2018") &&
                e.Start == new DateTime(2018, 02, 14, 06, 0, 0) &&
                e.End == new DateTime(2018, 02, 14, 18, 0, 0))), Times.Once);
        }

        //GEL -> DEGEL avec lastFreeze
        [TestMethod]
        public void TestcreateFreezeAlarm00_gel_degel_avecFreeze()
        {
            //GIVEN
            string deviceId = "1";
            string siteId = "1";
            DateTime dateRef = new DateTime(2018, 02, 14, 6, 0, 0);
            Mock<IDeviceRepository> deviceRepo = new Mock<IDeviceRepository>();
            Mock<IFreezeRepository> freezeRepo = new Mock<IFreezeRepository>();
            freezeRepo.Setup(o => o.GetLastFreezeByDevice("1")).Returns(new Freeze
            {
                Date = new DateTime(2018, 02, 14, 0, 0, 0),
                DeviceId = "1",
                TrustIndication = 4
            });
            Dictionary<DateTime, FreezingProbability> dico = new Dictionary<DateTime, FreezingProbability>();
            // 00
            dico.Add(dateRef, FreezingProbability.ZERO);
            dico.Add(dateRef.AddHours(12), FreezingProbability.ZERO);

            //WHEN
            AlarmService alarmService = new AlarmService(deviceRepo.Object, freezeRepo.Object);
            alarmService.CreateFreezeAlarm(deviceId, siteId, dico);

            //THEN
            deviceRepo.Verify(o => o.AddAlarm("1", It.Is<Alarm>(e =>
                e.AlarmGravity == Alarm.Gravity.Critical &&
                e.AlarmType == Alarm.Type.FreezeWarning &&
                e.Description.Contains("14/02/2018") &&
                e.Description.Contains("dégel") &&
                e.Description.Contains("1") &&
                e.ShortDescription.Contains("Dégel") &&
                e.ShortDescription.Contains("14/02/2018") &&
                e.Start == new DateTime(2018, 02, 14, 06, 0, 0) &&
                e.End == null)), Times.Once);
        }

        //DEGEL -> GEL avec lastFreeze (44)
        [TestMethod]
        public void TestcreateFreezeAlarm44_degel_gel_avecFreeze()
        {
            //GIVEN
            string deviceId = "1";
            string siteId = "1";
            DateTime dateRef = new DateTime(2018, 02, 14, 6, 0, 0);
            Mock<IDeviceRepository> deviceRepo = new Mock<IDeviceRepository>();
            Mock<IFreezeRepository> freezeRepo = new Mock<IFreezeRepository>();
            freezeRepo.Setup(o => o.GetLastFreezeByDevice("1")).Returns(new Freeze
            {
                Date = new DateTime(2018, 02, 14, 0, 0, 0),
                DeviceId = "1",
                TrustIndication = 0
            });
            Dictionary<DateTime, FreezingProbability> dico = new Dictionary<DateTime, FreezingProbability>();
            // 44
            dico.Add(dateRef, FreezingProbability.IMMINENT);
            dico.Add(dateRef.AddHours(12), FreezingProbability.IMMINENT);

            //WHEN
            AlarmService alarmService = new AlarmService(deviceRepo.Object, freezeRepo.Object);
            alarmService.CreateFreezeAlarm(deviceId, siteId, dico);

            //THEN
            deviceRepo.Verify(o => o.AddAlarm("1", It.Is<Alarm>(e =>
                e.AlarmGravity == Alarm.Gravity.Critical &&
                e.AlarmType == Alarm.Type.FreezeWarning &&
                e.Description.Contains("14/02/2018") &&
                e.Description.Contains("gel") &&
                e.Description.Contains("1") &&
                e.ShortDescription.Contains("Gel") &&
                e.ShortDescription.Contains("14/02/2018") &&
                e.Start == new DateTime(2018, 02, 14, 06, 0, 0) &&
                e.End == new DateTime(2018, 02, 14, 18, 0, 0))), Times.Once);
        }

        //DEGEL -> GEL avec lastFreeze (40)
        [TestMethod]
        public void TestcreateFreezeAlarm40_degel_gel_avecFreeze()
        {
            //GIVEN
            string deviceId = "1";
            string siteId = "1";
            DateTime dateRef = new DateTime(2018, 02, 14, 6, 0, 0);
            Mock<IDeviceRepository> deviceRepo = new Mock<IDeviceRepository>();
            Mock<IFreezeRepository> freezeRepo = new Mock<IFreezeRepository>();
            freezeRepo.Setup(o => o.GetLastFreezeByDevice("1")).Returns(new Freeze
            {
                Date = new DateTime(2018, 02, 14, 0, 0, 0),
                DeviceId = "1",
                TrustIndication = 0
            });
            Dictionary<DateTime, FreezingProbability> dico = new Dictionary<DateTime, FreezingProbability>();
            // 40
            dico.Add(dateRef, FreezingProbability.IMMINENT);
            dico.Add(dateRef.AddHours(12), FreezingProbability.ZERO);

            //WHEN
            AlarmService alarmService = new AlarmService(deviceRepo.Object, freezeRepo.Object);
            alarmService.CreateFreezeAlarm(deviceId, siteId, dico);

            //THEN
            deviceRepo.Verify(o => o.AddAlarm("1", It.Is<Alarm>(e =>
                e.AlarmGravity == Alarm.Gravity.Critical &&
                e.AlarmType == Alarm.Type.FreezeWarning &&
                e.Description.Contains("14/02/2018") &&
                e.Description.Contains("gel") &&
                e.Description.Contains("1") &&
                e.ShortDescription.Contains("Gel") &&
                e.ShortDescription.Contains("14/02/2018") &&
                e.Start == new DateTime(2018, 02, 14, 06, 0, 0) &&
                e.End == new DateTime(2018, 02, 14, 18, 0, 0))), Times.Once);
        }


        //GEL -> GEL avec lastFreeze (44)
        [TestMethod]
        public void TestcreateFreezeAlarm44_gel_gel_avecFreeze()
        {
            //GIVEN
            string deviceId = "1";
            string siteId = "1";
            DateTime dateRef = new DateTime(2018, 02, 14, 6, 0, 0);
            Mock<IDeviceRepository> deviceRepo = new Mock<IDeviceRepository>();
            Mock<IFreezeRepository> freezeRepo = new Mock<IFreezeRepository>();
            freezeRepo.Setup(o => o.GetLastFreezeByDevice("1")).Returns(new Freeze
            {
                Date = new DateTime(2018, 02, 14, 0, 0, 0),
                DeviceId = "1",
                TrustIndication = 4
            });
            deviceRepo.Setup(o => o.GetCrossAlarmsByDevice("1", new DateTime(2018, 02, 14, 6, 0, 0), new DateTime(2018, 02, 14, 18, 0, 0))).Returns(new List<Alarm>()
            {
                new Alarm()
                {
                    Id = $"{"1"}-alarm{DateTime.UtcNow.ToString("yyyyMMddHHmmss")}",
                    DeviceId = "1",
                    SiteId = "1",
                    IsActive = true,
                    AlarmType = Alarm.Type.FreezeWarning,
                    AlarmGravity = Alarm.Gravity.Critical,
                    OccuredAt = DateTime.UtcNow,
                    ShortDescription = "Gel",
                    Description = "Gel",
                    Start = new DateTime(2018, 02, 13, 6, 0, 0),
                    End = new DateTime(2018, 02, 14, 6, 0, 0)
        }
            });

            Dictionary<DateTime, FreezingProbability> dico = new Dictionary<DateTime, FreezingProbability>();
            // 44
            dico.Add(dateRef, FreezingProbability.IMMINENT);
            dico.Add(dateRef.AddHours(12), FreezingProbability.IMMINENT);

            //WHEN
            AlarmService alarmService = new AlarmService(deviceRepo.Object, freezeRepo.Object);
            alarmService.CreateFreezeAlarm(deviceId, siteId, dico);

            //THEN
            deviceRepo.Verify(o => o.UpdateAlarm("1", It.IsAny<string>(),
                It.IsAny<DateTime>(),
                new DateTime(2018, 02, 14, 18, 0, 0)),
                Times.Once);
        }

        //GEL -> GEL avec lastFreeze (40)
        [TestMethod]
        public void TestcreateFreezeAlarm40_gel_gel_avecFreeze()
        {
            //GIVEN
            string deviceId = "1";
            string siteId = "1";
            DateTime dateRef = new DateTime(2018, 02, 14, 6, 0, 0);
            Mock<IDeviceRepository> deviceRepo = new Mock<IDeviceRepository>();
            Mock<IFreezeRepository> freezeRepo = new Mock<IFreezeRepository>();
            freezeRepo.Setup(o => o.GetLastFreezeByDevice("1")).Returns(new Freeze
            {
                Date = new DateTime(2018, 02, 14, 0, 0, 0),
                DeviceId = "1",
                TrustIndication = 4
            });
            Dictionary<DateTime, FreezingProbability> dico = new Dictionary<DateTime, FreezingProbability>();
            // 40
            dico.Add(dateRef, FreezingProbability.IMMINENT);
            dico.Add(dateRef.AddHours(12), FreezingProbability.ZERO);

            //WHEN
            AlarmService alarmService = new AlarmService(deviceRepo.Object, freezeRepo.Object);
            alarmService.CreateFreezeAlarm(deviceId, siteId, dico);

            //TODO : Alexis! Degel -> pas de date de fin

            //THEN
            deviceRepo.Verify(o => o.AddAlarm("1", It.Is<Alarm>(e =>
                e.AlarmGravity == Alarm.Gravity.Critical &&
                e.AlarmType == Alarm.Type.FreezeWarning &&
                e.Description.Contains("14/02/2018") &&
                e.Description.Contains("dégel") &&
                e.Description.Contains("1") &&
                e.ShortDescription.Contains("Dégel") &&
                e.ShortDescription.Contains("14/02/2018") &&
                e.Start == new DateTime(2018, 02, 14, 18, 0, 0) &&
                e.End == new DateTime(2018, 02, 14, 18, 0, 0))), Times.Once);
        }

        // LFE last
        //lastfreeze = null / 44
        [TestMethod]
        public void TestcreateFreezeAlarm44_sansFreeze_BD()
        {
            //GIVEN
            string deviceId = "1";
            string siteId = "1";
            DateTime dateRef = new DateTime(2018, 02, 14, 6, 0, 0);
            Mock<IDeviceRepository> deviceRepo = new Mock<IDeviceRepository>();
            Mock<IFreezeRepository> freezeRepo = new Mock<IFreezeRepository>();
            Dictionary<DateTime, FreezingProbability> dico = new Dictionary<DateTime, FreezingProbability>();
            deviceRepo.Setup(o => o.GetCrossAlarmsByDevice("1", new DateTime(2018, 02, 14, 6, 0, 0), new DateTime(2018, 02, 14, 18, 0, 0))).Returns(new List<Alarm>()
            {
                new Alarm
                {
                    Id = "6",
                    Start = new DateTime(2018, 02, 10, 6, 0, 0),
                    End = new DateTime(2018, 02, 14, 7, 0, 0)
                }
            });
            // 44
            dico.Add(dateRef, FreezingProbability.IMMINENT);
            dico.Add(dateRef.AddHours(12), FreezingProbability.IMMINENT);

            //WHEN
            AlarmService alarmService = new AlarmService(deviceRepo.Object, freezeRepo.Object);
            alarmService.CreateFreezeAlarm(deviceId, siteId, dico);

            //THEN
            deviceRepo.Verify(o => o.UpdateAlarm("1", "6",
                It.IsAny<DateTime>(),
                new DateTime(2018, 02, 14, 18, 0, 0)),
                Times.Once);
        }

        // LFE autre
        //lastfreeze = null / 40

        //CFEOG last
        //lastfreeze = null / 044

        //CFEOG autre
        //lastfreeze = null / 040

        // HTCED last
        //lastfreeze = 0 / 44

        // HTCED autre
        //lastfreeze = 0 / 40

        // HTCEPD last
        //lastfreeze = 4 / 44

        // HTCEPD autre
        //lastfreeze = 4 / 40
    }
}
