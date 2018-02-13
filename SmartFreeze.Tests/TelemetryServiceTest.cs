using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SmartFreeze.Repositories;
using SmartFreeze.Services;
using System;
using System.Collections.Generic;
using System.Text;


namespace SmartFreeze.Tests
{
    [TestClass]
    class TelemetryServiceTest
    {
        [TestMethod]
        public void GetByDeviceTest(string deviceId, DateTime? from, DateTime? to, int rowsPerPage, int pageNumber)
        {
            Mock<ITelemetryRepository> alarmRepo = new Mock<ITelemetryRepository>();
            TelemetryService telemetryService = new TelemetryService(alarmRepo.Object);
        }
    }
}
