using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NFluent;
using WeatherLibrary.Abstraction;
using WeatherLibrary.Algorithmes.Freeze;

namespace WeatherLibrary.Tests.AlgorithmesTest.FreezeTest
{
    [TestClass]
    public class FreezingAlgorithmeTest
    {
        FreezingAlgorithme algo;

        [TestInitialize]
        public void Setup()
        {
            algo = new FreezingAlgorithme();
        }

        //[TestMethod]
        //public void DewPointTest()
        //{
        //    double temperature = 20.0; // Celsius
        //    double humidity = 45.0;

        //    double result = algo.DewPoint(humidity, temperature);
        //    Console.WriteLine(result);
        //    Check.That(result).IsEqualTo(7.65);
        //}

        //[TestMethod]
        //public void FreezingPointTest()
        //{
        //    double temperature = -20; // Celsius
        //    double dewPoint = 7.65;


        //    double result = algo.FreezingPoint(dewPoint, temperature);
        //    Console.WriteLine(result);
        //    Check.That(result).IsEqualTo(9.70);
        //}

        //[TestMethod]
        //public void IsFreezingTest()
        //{
        //    Mock<IWeather> deviceMock = new Mock<IWeather>();
        //    deviceMock.Setup(e => e.Temperature).Returns(-48.1);

        //    IWeather device = deviceMock.Object;
        //    bool freezing = algo.IsFreezing(device);

        //    Check.That(freezing).IsEqualTo(true);

        //    deviceMock.Setup(e => e.Temperature).Returns(-30.0);
        //    deviceMock.Setup(e => e.Humidity).Returns(80.0);

        //    freezing = algo.IsFreezing(device);
        //    Check.That(freezing).IsEqualTo(true);

        //    deviceMock.Setup(e => e.Temperature).Returns(-1.0);
        //    deviceMock.Setup(e => e.Humidity).Returns(1.0);

        //    freezing = algo.IsFreezing(device);
        //    Check.That(freezing).IsEqualTo(false);
        //}

