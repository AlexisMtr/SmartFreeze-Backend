using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NFluent;
using WeatherLibrary.Abstraction;
using WeatherLibrary.Algorithmes.Freeze;
using WeatherLibrary.GoogleMapElevation;

namespace WeatherLibrary.Tests
{
    [TestClass]
    public class FreezingAlgorithmeTest
    {
        FreezingAlgorithme algo;
        DateTime now;

        [TestInitialize]
        public void Setup()
        {
            DateTime now = DateTime.Now;
            Mock<IAltitudeClient> altitudeClientMock = new Mock<IAltitudeClient>();
            Mock<ILogger> logger = new Mock<ILogger>();
            algo = new FreezingAlgorithme(altitudeClientMock.Object, logger.Object);
        }

        [TestMethod]
        public void ExecuteAlgorithmeOnDevice_FreezingIminentTest()
        {
            //Initialize
            Mock<IWeather> deviceMock = new Mock<IWeather>();
            deviceMock.Setup(e => e.Temperature).Returns(-30.0);
            deviceMock.Setup(e => e.Humidity).Returns(80.0);
            deviceMock.Setup(e => e.Date).Returns(now);
            IWeather device = deviceMock.Object;

            //Execute
            FreezeForecast freezing = algo.Execute(device).Result;

            //Test
            Check.That(freezing.FreezingStart.HasValue).IsEqualTo(true);
            Check.That(freezing.FreezingEnd.HasValue).IsEqualTo(false);
            Check.That(freezing.FreezingProbabilityList.Count).IsEqualTo(1);
            Check.That(freezing.FreezingProbabilityList.ContainsKey(now)).IsEqualTo(true);
            Check.That(freezing.FreezingProbabilityList.ContainsValue(FreezeForecast.FreezingProbability.IMMINENT)).IsEqualTo(true);


        }

        [TestMethod]
        public void ExecuteAlorithmeOnDevice_NotFreezingTest()
        {
            //Initialize
            Mock<IWeather> deviceMock = new Mock<IWeather>();
            deviceMock.Setup(e => e.Temperature).Returns(10);
            deviceMock.Setup(e => e.Humidity).Returns(30.0);
            deviceMock.Setup(e => e.Date).Returns(now);
            IWeather device = deviceMock.Object;

            //Execute
            FreezeForecast freezing = algo.Execute(device).Result;

            //Test
            Check.That(freezing.FreezingStart.HasValue).IsEqualTo(true);
            Check.That(freezing.FreezingEnd.HasValue).IsEqualTo(false);
            Check.That(freezing.FreezingProbabilityList.Count).IsEqualTo(1);
            Check.That(freezing.FreezingProbabilityList.ContainsKey(now)).IsEqualTo(true);
            Check.That(freezing.FreezingProbabilityList.ContainsValue(FreezeForecast.FreezingProbability.ZERO)).IsEqualTo(true);
        }
        [TestMethod]
        public void ExecuteAlorithmeOnDevice_MinimumChanceToFreezeTest()
        {
            //Initialize

            Mock<IWeather> deviceMock = new Mock<IWeather>();
            deviceMock.Setup(e => e.Temperature).Returns(4);
            deviceMock.Setup(e => e.Humidity).Returns(30.0);
            deviceMock.Setup(e => e.Date).Returns(now);
            IWeather device = deviceMock.Object;

            //Execute
            FreezeForecast freezing = algo.Execute(device).Result;

            //Test
            Check.That(freezing.FreezingStart.HasValue).IsEqualTo(true);
            Check.That(freezing.FreezingEnd.HasValue).IsEqualTo(false);
            Check.That(freezing.FreezingProbabilityList.Count).IsEqualTo(1);
            Check.That(freezing.FreezingProbabilityList.ContainsKey(now)).IsEqualTo(true);
            Check.That(freezing.FreezingProbabilityList.ContainsValue(FreezeForecast.FreezingProbability.MINIMUM)).IsEqualTo(true);
        }

