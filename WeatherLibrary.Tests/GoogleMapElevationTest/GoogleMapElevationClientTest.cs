using Microsoft.VisualStudio.TestTools.UnitTesting;
using NFluent;
using System.Configuration;
using WeatherLibrary.Abstraction;
using WeatherLibrary.GoogleMapElevation;

namespace WeatherLibrary.Tests.GoogleMapElevationTest
{
    [TestClass]
    public class GoogleMapElevationClientTest
    {
        [TestMethod]
        public void GetAltitudeTest()
        {
            string key = ConfigurationManager.AppSettings["GmeAPIKey"];
            GoogleMapElevationClient gmec = new GoogleMapElevationClient(key);
            IStationPosition position = gmec.GetAltitude(39.7391536, -104.9847034).Result;

            Check.That(position.Latitude).IsEqualTo(39.7391536);
            Check.That(position.Longitude).IsEqualTo(-104.9847034);
            Check.That(position.Altitude).IsCloseTo(1608.63793945313, 0.2);

            gmec.Dispose();

        }
    }
}