        [TestMethod]
        public void ExecuteDeviceTest()
        {
            //initialize
            Mock<IWeather> deviceMock = new Mock<IWeather>();
            deviceMock.Setup(e => e.Temperature).Returns(-48.1);
            
            IWeather device = deviceMock.Object;

            //execute
            Task<FreezeForecast> freeze = algo.Execute(device);

            //tests
            Check.That(freeze.Result.FreezingStart.HasValue).IsEqualTo(true);
            Check.That(freeze.Result.FreezingEnd.HasValue).IsEqualTo(false);
            Check.That(freeze.Result.FreezingStart.Value.Day).IsEqualTo(DateTime.Now.Day);
            Check.That(freeze.Result.FreezingStart.Value.Month).IsEqualTo(DateTime.Now.Month);
            Check.That(freeze.Result.FreezingStart.Value.Year).IsEqualTo(DateTime.Now.Year);

            //initialize
            deviceMock.Setup(e => e.Temperature).Returns(-1.0);
            deviceMock.Setup(e => e.Humidity).Returns(1.0);
            device = deviceMock.Object;

            //execute
            freeze = algo.Execute(device);

            //test
            Check.That(freeze.Result.FreezingStart.HasValue).IsEqualTo(false);
            Check.That(freeze.Result.FreezingEnd.HasValue).IsEqualTo(false);

        }
        [TestMethod]
        public void ExecuteForecastFreezingFirstDayTest()
        {
            //initialize
            Mock<IWeather> deviceMock = new Mock<IWeather>();
            deviceMock.Setup(e => e.Temperature).Returns(-30.0);
            deviceMock.Setup(e => e.Humidity).Returns(80.0);
            IWeather device = deviceMock.Object;

            Mock<IStationPosition> stationDeviceMock = new Mock<IStationPosition>();
            IStationPosition stationDevice = stationDeviceMock.Object;

            Mock<IWeather> freezingTodayMock = new Mock<IWeather>();
            freezingTodayMock.Setup(e => e.Temperature).Returns(-48.1);
            freezingTodayMock.Setup(e => e.Date).Returns(DateTime.Now);

            IWeather freezingToday = freezingTodayMock.Object;
            IEnumerable<IWeather> forecastList = new List<IWeather> { freezingToday };

            Mock<IWeather> tomorrowMock = new Mock<IWeather>();
            tomorrowMock.Setup(e => e.Temperature).Returns(-48.1);
            tomorrowMock.Setup(e => e.Date).Returns(DateTime.Now.AddDays(1));
            IWeather tomorrow = tomorrowMock.Object;
            (forecastList as List<IWeather>).Add(tomorrow);

            Mock<IWeather> afterTomorrowMock = new Mock<IWeather>();
            afterTomorrowMock.Setup(e => e.Temperature).Returns(-48.1);
            afterTomorrowMock.Setup(e => e.Date).Returns(DateTime.Now.AddDays(2));
            IWeather afterTomorrow = afterTomorrowMock.Object;
            (forecastList as List<IWeather>).Add(afterTomorrow);

            Mock<IWeather> afterMock = new Mock<IWeather>();
            afterMock.Setup(e => e.Temperature).Returns(-48.1);
            afterMock.Setup(e => e.Date).Returns(DateTime.Now.AddDays(3));
            IWeather after = afterMock.Object;
            (forecastList as List<IWeather>).Add(after);

            Mock<IWeather> endFreezeingDayMock = new Mock<IWeather>();
            endFreezeingDayMock.Setup(e => e.Temperature).Returns(-1.0);
            endFreezeingDayMock.Setup(e => e.Humidity).Returns(1.0);
            endFreezeingDayMock.Setup(e => e.Date).Returns(DateTime.Now.AddDays(4));
            IWeather endFreezeingDay = endFreezeingDayMock.Object;
            (forecastList as List<IWeather>).Add(endFreezeingDay);

            Mock<IWeather> endFreezeingMock = new Mock<IWeather>();
            endFreezeingMock.Setup(e => e.Temperature).Returns(-1.0);
            endFreezeingMock.Setup(e => e.Humidity).Returns(1.0);
            endFreezeingMock.Setup(e => e.Date).Returns(DateTime.Now.AddDays(5));
            IWeather endFreezeing = endFreezeingMock.Object;
            (forecastList as List<IWeather>).Add(endFreezeing);

            Mock<IStationPosition> stationMock = new Mock<IStationPosition>();
            IStationPosition station = stationMock.Object;


            //execute 
            Task<FreezeForecast> freeze = algo.Execute(device, stationDevice, freezingToday, forecastList, station);

            //test
            Check.That(freeze.Result.FreezingStart.HasValue).IsEqualTo(true);
            Check.That(freeze.Result.FreezingStart.Value.Day).IsEqualTo(DateTime.Now.Day);
            Check.That(freeze.Result.FreezingStart.Value.Month).IsEqualTo(DateTime.Now.Month);
            Check.That(freeze.Result.FreezingStart.Value.Year).IsEqualTo(DateTime.Now.Year);

            Check.That(freeze.Result.FreezingEnd.HasValue).IsEqualTo(true);
            Check.That(freeze.Result.FreezingEnd.Value.Day).IsEqualTo(DateTime.Now.AddDays(5).Day);
            Check.That(freeze.Result.FreezingEnd.Value.Month).IsEqualTo(DateTime.Now.AddDays(5).Month);
            Check.That(freeze.Result.FreezingEnd.Value.Year).IsEqualTo(DateTime.Now.AddDays(5).Year);
        }
        [TestMethod]
        public void ExecuteForecastFreezingSecondDayToFourthTest()
        {
            //initialize
            Mock<IWeather> deviceMock = new Mock<IWeather>();
            deviceMock.Setup(e => e.Temperature).Returns(-30.0);
            deviceMock.Setup(e => e.Humidity).Returns(75.0);
            IWeather device = deviceMock.Object;

            deviceMock.SetupGet(o => o.Humidity).Returns(1);

            Mock<IStationPosition> stationDeviceMock = new Mock<IStationPosition>();
            IStationPosition stationDevice = stationDeviceMock.Object;

            Mock<IWeather> freezingTodayMock = new Mock<IWeather>();
            freezingTodayMock.Setup(e => e.Temperature).Returns(-50.1);
            freezingTodayMock.Setup(e => e.Humidity).Returns(90);
            freezingTodayMock.Setup(e => e.Date).Returns(DateTime.Now);
            IWeather freezingToday = freezingTodayMock.Object;
            IEnumerable<IWeather> forecastList = new List<IWeather> { freezingToday };

            Mock<IWeather> tomorrowMock = new Mock<IWeather>();
            tomorrowMock.Setup(e => e.Temperature).Returns(-30.1);
            tomorrowMock.Setup(e => e.Humidity).Returns(100);
            tomorrowMock.Setup(e => e.Date).Returns(DateTime.Now.AddDays(1));
            IWeather tomorrow = tomorrowMock.Object;
            (forecastList as List<IWeather>).Add(tomorrow);

            Mock<IWeather> afterTomorrowMock = new Mock<IWeather>();
            afterTomorrowMock.Setup(e => e.Temperature).Returns(-10.1);
            tomorrowMock.Setup(e => e.Humidity).Returns(50.0);
            afterTomorrowMock.Setup(e => e.Date).Returns(DateTime.Now.AddDays(2));
            IWeather afterTomorrow = afterTomorrowMock.Object;
            (forecastList as List<IWeather>).Add(afterTomorrow);

            Mock<IWeather> afterMock = new Mock<IWeather>();
            afterMock.Setup(e => e.Temperature).Returns(3);
            afterMock.Setup(e => e.Humidity).Returns(0);
            afterMock.Setup(e => e.Date).Returns(DateTime.Now.AddDays(3));
            IWeather after = afterMock.Object;
            (forecastList as List<IWeather>).Add(after);

            Mock<IWeather> endFreezeingDayMock = new Mock<IWeather>();
            endFreezeingDayMock.Setup(e => e.Temperature).Returns(-1.0);
            endFreezeingDayMock.Setup(e => e.Humidity).Returns(1.0);
            endFreezeingDayMock.Setup(e => e.Date).Returns(DateTime.Now.AddDays(4));
            IWeather endFreezeingDay = endFreezeingDayMock.Object;
            (forecastList as List<IWeather>).Add(endFreezeingDay);

            Mock<IWeather> endFreezeingMock = new Mock<IWeather>();
            endFreezeingMock.Setup(e => e.Temperature).Returns(-1.0);
            endFreezeingMock.Setup(e => e.Humidity).Returns(1.0);
            endFreezeingMock.Setup(e => e.Date).Returns(DateTime.Now.AddDays(5));
            IWeather endFreezeing = endFreezeingMock.Object;
            (forecastList as List<IWeather>).Add(endFreezeing);

            Mock<IStationPosition> stationMock = new Mock<IStationPosition>();
            IStationPosition station = stationMock.Object;


            //execute 
            Task<FreezeForecast> freeze = algo.Execute(device, stationDevice, freezingToday, forecastList, station);

            //test
            Check.That(freeze.Result.FreezingStart.HasValue).IsEqualTo(true);
            Check.That(freeze.Result.FreezingStart.Value.Day).IsEqualTo(DateTime.Now.Day);
            Check.That(freeze.Result.FreezingStart.Value.Month).IsEqualTo(DateTime.Now.Month);
            Check.That(freeze.Result.FreezingStart.Value.Year).IsEqualTo(DateTime.Now.Year);

            Check.That(freeze.Result.FreezingEnd.HasValue).IsEqualTo(true);
            Check.That(freeze.Result.FreezingEnd.Value.Day).IsEqualTo(DateTime.Now.AddDays(5).Day);
            Check.That(freeze.Result.FreezingEnd.Value.Month).IsEqualTo(DateTime.Now.AddDays(5).Month);
            Check.That(freeze.Result.FreezingEnd.Value.Year).IsEqualTo(DateTime.Now.AddDays(5).Year);

        }
    }

}
