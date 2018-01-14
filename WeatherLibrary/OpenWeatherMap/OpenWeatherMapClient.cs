using AutoMapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using WeatherLibrary.Abstraction;
using WeatherLibrary.OpenWeatherMap.Internals;

namespace WeatherLibrary.OpenWeatherMap
{
    public class OpenWeatherMapClient : IDisposable, IWeatherClient<OwmCurrentWeather, OwmForecastWeather>
    {
        private readonly HttpClient client;
        private readonly string commonRequestParams;

        public OpenWeatherMapClient(string apiKey, string lang = "en", Unit unit = Unit.Default)
        {
            Configurations.MapperConfiguration.ConfigureMapper();

            this.client = new HttpClient { BaseAddress = new Uri("http://api.openweathermap.org/data/2.5/") };
            this.commonRequestParams = $"appid={apiKey}&units={unit}&lang={lang}";
        }

        public OpenWeatherMapClient(HttpClient client, string apiKey, string lang = "en", Unit unit = Unit.Default)
        {
            Configurations.MapperConfiguration.ConfigureMapper();

            this.client = client;
            this.commonRequestParams = $"appid={apiKey}&units={unit}&lang={lang}";
        }

        public async Task<OwmCurrentWeather> GetCurrentWeather(double latitude, double longitude)
        {
            var response = await this.client.GetAsync($"weather?lat={latitude}&lon={longitude}&{this.commonRequestParams}");
            var root = JsonConvert.DeserializeObject<OwmCurrentRoot>(await response.Content.ReadAsStringAsync());

            return new OwmCurrentWeather
            {
                Weather = Mapper.Map<OwmWeather>(root),
                StationPosition = Mapper.Map<OwmStationPosition>(root)
            };
        }

        public async Task<OwmForecastWeather> GetForecastWeather(double latitude, double longitude)
        {
            var response = await this.client.GetAsync($"forecast?lat={latitude}&lon={longitude}&{this.commonRequestParams}");
            var root = JsonConvert.DeserializeObject<OwmForecastRoot>(await response.Content.ReadAsStringAsync());

            return new OwmForecastWeather
            {
                StationPosition = Mapper.Map<OwmStationPosition>(root),
                Foracast = Mapper.Map<IEnumerable<OwmWeather>>(root.Forecast)
            };
        }

        public void Dispose()
        {
            if (this.client != null)
                this.client.Dispose();
        }
    }
}
