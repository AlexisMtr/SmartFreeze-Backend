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

        [TestInitialize]
        public void Setup()
        {
            Mock<IAltitudeClient> altitudeClientMock = new Mock<IAltitudeClient>();
            algo = new FreezingAlgorithme(altitudeClientMock.Object);
        }

        //[TestMethod]
        //public void ExecuteDeviceTest()
        //{
        //    //initialize
        //    Mock<IWeather> deviceMock = new Mock<IWeather>();
        //    deviceMock.Setup(e => e.Temperature).Returns(-48.1);

        //    IWeather device = deviceMock.Object;

        //    //execute
        //    FreezeForecast freeze = algo.Execute(device).Result;

        //    //tests
        //    Check.That(freeze.FreezingStart.HasValue).IsEqualTo(true);
        //    Check.That(freeze.FreezingEnd.HasValue).IsEqualTo(false);
        //    Check.That(freeze.FreezingStart.Value.Day).IsEqualTo(DateTime.Now.Day);
        //    Check.That(freeze.FreezingStart.Value.Month).IsEqualTo(DateTime.Now.Month);
        //    Check.That(freeze.FreezingStart.Value.Year).IsEqualTo(DateTime.Now.Year);

        //    //initialize
        //    deviceMock.Setup(e => e.Temperature).Returns(-1.0);
        //    deviceMock.Setup(e => e.Humidity).Returns(1.0);
        //    device = deviceMock.Object;

        //    //execute
        //    freeze = algo.Execute(device).Result;

        //    //test
        //    Check.That(freeze.FreezingStart.HasValue).IsEqualTo(false);
        //    Check.That(freeze.FreezingEnd.HasValue).IsEqualTo(false);

        //}
        //[TestMethod]
        //public void ExecuteForecastFreezingFirstDayTest()
        //{
        //    //initialize
        //    Mock<IWeather> deviceMock = new Mock<IWeather>();
        //    deviceMock.Setup(e => e.Temperature).Returns(-30.0);
        //    deviceMock.Setup(e => e.Humidity).Returns(80.0);
        //    IWeather device = deviceMock.Object;

        //    Mock<IStationPosition> stationDeviceMock = new Mock<IStationPosition>();
        //    stationDeviceMock.Setup(e => e.Altitude).Returns(1_600);
        //    IStationPosition stationDevice = stationDeviceMock.Object;

        //    Mock<IWeather> freezingTodayMock = new Mock<IWeather>();
        //    freezingTodayMock.Setup(e => e.Temperature).Returns(-48.1);
        //    freezingTodayMock.Setup(e => e.Humidity).Returns(80.0);
        //    freezingTodayMock.Setup(e => e.Date).Returns(DateTime.Now);

        //    IWeather freezingToday = freezingTodayMock.Object;
        //    IEnumerable<IWeather> forecastList = new List<IWeather> { freezingToday };

        //    Mock<IWeather> tomorrowMock = new Mock<IWeather>();
        //    tomorrowMock.Setup(e => e.Temperature).Returns(-48.1);
        //    tomorrowMock.Setup(e => e.Humidity).Returns(80.0);
        //    tomorrowMock.Setup(e => e.Date).Returns(DateTime.Now.AddDays(1));
        //    IWeather tomorrow = tomorrowMock.Object;
        //    (forecastList as List<IWeather>).Add(tomorrow);

        //    Mock<IWeather> afterTomorrowMock = new Mock<IWeather>();
        //    afterTomorrowMock.Setup(e => e.Temperature).Returns(-48.1);
        //    afterTomorrowMock.Setup(e => e.Humidity).Returns(80.0);
        //    afterTomorrowMock.Setup(e => e.Date).Returns(DateTime.Now.AddDays(2));
        //    IWeather afterTomorrow = afterTomorrowMock.Object;
        //    (forecastList as List<IWeather>).Add(afterTomorrow);

        //    Mock<IWeather> afterMock = new Mock<IWeather>();
        //    afterMock.Setup(e => e.Temperature).Returns(-48.1);
        //    afterMock.Setup(e => e.Humidity).Returns(80.0);
        //    afterMock.Setup(e => e.Date).Returns(DateTime.Now.AddDays(3));
        //    IWeather after = afterMock.Object;
        //    (forecastList as List<IWeather>).Add(after);

        //    Mock<IWeather> endFreezeingDayMock = new Mock<IWeather>();
        //    endFreezeingDayMock.Setup(e => e.Temperature).Returns(-1.0);
        //    endFreezeingDayMock.Setup(e => e.Humidity).Returns(1.0);
        //    endFreezeingDayMock.Setup(e => e.Date).Returns(DateTime.Now.AddDays(4));
        //    IWeather endFreezeingDay = endFreezeingDayMock.Object;
        //    (forecastList as List<IWeather>).Add(endFreezeingDay);

        //    Mock<IWeather> endFreezeingMock = new Mock<IWeather>();
        //    endFreezeingMock.Setup(e => e.Temperature).Returns(-1.0);
        //    endFreezeingMock.Setup(e => e.Humidity).Returns(1.0);
        //    endFreezeingMock.Setup(e => e.Date).Returns(DateTime.Now.AddDays(5));
        //    IWeather endFreezeing = endFreezeingMock.Object;
        //    (forecastList as List<IWeather>).Add(endFreezeing);

        //    //39.7391536, -104.9847034 => Elevation : 1600m 
        //    Mock<IStationPosition> weatherStationMock = new Mock<IStationPosition>();
        //    weatherStationMock.Setup(e => e.Latitude).Returns(39.7391536);
        //    weatherStationMock.Setup(e => e.Longitude).Returns(-104.9847034);
        //    IStationPosition weatherStation = weatherStationMock.Object;

        //    Mock<IAltitudeClient> altitudeClient = new Mock<IAltitudeClient>();
        //    altitudeClient.Setup(o => o.GetAltitude(It.IsAny<double>(), It.IsAny<double>())).ReturnsAsync(new GmeElevation
        //    {
        //        Altitude = 0
        //    });
        //    var freezeAlgo = new FreezingAlgorithme(altitudeClient.Object);

        //    //execute 
        //    FreezeForecast freeze = freezeAlgo.Execute(device, stationDevice, freezingToday, forecastList, weatherStation).Result;

        //    //test

        //    Check.That(freeze.FreezingStart.HasValue).IsEqualTo(true);
        //    Check.That(freeze.FreezingStart.Value.Day).IsEqualTo(DateTime.Now.Day);
        //    Check.That(freeze.FreezingStart.Value.Month).IsEqualTo(DateTime.Now.Month);
        //    Check.That(freeze.FreezingStart.Value.Year).IsEqualTo(DateTime.Now.Year);

        //    // TODO : Clarck, check values, test failed
        //    Check.That(freeze.FreezingEnd.HasValue).IsEqualTo(true);
        //    Check.That(freeze.FreezingEnd.Value.Day).IsEqualTo(DateTime.Now.AddDays(3).Day);
        //    Check.That(freeze.FreezingEnd.Value.Month).IsEqualTo(DateTime.Now.AddDays(3).Month);
        //    Check.That(freeze.FreezingEnd.Value.Year).IsEqualTo(DateTime.Now.AddDays(3).Year);
        //}
        //[TestMethod]
        //public void ExecuteForecastFreezingSecondDayToFourthTest()
        //{
        //    //initialize
        //    Mock<IWeather> deviceMock = new Mock<IWeather>();
        //    deviceMock.Setup(e => e.Temperature).Returns(-30.0);
        //    deviceMock.Setup(e => e.Humidity).Returns(75.0);
        //    IWeather device = deviceMock.Object;

        //    deviceMock.SetupGet(o => o.Humidity).Returns(1);

        //    Mock<IStationPosition> stationDeviceMock = new Mock<IStationPosition>();
        //    IStationPosition stationDevice = stationDeviceMock.Object;

        //    Mock<IWeather> freezingTodayMock = new Mock<IWeather>();
        //    freezingTodayMock.Setup(e => e.Temperature).Returns(-50.1);
        //    freezingTodayMock.Setup(e => e.Humidity).Returns(90);
        //    freezingTodayMock.Setup(e => e.Date).Returns(DateTime.Now);
        //    IWeather freezingToday = freezingTodayMock.Object;
        //    IEnumerable<IWeather> forecastList = new List<IWeather> { freezingToday };

        //    Mock<IWeather> tomorrowMock = new Mock<IWeather>();
        //    tomorrowMock.Setup(e => e.Temperature).Returns(-30.1);
        //    tomorrowMock.Setup(e => e.Humidity).Returns(100);
        //    tomorrowMock.Setup(e => e.Date).Returns(DateTime.Now.AddDays(1));
        //    IWeather tomorrow = tomorrowMock.Object;
        //    (forecastList as List<IWeather>).Add(tomorrow);

        //    Mock<IWeather> afterTomorrowMock = new Mock<IWeather>();
        //    afterTomorrowMock.Setup(e => e.Temperature).Returns(-10.1);
        //    tomorrowMock.Setup(e => e.Humidity).Returns(50.0);
        //    afterTomorrowMock.Setup(e => e.Date).Returns(DateTime.Now.AddDays(2));
        //    IWeather afterTomorrow = afterTomorrowMock.Object;
        //    (forecastList as List<IWeather>).Add(afterTomorrow);

        //    Mock<IWeather> afterMock = new Mock<IWeather>();
        //    afterMock.Setup(e => e.Temperature).Returns(3);
        //    afterMock.Setup(e => e.Humidity).Returns(0);
        //    afterMock.Setup(e => e.Date).Returns(DateTime.Now.AddDays(3));
        //    IWeather after = afterMock.Object;
        //    (forecastList as List<IWeather>).Add(after);

        //    Mock<IWeather> endFreezeingDayMock = new Mock<IWeather>();
        //    endFreezeingDayMock.Setup(e => e.Temperature).Returns(-1.0);
        //    endFreezeingDayMock.Setup(e => e.Humidity).Returns(1.0);
        //    endFreezeingDayMock.Setup(e => e.Date).Returns(DateTime.Now.AddDays(4));
        //    IWeather endFreezeingDay = endFreezeingDayMock.Object;
        //    (forecastList as List<IWeather>).Add(endFreezeingDay);

        //    Mock<IWeather> endFreezeingMock = new Mock<IWeather>();
        //    endFreezeingMock.Setup(e => e.Temperature).Returns(-1.0);
        //    endFreezeingMock.Setup(e => e.Humidity).Returns(1.0);
        //    endFreezeingMock.Setup(e => e.Date).Returns(DateTime.Now.AddDays(5));
        //    IWeather endFreezeing = endFreezeingMock.Object;
        //    (forecastList as List<IWeather>).Add(endFreezeing);

        //    Mock<IStationPosition> stationMock = new Mock<IStationPosition>();
        //    IStationPosition station = stationMock.Object;

        //    Mock<IAltitudeClient> altitudeClient = new Mock<IAltitudeClient>();
        //    altitudeClient.Setup(o => o.GetAltitude(It.IsAny<double>(), It.IsAny<double>())).ReturnsAsync(new GmeElevation
        //    {
        //        Altitude = 0
        //    });
        //    var freezeAlgo = new FreezingAlgorithme(altitudeClient.Object);

        //    //execute 
        //    FreezeForecast freeze = freezeAlgo.Execute(device, stationDevice, freezingToday, forecastList, station).Result;

        //    //test
        //    // TODO : Clark, check values, test failed
        //    Check.That(freeze.FreezingStart.HasValue).IsEqualTo(true);
        //    Check.That(freeze.FreezingStart.Value.Day).IsEqualTo(DateTime.Now.Day);
        //    Check.That(freeze.FreezingStart.Value.Month).IsEqualTo(DateTime.Now.Month);
        //    Check.That(freeze.FreezingStart.Value.Year).IsEqualTo(DateTime.Now.Year);

        //    Check.That(freeze.FreezingEnd.HasValue).IsEqualTo(true);
        //    Check.That(freeze.FreezingEnd.Value.Day).IsEqualTo(DateTime.Now.AddDays(5).Day);
        //    Check.That(freeze.FreezingEnd.Value.Month).IsEqualTo(DateTime.Now.AddDays(5).Month);
        //    Check.That(freeze.FreezingEnd.Value.Year).IsEqualTo(DateTime.Now.AddDays(5).Year);

        //}

        [TestMethod]
        public void ExecuteAlgorithmeOnDevice_FreezingIminentTest()
        {
            //Initialize
            Mock<IWeather> deviceMock = new Mock<IWeather>();
            deviceMock.Setup(e => e.Temperature).Returns(-30.0);
            deviceMock.Setup(e => e.Humidity).Returns(80.0);
            deviceMock.Setup(e => e.Date).Returns(DateTime.Now);
            IWeather device = deviceMock.Object;

            //Execute
            FreezeForecast freezing = algo.Execute(device).Result;

            //Test
            Check.That(freezing.FreezingStart.HasValue).IsEqualTo(true);
            Check.That(freezing.FreezingEnd.HasValue).IsEqualTo(false);
            Check.That(freezing.FreezingProbabilityList.Count).IsEqualTo(1);
            Check.That(freezing.FreezingProbabilityList[0]).IsEqualTo(FreezeForecast.FreezingProbability.IMMINENT);

        }

        [TestMethod]
        public void ExecuteAlorithmeOnDevice_NotFreezingTest()
        {
            //Initialize
            Mock<IWeather> deviceMock = new Mock<IWeather>();
            deviceMock.Setup(e => e.Temperature).Returns(10);
            deviceMock.Setup(e => e.Humidity).Returns(30.0);
            deviceMock.Setup(e => e.Date).Returns(DateTime.Now);
            IWeather device = deviceMock.Object;

            //Execute
            FreezeForecast freezing = algo.Execute(device).Result;

            //Test
            Check.That(freezing.FreezingStart.HasValue).IsEqualTo(true);
            Check.That(freezing.FreezingEnd.HasValue).IsEqualTo(false);
            Check.That(freezing.FreezingProbabilityList.Count).IsEqualTo(1);
            Check.That(freezing.FreezingProbabilityList[0]).IsEqualTo(FreezeForecast.FreezingProbability.ZERO);
        }
        [TestMethod]
        public void ExecuteAlorithmeOnDevice_MinimumChanceToFreezeTest()
        {
            //Initialize
            Mock<IWeather> deviceMock = new Mock<IWeather>();
            deviceMock.Setup(e => e.Temperature).Returns(4);
            deviceMock.Setup(e => e.Humidity).Returns(30.0);
            deviceMock.Setup(e => e.Date).Returns(DateTime.Now);
            IWeather device = deviceMock.Object;

            //Execute
            FreezeForecast freezing = algo.Execute(device).Result;

            //Test
            Check.That(freezing.FreezingStart.HasValue).IsEqualTo(true);
            Check.That(freezing.FreezingEnd.HasValue).IsEqualTo(false);
            Check.That(freezing.FreezingProbabilityList.Count).IsEqualTo(1);
            Check.That(freezing.FreezingProbabilityList[0]).IsEqualTo(FreezeForecast.FreezingProbability.MINIMUM);
        }

        [TestMethod]
        public void ExecuteAlorithmeOnDevice_MediumChanceToFreezeTest()
        {
            //Initialize
            Mock<IWeather> deviceMock = new Mock<IWeather>();
            deviceMock.Setup(e => e.Temperature).Returns(-2);
            deviceMock.Setup(e => e.Humidity).Returns(45.0);
            deviceMock.Setup(e => e.Date).Returns(DateTime.Now);
            IWeather device = deviceMock.Object;

            //Execute
            FreezeForecast freezing = algo.Execute(device).Result;

            //Test
            Check.That(freezing.FreezingStart.HasValue).IsEqualTo(true);
            Check.That(freezing.FreezingEnd.HasValue).IsEqualTo(false);
            Check.That(freezing.FreezingProbabilityList.Count).IsEqualTo(1);
            Check.That(freezing.FreezingProbabilityList[0]).IsEqualTo(FreezeForecast.FreezingProbability.MEDIUM);
        }

        [TestMethod]
        public void ExecuteAlorithmeOnDevice_HighChanceToFreezeTest()
        {
            //Initialize
            Mock<IWeather> deviceMock = new Mock<IWeather>();
            deviceMock.Setup(e => e.Temperature).Returns(-2);
            deviceMock.Setup(e => e.Humidity).Returns(85.0);
            deviceMock.Setup(e => e.Date).Returns(DateTime.Now);
            IWeather device = deviceMock.Object;

            //Execute
            FreezeForecast freezing = algo.Execute(device).Result;

            //Test
            Check.That(freezing.FreezingStart.HasValue).IsEqualTo(true);
            Check.That(freezing.FreezingEnd.HasValue).IsEqualTo(false);
            Check.That(freezing.FreezingProbabilityList.Count).IsEqualTo(1);
            Check.That(freezing.FreezingProbabilityList[0]).IsEqualTo(FreezeForecast.FreezingProbability.HIGH);
        }

        [TestMethod]
        public void ExecuteAlgorithmeWithForecast_ElevationUnderHundredTest()
        {
            //initialize
            //Mock<IWeather> deviceMock = new Mock<IWeather>();
            //deviceMock.SetupGet(e => e.Temperature).Returns(6.0);
            //deviceMock.SetupGet(e => e.Humidity).Returns(75.0);
            //IWeather device = deviceMock.Object;

            DeviceTest device = new DeviceTest { Temperature = 6.0, Humidity = 80};


            Mock<IStationPosition> stationDeviceMock = new Mock<IStationPosition>();
            stationDeviceMock.Setup(e => e.Altitude).Returns(10);
            IStationPosition stationDevice = stationDeviceMock.Object;

            Mock<IWeather> freezingTodayMock = new Mock<IWeather>();
            freezingTodayMock.Setup(e => e.Temperature).Returns(-6.0);
            freezingTodayMock.Setup(e => e.Humidity).Returns(80);
            freezingTodayMock.Setup(e => e.Date).Returns(DateTime.Now);
            IWeather freezingToday = freezingTodayMock.Object;
            IEnumerable<IWeather> forecastList = new List<IWeather> { freezingToday };

            Mock<IWeather> tomorrowMock = new Mock<IWeather>();
            tomorrowMock.Setup(e => e.Temperature).Returns(-6);
            tomorrowMock.Setup(e => e.Humidity).Returns(80);
            tomorrowMock.Setup(e => e.Date).Returns(DateTime.Now.AddDays(1));
            IWeather tomorrow = tomorrowMock.Object;
            (forecastList as List<IWeather>).Add(tomorrow);

            Mock<IWeather> afterTomorrowMock = new Mock<IWeather>();
            afterTomorrowMock.Setup(e => e.Temperature).Returns(-13.0);
            tomorrowMock.Setup(e => e.Humidity).Returns(50.0);
            afterTomorrowMock.Setup(e => e.Date).Returns(DateTime.Now.AddDays(2));
            IWeather afterTomorrow = afterTomorrowMock.Object;
            (forecastList as List<IWeather>).Add(afterTomorrow);

            Mock<IWeather> afterMock = new Mock<IWeather>();
            afterMock.Setup(e => e.Temperature).Returns(-42);
            afterMock.Setup(e => e.Humidity).Returns(90);
            afterMock.Setup(e => e.Date).Returns(DateTime.Now.AddDays(3));
            IWeather after = afterMock.Object;
            (forecastList as List<IWeather>).Add(after);

            Mock<IWeather> endFreezeingDayMock = new Mock<IWeather>();
            endFreezeingDayMock.Setup(e => e.Temperature).Returns(-11.0);
            endFreezeingDayMock.Setup(e => e.Humidity).Returns(1.0);
            endFreezeingDayMock.Setup(e => e.Date).Returns(DateTime.Now.AddDays(4));
            IWeather endFreezeingDay = endFreezeingDayMock.Object;
            (forecastList as List<IWeather>).Add(endFreezeingDay);

            Mock<IWeather> endFreezeingMock = new Mock<IWeather>();
            endFreezeingMock.Setup(e => e.Temperature).Returns(-20);
            endFreezeingMock.Setup(e => e.Humidity).Returns(85);
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

            //test voir l'idée du coeff ... 
            Check.That(freeze.FreezingStart.HasValue).IsEqualTo(true);
            Check.That(altitudeClient.Object.GetAltitude(0, 0).Result.Altitude - stationDevice.Altitude).IsStrictlyLessThan(Math.Abs(100));
            Console.WriteLine((forecastList as List<IWeather>).Count);
            Check.That(freeze.FreezingProbabilityList.Count - 1).IsEqualTo((forecastList as List<IWeather>).Count);
            Check.That(freeze.FreezingProbabilityList[0]).IsEqualTo(FreezeForecast.FreezingProbability.ZERO);
            Check.That(freeze.FreezingProbabilityList[1]).IsEqualTo(FreezeForecast.FreezingProbability.ZERO); 
            Check.That(freeze.FreezingProbabilityList[2]).IsEqualTo(FreezeForecast.FreezingProbability.ZERO);
            Check.That(freeze.FreezingProbabilityList[3]).IsEqualTo(FreezeForecast.FreezingProbability.MEDIUM);
            Check.That(freeze.FreezingProbabilityList[4]).IsEqualTo(FreezeForecast.FreezingProbability.IMMINENT);
            Check.That(freeze.FreezingProbabilityList[5]).IsEqualTo(FreezeForecast.FreezingProbability.MINIMUM);
            Check.That(freeze.FreezingProbabilityList[6]).IsEqualTo(FreezeForecast.FreezingProbability.HIGH);
            Check.That(freeze.FreezingEnd.HasValue).IsEqualTo(true);

        }

        [TestMethod]
        public void ExecuteAlgorithmeWithForecast_ElevationOverHundredTest()

        {
            //initialize
            //Mock<IWeather> deviceMock = new Mock<IWeather>();
            //deviceMock.SetupGet(e => e.Temperature).Returns(6.0);
            //deviceMock.SetupGet(e => e.Humidity).Returns(75.0);
            //IWeather device = deviceMock.Object;
            DeviceTest device = new DeviceTest { Temperature = 6.0, Humidity = 80 };

            Mock<IStationPosition> stationDeviceMock = new Mock<IStationPosition>();
            stationDeviceMock.Setup(e => e.Altitude).Returns(1000);
            IStationPosition stationDevice = stationDeviceMock.Object;

            WeatherTest freezingToday = new WeatherTest { Temperature = 6.0, Humidity = 80 };

            //Mock<IWeather> freezingTodayMock = new Mock<IWeather>();
            //freezingTodayMock.Setup(e => e.Temperature).Returns(-6.0);
            //freezingTodayMock.Setup(e => e.Humidity).Returns(80);
            //freezingTodayMock.Setup(e => e.Date).Returns(DateTime.Now);
            //IWeather freezingToday = freezingTodayMock.Object;
            IEnumerable<IWeather> forecastList = new List<IWeather> { freezingToday };

            WeatherTest tomorrow = new WeatherTest { Temperature = -10.0, Humidity = 90 };
            //Mock<IWeather> tomorrowMock = new Mock<IWeather>();
            //tomorrowMock.Setup(e => e.Temperature).Returns(-10);
            //tomorrowMock.Setup(e => e.Humidity).Returns(80);
            //tomorrowMock.Setup(e => e.Date).Returns(DateTime.Now.AddDays(1));
            //IWeather tomorrow = tomorrowMock.Object;
            (forecastList as List<IWeather>).Add(tomorrow);

            WeatherTest afterTomorrow = new WeatherTest { Temperature = -30.0, Humidity = 90 };
            //Mock<IWeather> afterTomorrowMock = new Mock<IWeather>();
            //afterTomorrowMock.Setup(e => e.Temperature).Returns(-30.0);
            //tomorrowMock.Setup(e => e.Humidity).Returns(90.0);
            //afterTomorrowMock.Setup(e => e.Date).Returns(DateTime.Now.AddDays(2));
            //IWeather afterTomorrow = afterTomorrowMock.Object;
            (forecastList as List<IWeather>).Add(afterTomorrow);

            //Mock<IWeather> afterMock = new Mock<IWeather>();
            //afterMock.Setup(e => e.Temperature).Returns(3);
            //afterMock.Setup(e => e.Humidity).Returns(0);
            //afterMock.Setup(e => e.Date).Returns(DateTime.Now.AddDays(3));
            //IWeather after = afterMock.Object;
            //(forecastList as List<IWeather>).Add(after);

            //Mock<IWeather> endFreezeingDayMock = new Mock<IWeather>();
            //endFreezeingDayMock.Setup(e => e.Temperature).Returns(-1.0);
            //endFreezeingDayMock.Setup(e => e.Humidity).Returns(1.0);
            //endFreezeingDayMock.Setup(e => e.Date).Returns(DateTime.Now.AddDays(4));
            //IWeather endFreezeingDay = endFreezeingDayMock.Object;
            //(forecastList as List<IWeather>).Add(endFreezeingDay);

            //Mock<IWeather> endFreezeingMock = new Mock<IWeather>();
            //endFreezeingMock.Setup(e => e.Temperature).Returns(-1.0);
            //endFreezeingMock.Setup(e => e.Humidity).Returns(1.0);
            //endFreezeingMock.Setup(e => e.Date).Returns(DateTime.Now.AddDays(5));
            //IWeather endFreezeing = endFreezeingMock.Object;
            //(forecastList as List<IWeather>).Add(endFreezeing);

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

            //test voir l'idée du coeff ... 
            Check.That(freeze.FreezingStart.HasValue).IsEqualTo(true);
            Check.That(Math.Abs(altitudeClient.Object.GetAltitude(0, 0).Result.Altitude - stationDevice.Altitude)).IsStrictlyGreaterThan(Math.Abs(100));
            Console.WriteLine((forecastList as List<IWeather>).Count);
            Check.That(freeze.FreezingProbabilityList.Count - 1).IsEqualTo((forecastList as List<IWeather>).Count);
            Check.That(freeze.FreezingProbabilityList[0]).IsEqualTo(FreezeForecast.FreezingProbability.ZERO);
            Check.That(freeze.FreezingProbabilityList[1]).IsEqualTo(FreezeForecast.FreezingProbability.ZERO);
            Check.That(freeze.FreezingProbabilityList[2]).IsEqualTo(FreezeForecast.FreezingProbability.HIGH);
            Check.That(freeze.FreezingProbabilityList[3]).IsEqualTo(FreezeForecast.FreezingProbability.IMMINENT);
            Check.That(freeze.FreezingEnd.HasValue).IsEqualTo(true);
        }

        private class DeviceTest : IWeather
        {
            public double Pressure { get; set ; }
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
