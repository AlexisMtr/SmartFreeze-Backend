using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SmartFreeze.Services;
using SmartFreeze.Repositories;
using SmartFreeze.Filters;
using SmartFreeze.Models;
using System;
using System.Collections.Generic;
using NFluent;

namespace SmartFreeze.Tests
{
    [TestClass]
    public class AlarmServiceTest
    {
        [TestMethod]
        public void TestGetAll() {
            //GIVEN
            Mock<IAlarmRepository> alarmRepo = new Mock<IAlarmRepository>();
            AlarmService alarmService = new AlarmService(alarmRepo.Object);
            
            //WHEN
            alarmRepo.Setup(o => o.Count(It.IsAny<DeviceAlarmFilter>())).Returns(3).Verifiable();
            alarmRepo.Setup(o => o.Get(It.IsAny<DeviceAlarmFilter>(), 5, 1)).Returns(new List<Alarm>
            {
                new Alarm
                {
                    Id = "A45880",
                    DeviceId = "",
                    SiteId = "",
                    IsActive = true,
                    AlarmType = Alarm.Type.DeviceFailure,
                    AlarmGravity = Alarm.Gravity.Information,
                    OccuredAt = DateTime.UtcNow,
                    ShortDescription = "humidité entre 30 et 39",
                    Description = "L'humidité intérieur est anormale"
                },
                new Alarm
                {
                    Id = "A45881",
                    DeviceId = "",
                    SiteId = "",
                    IsActive = true,
                    AlarmType = Alarm.Type.DeviceFailure,
                    AlarmGravity = Alarm.Gravity.Information,
                    OccuredAt = DateTime.UtcNow,
                    ShortDescription = "humidité entre 30 et 39",
                    Description = "L'humidité intérieur est anormale"
                }
            }).Verifiable();

            AlarmFilter alarmFilter = new AlarmFilter
            {
                AlarmType = Alarm.Type.DeviceFailure,
                Gravity = Alarm.Gravity.Critical
            };

            var items = alarmService.GetAll(alarmFilter, rowsPerPage: 5, pageNumber: 1);

            Check.That(items.PageCount).IsEqualTo(1);
            Check.That(items.TotalItemsCount).IsEqualTo(3);
            Check.That(items.Items).HasSize(2);

            alarmRepo.VerifyAll();
        }


        [TestMethod]
        public void GetByDevice()
        {
            Mock<IAlarmRepository> alarmRepo = new Mock<IAlarmRepository>();
            AlarmService alarmService = new AlarmService(alarmRepo.Object);

            //WHEN
            alarmRepo.Setup(o => o.CountByDevice("A81758FFFE0302EA", It.IsAny<DeviceAlarmFilter>())).Returns(1).Verifiable();
            alarmRepo.Setup(o => o.GetByDevice("A81758FFFE0302EA", It.IsAny<DeviceAlarmFilter>(), 5, 1)).Returns(new List<Alarm>
            {
                new Alarm
                {
                    Id = "A45880",
                    DeviceId = "",
                    SiteId = "",
                    IsActive = true,
                    AlarmType = Alarm.Type.DeviceFailure,
                    AlarmGravity = Alarm.Gravity.Information,
                    OccuredAt = DateTime.UtcNow,
                    ShortDescription = "humidité entre 30 et 39",
                    Description = "L'humidité intérieur est anormale"
                }
            }).Verifiable();

            AlarmFilter alarmFilter = new AlarmFilter
            {
                AlarmType = Alarm.Type.DeviceFailure,
                Gravity = Alarm.Gravity.Critical
            };

            var items = alarmService.GetByDevice("A81758FFFE0302EA",alarmFilter, rowsPerPage: 5, pageNumber: 1);

            Check.That(items.PageCount).IsEqualTo(1);
            Check.That(items.TotalItemsCount).IsEqualTo(1);
            Check.That(items.Items).HasSize(1);

            alarmRepo.VerifyAll();

        }



    }

   
}
