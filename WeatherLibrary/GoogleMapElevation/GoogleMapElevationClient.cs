using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WeatherLibrary.GoogleMapElevation.DTOs;
using WeatherLibrary.GoogleMapElevation.Internals;

namespace WeatherLibrary.GoogleMapElevation
{
    public class GoogleMapElevationClient
    {
        private readonly HttpClient client;
        private readonly string apiKey;

        public GoogleMapElevationClient(string apiKey)
        {
            this.client = new HttpClient { BaseAddress = new Uri("https://maps.googleapis.com/maps/api/elevation/") };
            this.apiKey = apiKey;
        }

        public GoogleMapElevationClient()
        {
            this.client = new HttpClient { BaseAddress = new Uri("https://maps.googleapis.com/maps/api/elevation/") };
            this.apiKey = "AIzaSyCSpneEWisPNL0ZP7W6ayidLegkn-8MxaY";
        }

        public async Task<List<GMEAltitude>> GetAltitude(double latitude, double longitude)
        {
            var response = await this.client.GetAsync($"json?locations={latitude.ToString(CultureInfo.CreateSpecificCulture("en-GB"))},{longitude.ToString(CultureInfo.CreateSpecificCulture("en-GB"))}&key={this.apiKey}");
            var jsonRoot = JsonConvert.DeserializeObject<GMERoot>(await response.Content.ReadAsStringAsync());
            IEnumerable<GMEAltitude> altitudeList = new List<GMEAltitude>();
            return (jsonRoot.Results as List<GMEAltitude>);
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