        [TestMethod]
        public void ExecuteAlorithmeOnDevice_MediumChanceToFreezeTest()
        {
            //Initialize
            Mock<IWeather> deviceMock = new Mock<IWeather>();
            deviceMock.Setup(e => e.Temperature).Returns(-2);
            deviceMock.Setup(e => e.Humidity).Returns(45.0);
            deviceMock.Setup(e => e.Date).Returns(now);
            IWeather device = deviceMock.Object;

            //Execute
            FreezeForecast freezing = algo.Execute(device).Result;

            //Test
            Check.That(freezing.FreezingStart.HasValue).IsEqualTo(true);
            Check.That(freezing.FreezingEnd.HasValue).IsEqualTo(false);
            Check.That(freezing.FreezingProbabilityList.Count).IsEqualTo(1);
            Check.That(freezing.FreezingProbabilityList.ContainsKey(now)).IsEqualTo(true);
            Check.That(freezing.FreezingProbabilityList.ContainsValue(FreezeForecast.FreezingProbability.MEDIUM)).IsEqualTo(true);
        }

        [TestMethod]
        public void ExecuteAlorithmeOnDevice_HighChanceToFreezeTest()
        {
            //Initialize
            Mock<IWeather> deviceMock = new Mock<IWeather>();
            deviceMock.Setup(e => e.Temperature).Returns(-2);
            deviceMock.Setup(e => e.Humidity).Returns(85.0);
            deviceMock.Setup(e => e.Date).Returns(now);
            IWeather device = deviceMock.Object;

            //Execute
            FreezeForecast freezing = algo.Execute(device).Result;

            //Test
            Check.That(freezing.FreezingStart.HasValue).IsEqualTo(true);
            Check.That(freezing.FreezingEnd.HasValue).IsEqualTo(false);
            Check.That(freezing.FreezingProbabilityList.Count).IsEqualTo(1);
            Check.That(freezing.FreezingProbabilityList.ContainsKey(now)).IsEqualTo(true);
            Check.That(freezing.FreezingProbabilityList.ContainsValue(FreezeForecast.FreezingProbability.HIGH)).IsEqualTo(true);

        }
        [TestMethod]
        public void ExecuteAlgorithmeWithForecast_ElevationUnderHundredTest()
        {
            //initialize
            DeviceTest device = new DeviceTest { Temperature = 6.0, Humidity = 80 };

            Mock<IStationPosition> stationDeviceMock = new Mock<IStationPosition>();
            stationDeviceMock.Setup(e => e.Altitude).Returns(10);
            IStationPosition stationDevice = stationDeviceMock.Object;


            WeatherTest freezingToday = new WeatherTest { Temperature = -6.0, Humidity = 80, Date = now };
            //Mock<IWeather> freezingTodayMock = new Mock<IWeather>();
          
            //freezingTodayMock.Setup(e => e.Temperature).Returns(20);
            //freezingTodayMock.Setup(e => e.Humidity).Returns(80);
            //freezingTodayMock.Setup(e => e.Date).Returns(now);
            //IWeather freezingToday = freezingTodayMock.Object;
            IEnumerable<IWeather> forecastList = new List<IWeather> { freezingToday };

            //Mock<IWeather> tomorrowMock = new Mock<IWeather>();
            //tomorrowMock.Setup(e => e.Temperature).Returns(-6);
            //tomorrowMock.Setup(e => e.Humidity).Returns(80);
            //tomorrowMock.Setup(e => e.Date).Returns(now.AddDays(1));
            //IWeather tomorrow = tomorrowMock.Object;
            WeatherTest tomorrow = new WeatherTest { Temperature = 12.0, Humidity = 80, Date = now.AddDays(1) };
            (forecastList as List<IWeather>).Add(tomorrow);

            //Mock<IWeather> afterTomorrowMock = new Mock<IWeather>();
            //afterTomorrowMock.Setup(e => e.Temperature).Returns(-13.0);
            //afterTomorrowMock.Setup(e => e.Humidity).Returns(50.0);
            //afterTomorrowMock.Setup(e => e.Date).Returns(now.AddDays(2));
            //IWeather afterTomorrow = afterTomorrowMock.Object;
            WeatherTest afterTomorrow = new WeatherTest { Temperature = -13.0, Humidity = 50, Date = now.AddDays(2) };
            (forecastList as List<IWeather>).Add(afterTomorrow);

            //Mock<IWeather> afterMock = new Mock<IWeather>();
            //afterMock.Setup(e => e.Temperature).Returns(-42);
            //afterMock.Setup(e => e.Humidity).Returns(90);
            //afterMock.Setup(e => e.Date).Returns(now.AddDays(3));
            //IWeather after = afterMock.Object;
            WeatherTest after = new WeatherTest { Temperature = -42, Humidity = 90, Date = now.AddDays(3) };
            (forecastList as List<IWeather>).Add(after);

            //Mock<IWeather> endFreezeingDayMock = new Mock<IWeather>();
            //endFreezeingDayMock.Setup(e => e.Temperature).Returns(-11.0);
            //endFreezeingDayMock.Setup(e => e.Humidity).Returns(1.0);
            //endFreezeingDayMock.Setup(e => e.Date).Returns(now.AddDays(4));
            //IWeather endFreezeingDay = endFreezeingDayMock.Object;
            WeatherTest endFreezeingDay = new WeatherTest { Temperature = -11.0, Humidity = 1.0, Date = now.AddDays(4) };
            (forecastList as List<IWeather>).Add(endFreezeingDay);

            //Mock<IWeather> endFreezeingMock = new Mock<IWeather>();
            //endFreezeingMock.Setup(e => e.Temperature).Returns(-20);
            //endFreezeingMock.Setup(e => e.Humidity).Returns(85);
            //endFreezeingMock.Setup(e => e.Date).Returns(now.AddDays(5));
            //IWeather endFreezeing = endFreezeingMock.Object;
            WeatherTest endFreezeing = new WeatherTest { Temperature = -20, Humidity = 85, Date = now.AddDays(5) };
            (forecastList as List<IWeather>).Add(endFreezeing);

            Mock<IStationPosition> stationMock = new Mock<IStationPosition>();
            IStationPosition station = stationMock.Object;

            Mock<IAltitudeClient> altitudeClient = new Mock<IAltitudeClient>();
            altitudeClient.Setup(o => o.GetAltitude(It.IsAny<double>(), It.IsAny<double>())).ReturnsAsync(new GmeElevation
            {
                Altitude = 0
            });
            Mock<ILogger> logger = new Mock<ILogger>();
            var freezeAlgo = new FreezingAlgorithme(altitudeClient.Object, logger.Object);

            //execute 
            FreezeForecast freeze = freezeAlgo.Execute(device, stationDevice, freezingToday, forecastList, station).Result;
            
            //test
            Check.That(freeze.FreezingStart.HasValue).IsEqualTo(true);
            Check.That(altitudeClient.Object.GetAltitude(0, 0).Result.Altitude - stationDevice.Altitude).IsStrictlyLessThan(Math.Abs(100));
            Console.WriteLine((forecastList as List<IWeather>).Count);
            Check.That(freeze.FreezingProbabilityList.Count).IsEqualTo((forecastList as List<IWeather>).Count);
            Check.That(freeze.FreezingProbabilityList.GetValueOrDefault(now)).IsEqualTo(FreezeForecast.FreezingProbability.HIGH);
            Check.That(freeze.FreezingProbabilityList.GetValueOrDefault(now.AddDays(1))).IsEqualTo(FreezeForecast.FreezingProbability.ZERO);
            Check.That(freeze.FreezingProbabilityList.GetValueOrDefault(now.AddDays(2))).IsEqualTo(FreezeForecast.FreezingProbability.MEDIUM);
            Check.That(freeze.FreezingProbabilityList.GetValueOrDefault(now.AddDays(3))).IsEqualTo(FreezeForecast.FreezingProbability.IMMINENT);
            Check.That(freeze.FreezingProbabilityList.GetValueOrDefault(now.AddDays(4))).IsEqualTo(FreezeForecast.FreezingProbability.MEDIUM);
            Check.That(freeze.FreezingProbabilityList.GetValueOrDefault(now.AddDays(5))).IsEqualTo(FreezeForecast.FreezingProbability.IMMINENT);
            Check.That(freeze.FreezingEnd.HasValue).IsEqualTo(true);

        }

