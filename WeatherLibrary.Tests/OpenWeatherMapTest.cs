using Microsoft.VisualStudio.TestTools.UnitTesting;
using NFluent;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using WeatherLibrary.OpenWeatherMap;
using WeatherLibrary.Test.Mocks;

namespace WeatherLibrary.Test
{
    [TestClass]
    public class OpenWeatherMapTest
    {
        private OpenWeatherMapClient client;

        [TestInitialize]
        public void Setup()
        {
            var owmWeather = File.ReadAllText(Path.GetFullPath("OwmWeather.json"));
            var owmForecast = File.ReadAllText(Path.GetFullPath("OwmForecast.json"));

            var baseAddress = "http://api.openweathermap.org/data/2.5/";
            var dictionary = new Dictionary<string, string>
            {
                { $"{baseAddress}weather?", owmWeather },
                { $"{baseAddress}forecast?", owmForecast }
            };

            var httpClient = new HttpClient(new HttpClientMessageHandlerMock(dictionary))
            {
                BaseAddress = new Uri(baseAddress)
            };
            client = new OpenWeatherMapClient(httpClient, string.Empty);
        }

        [TestCleanup]
        public void TearDown()
        {
            if(client != null)
                client.Dispose();
        }

        [TestMethod]
        public void Owm_GetCurrentWeather()
        {
            var weather = client.GetCurrentWeather(45.1822, 5.7275).Result;
            
            Check.That(weather.Weather).IsNotNull()
                .And.IsInstanceOf<OwmWeather>();
            Check.That(weather.StationPosition).IsNotNull()
                .And.IsInstanceOf<OwmStationPosition>();
        }
    }
}
