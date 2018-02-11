using System;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Newtonsoft.Json;
using WeatherLibrary.Abstraction;
using WeatherLibrary.GoogleMapElevation.Internals;

namespace WeatherLibrary.GoogleMapElevation
{
    public class GoogleMapElevationClient : IDisposable, IAltitudeClient
    {
        private readonly HttpClient client;
        private readonly string apiKey;

        public GoogleMapElevationClient(string apiKey)
        {
            this.client = new HttpClient { BaseAddress = new Uri("https://maps.googleapis.com/maps/api/elevation/") };
            this.apiKey = apiKey;
        }

        public async Task<IStationPosition> GetAltitude(double latitude, double longitude)
        {
            string lat = latitude.ToString(CultureInfo.CreateSpecificCulture("en-GB"));
            string lng = longitude.ToString(CultureInfo.CreateSpecificCulture("en-GB"));

            var response = await this.client.GetAsync($"json?locations={lat},{lng}&key={this.apiKey}");
            var jsonRoot = JsonConvert.DeserializeObject<GMERoot>(await response.Content.ReadAsStringAsync());

            return Mapper.Map<GmeElevation>(jsonRoot);
        }


        public void Dispose()
        {
            if (this.client != null)
            {
                this.client.Dispose();
            }
        }
    }
}
