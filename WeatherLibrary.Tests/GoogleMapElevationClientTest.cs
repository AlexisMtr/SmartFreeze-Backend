using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NFluent;
using WeatherLibrary.Abstraction;
using WeatherLibrary.GoogleMapElevation;

namespace WeatherLibrary.Tests
{
    [TestClass]
    public class GoogleMapElevationClientTest
    {
        [TestMethod]
        public void GetAltitudeTest()
        {
            // need to add appsettings.json file with the following format:
            // { "GmeApiKey": "<YOUR_APIKEY>" }

            var key = Settings().GetSection("GmeApiKey").Value;
            using (var client = new GoogleMapElevationClient(key))
            {
                IStationPosition position = client.GetAltitude(39.7391536, -104.9847034).Result;

                Check.That(position.Latitude).IsEqualTo(39.7391536);
                Check.That(position.Longitude).IsEqualTo(-104.9847034);
                Check.That(position.Altitude).IsCloseTo(1608.63793945313, 0.2);
            }
        }

        private IConfigurationRoot Settings()
        {
            return new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
        }
    }
}
