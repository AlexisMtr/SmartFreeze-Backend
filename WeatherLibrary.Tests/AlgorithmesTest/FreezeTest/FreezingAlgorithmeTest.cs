using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NFluent;
using WeatherLibrary.Abstraction;
using WeatherLibrary.Algorithmes.Freeze;
using WeatherLibrary.GoogleMapElevation;

namespace WeatherLibrary.Tests.AlgorithmesTest.FreezeTest
{
    [TestClass]
    public class FreezingAlgorithmeTest
    {
        FreezingAlgorithme algo;

        [TestInitialize]
        public void Setup()
        {
            Mock<IAltitudeClient> altitudeClientMock = new Mock<IAltitudeClient>();
            algo = new FreezingAlgorithme(altitudeClientMock.Object);
        }

        [TestMethod]
        public void ExecuteDeviceTest()
        {
            //initialize
            Mock<IWeather> deviceMock = new Mock<IWeather>();
            deviceMock.Setup(e => e.Temperature).Returns(-48.1);
            
            IWeather device = deviceMock.Object;

            //execute
            FreezeForecast freeze = algo.Execute(device).Result;

            //tests
            Check.That(freeze.FreezingStart.HasValue).IsEqualTo(true);
            Check.That(freeze.FreezingEnd.HasValue).IsEqualTo(false);
            Check.That(freeze.FreezingStart.Value.Day).IsEqualTo(DateTime.Now.Day);
            Check.That(freeze.FreezingStart.Value.Month).IsEqualTo(DateTime.Now.Month);
            Check.That(freeze.FreezingStart.Value.Year).IsEqualTo(DateTime.Now.Year);

            //initialize
            deviceMock.Setup(e => e.Temperature).Returns(-1.0);
            deviceMock.Setup(e => e.Humidity).Returns(1.0);
            device = deviceMock.Object;

            //execute
            freeze = algo.Execute(device).Result;

            //test
            Check.That(freeze.FreezingStart.HasValue).IsEqualTo(false);
            Check.That(freeze.FreezingEnd.HasValue).IsEqualTo(false);

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
            stationDeviceMock.Setup(e => e.Altitude).Returns(1_600);
            IStationPosition stationDevice = stationDeviceMock.Object;

            Mock<IWeather> freezingTodayMock = new Mock<IWeather>();
            freezingTodayMock.Setup(e => e.Temperature).Returns(-48.1);
            freezingTodayMock.Setup(e => e.Humidity).Returns(80.0);
            freezingTodayMock.Setup(e => e.Date).Returns(DateTime.Now);

            IWeather freezingToday = freezingTodayMock.Object;
            IEnumerable<IWeather> forecastList = new List<IWeather> { freezingToday };

            Mock<IWeather> tomorrowMock = new Mock<IWeather>();
            tomorrowMock.Setup(e => e.Temperature).Returns(-48.1);
            tomorrowMock.Setup(e => e.Humidity).Returns(80.0);
            tomorrowMock.Setup(e => e.Date).Returns(DateTime.Now.AddDays(1));
            IWeather tomorrow = tomorrowMock.Object;
            (forecastList as List<IWeather>).Add(tomorrow);

            Mock<IWeather> afterTomorrowMock = new Mock<IWeather>();
            afterTomorrowMock.Setup(e => e.Temperature).Returns(-48.1);
            afterTomorrowMock.Setup(e => e.Humidity).Returns(80.0);
            afterTomorrowMock.Setup(e => e.Date).Returns(DateTime.Now.AddDays(2));
            IWeather afterTomorrow = afterTomorrowMock.Object;
            (forecastList as List<IWeather>).Add(afterTomorrow);

            Mock<IWeather> afterMock = new Mock<IWeather>();
            afterMock.Setup(e => e.Temperature).Returns(-48.1);
            afterMock.Setup(e => e.Humidity).Returns(80.0);
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

            //39.7391536, -104.9847034 => Elevation : 1600m 
            Mock<IStationPosition> weatherStationMock = new Mock<IStationPosition>();
            weatherStationMock.Setup(e => e.Latitude).Returns(39.7391536);
            weatherStationMock.Setup(e => e.Longitude).Returns(-104.9847034);
            IStationPosition weatherStation = weatherStationMock.Object;

            Mock<IAltitudeClient> altitudeClient = new Mock<IAltitudeClient>();
            altitudeClient.Setup(o => o.GetAltitude(It.IsAny<double>(), It.IsAny<double>())).ReturnsAsync(new GmeElevation
            {
                Altitude = 0
            });
            var freezeAlgo = new FreezingAlgorithme(altitudeClient.Object);
            
            //execute 
            FreezeForecast freeze = freezeAlgo.Execute(device, stationDevice, freezingToday, forecastList, weatherStation).Result;

            //test

            Check.That(freeze.FreezingStart.HasValue).IsEqualTo(true);
            Check.That(freeze.FreezingStart.Value.Day).IsEqualTo(DateTime.Now.Day);
            Check.That(freeze.FreezingStart.Value.Month).IsEqualTo(DateTime.Now.Month);
            Check.That(freeze.FreezingStart.Value.Year).IsEqualTo(DateTime.Now.Year);

            // TODO : Clarck, check values, test failed
            Check.That(freeze.FreezingEnd.HasValue).IsEqualTo(true);
            Check.That(freeze.FreezingEnd.Value.Day).IsEqualTo(DateTime.Now.AddDays(3).Day);
            Check.That(freeze.FreezingEnd.Value.Month).IsEqualTo(DateTime.Now.AddDays(3).Month);
            Check.That(freeze.FreezingEnd.Value.Year).IsEqualTo(DateTime.Now.AddDays(3).Year);
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

            Mock<IAltitudeClient> altitudeClient = new Mock<IAltitudeClient>();
            altitudeClient.Setup(o => o.GetAltitude(It.IsAny<double>(), It.IsAny<double>())).ReturnsAsync(new GmeElevation
            {
                Altitude = 0
            });
            var freezeAlgo = new FreezingAlgorithme(altitudeClient.Object);

            //execute 
            FreezeForecast freeze = freezeAlgo.Execute(device, stationDevice, freezingToday, forecastList, station).Result;

            //test
            // TODO : Clark, check values, test failed
            Check.That(freeze.FreezingStart.HasValue).IsEqualTo(true);
            Check.That(freeze.FreezingStart.Value.Day).IsEqualTo(DateTime.Now.Day);
            Check.That(freeze.FreezingStart.Value.Month).IsEqualTo(DateTime.Now.Month);
            Check.That(freeze.FreezingStart.Value.Year).IsEqualTo(DateTime.Now.Year);

            Check.That(freeze.FreezingEnd.HasValue).IsEqualTo(true);
            Check.That(freeze.FreezingEnd.Value.Day).IsEqualTo(DateTime.Now.AddDays(5).Day);
            Check.That(freeze.FreezingEnd.Value.Month).IsEqualTo(DateTime.Now.AddDays(5).Month);
            Check.That(freeze.FreezingEnd.Value.Year).IsEqualTo(DateTime.Now.AddDays(5).Year);

        }
    }

}