        [TestMethod]
        public void ExecuteAlgorithmeWithForecast_ElevationOverHundredTest()
        {
            //initialize
            DeviceTest device = new DeviceTest { Temperature = 6.0, Humidity = 80 };

            Mock<IStationPosition> stationDeviceMock = new Mock<IStationPosition>();
            stationDeviceMock.Setup(e => e.Altitude).Returns(1000);
            IStationPosition stationDevice = stationDeviceMock.Object;

            WeatherTest freezingToday = new WeatherTest { Temperature = 7.0, Humidity = 80, Date = now };
            IList<IWeather> forecastList = new List<IWeather> { freezingToday };

            WeatherTest tomorrow = new WeatherTest { Temperature = 5.5, Humidity = 90, Date = now.AddDays(1) };
            forecastList.Add(tomorrow);

            WeatherTest afterTomorrow = new WeatherTest { Temperature = -1.0, Humidity = 90, Date = now.AddDays(2) };
            forecastList.Add(afterTomorrow);

            Mock<IStationPosition> stationMock = new Mock<IStationPosition>();
            IStationPosition station = stationMock.Object;

            Mock<IAltitudeClient> altitudeClient = new Mock<IAltitudeClient>();
            altitudeClient.Setup(o => o.GetAltitude(It.IsAny<double>(), It.IsAny<double>())).ReturnsAsync(new GmeElevation
            {
                Altitude = 0
            });
            Mock<ILogger> logger = new Mock<ILogger>();
            var freezeAlgo = new FreezingAlgorithme(altitudeClient.Object, logger.Object);

            //execute 
            FreezeForecast freeze = freezeAlgo.Execute(device, stationDevice, freezingToday, forecastList, station).Result;

            Check.That(freeze.FreezingStart.HasValue).IsEqualTo(true);
            Check.That(freeze.FreezingEnd.HasValue).IsEqualTo(true);

            Check.That(stationDevice.Altitude).IsStrictlyGreaterThan(100);

            Check.That(freeze.FreezingProbabilityList.Count).IsEqualTo(forecastList.Count());
            Check.That(freeze.FreezingProbabilityList.GetValueOrDefault(now)).IsEqualTo(FreezeForecast.FreezingProbability.ZERO);
            Check.That(freeze.FreezingProbabilityList.GetValueOrDefault(now.AddDays(1))).IsEqualTo(FreezeForecast.FreezingProbability.HIGH);
            Check.That(freeze.FreezingProbabilityList.GetValueOrDefault(now.AddDays(2))).IsEqualTo(FreezeForecast.FreezingProbability.IMMINENT);
        }

        private class DeviceTest : IWeather
        {
            public double Pressure { get; set; }
            public double Humidity { get; set; }
            public double Temperature { get; set; }
            public double WindSpeed { get; set; }
            public DateTime Date { get; set; }
        }
        private class WeatherTest : IWeather
        {
            public double Pressure { get; set; }
            public double Humidity { get; set; }
            public double Temperature { get; set; }
            public double WindSpeed { get; set; }
            public DateTime Date { get; set; }
        }
    }
}


