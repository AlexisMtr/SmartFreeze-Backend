//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading.Tasks;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Moq;
//using NFluent;
//using WeatherLibrary.Abstraction;
//using WeatherLibrary.Algorithmes.Util;
//using WeatherLibrary.GoogleMapElevation;

//namespace WeatherLibrary.Tests.Util
//{
//    [TestClass]
//    public class UtilTemperatureTest
//    {
//        GoogleMapElevationClient client;

//        [TestInitialize]
//        public void Setup()
//        {
//            client = new GoogleMapElevationClient("AIzaSyCSpneEWisPNL0ZP7W6ayidLegkn-8MxaY");
//        }

//        [TestMethod]
//        public void GetCurrentWeatherTest()
//        {
//            //initialize
//            Mock<IStationPosition> siteStationPositionMock = new Mock<IStationPosition>();
//            siteStationPositionMock.Setup(e => e.Altitude).Returns(1000.0);
//            IStationPosition siteStationPostion = siteStationPositionMock.Object;

//            Mock<IWeather> currentWeatherMock = new Mock<IWeather>();
//            currentWeatherMock.Setup(e => e.Temperature).Returns(10.0);
//            IWeather currentWeather = currentWeatherMock.Object;

//            UtilTemperature temperature = new UtilTemperature(client, siteStationPostion, currentWeather);

//            // execute : elevation = 1600m 
//            Task<UtilTemperatureCurrent> currentTemperature = temperature.GetCurrentWeather(39.7391536, -104.9847034);

//            //tests
//            Check.That(currentTemperature.Result.Temperature).IsNotEqualTo(currentWeather.Temperature);
//            Console.WriteLine(currentTemperature.Result.Temperature);
//            Check.That(currentTemperature.Result.Temperature).IsCloseTo(currentTemperature.Result.Temperature, 6.04);


//        }

//        [TestMethod]
//        public void GetCurrentWeatherSameTemperatureTest()
//        {
//            //initialize
//            Mock<IStationPosition> siteStationPositionMock = new Mock<IStationPosition>();
//            siteStationPositionMock.Setup(e => e.Altitude).Returns(1550.0);
//            IStationPosition siteStationPostion = siteStationPositionMock.Object;

//            Mock<IWeather> currentWeatherMock = new Mock<IWeather>();
//            currentWeatherMock.Setup(e => e.Temperature).Returns(10.0);
//            IWeather currentWeather = currentWeatherMock.Object;

//            UtilTemperature temperature = new UtilTemperature(client, siteStationPostion, currentWeather);

//            // execute : elevation = 1600m 
//            Task<UtilTemperatureCurrent> currentTemperature = temperature.GetCurrentWeather(39.7391536, -104.9847034);

//            //tests
//            Check.That(currentTemperature.Result.Temperature).IsEqualTo(currentWeather.Temperature);

//        }

//        [TestMethod]
//        public void GetForecastWeatherTest()
//        {
//            //intialize
//            Mock<IStationPosition> siteStationPositionMock = new Mock<IStationPosition>();
//            siteStationPositionMock.Setup(e => e.Altitude).Returns(1000.0);
//            IStationPosition siteStationPostion = siteStationPositionMock.Object;

//            Mock<IWeather> currentWeatherMock = new Mock<IWeather>();
//            currentWeatherMock.Setup(e => e.Temperature).Returns(10.0);
//            IWeather currentWeather = currentWeatherMock.Object;

//            Mock<IWeather> tomorrowWeatherMock = new Mock<IWeather>();
//            tomorrowWeatherMock.Setup(e => e.Temperature).Returns(15.3);
//            IWeather tomorrowWeather = tomorrowWeatherMock.Object;

//            Mock<IWeather> afterTomorrowWeatherMock = new Mock<IWeather>();
//            afterTomorrowWeatherMock.Setup(e => e.Temperature).Returns(-2.0);
//            IWeather afterTomorrowWeather = afterTomorrowWeatherMock.Object;


//            IEnumerable<IWeather> weatherList = new List<IWeather> { currentWeather, tomorrowWeather, afterTomorrowWeather };
//            UtilTemperature temperature = new UtilTemperature(client, siteStationPostion, weatherList);

//            //Execute
//            Task<UtilTemperatureForecast> forecastTemperature = temperature.GetForecastWeather(39.7391536, -104.9847034);

//            //Tests
//            Check.That(forecastTemperature.Result.TemperatureList[0]).IsNotEqualTo(currentWeather.Temperature);
//            Check.That(forecastTemperature.Result.TemperatureList[0]).IsCloseTo(forecastTemperature.Result.TemperatureList[0], 6.04);

//            Check.That(forecastTemperature.Result.TemperatureList[1]).IsNotEqualTo(currentWeather.Temperature);
//            Check.That(forecastTemperature.Result.TemperatureList[1]).IsCloseTo(forecastTemperature.Result.TemperatureList[1], 11.4);

//            Check.That(forecastTemperature.Result.TemperatureList[2]).IsNotEqualTo(currentWeather.Temperature);
//            Console.WriteLine(forecastTemperature.Result.TemperatureList[2]);
//            Check.That(Math.Round(forecastTemperature.Result.TemperatureList[2], 2)).IsEqualTo(-5.96);
//        }
//    }
//}
