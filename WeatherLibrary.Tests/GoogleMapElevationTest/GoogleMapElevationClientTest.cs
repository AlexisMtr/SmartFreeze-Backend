using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NFluent;
using WeatherLibrary.GoogleMapElevation;
using WeatherLibrary.GoogleMapElevation.Internals;

namespace WeatherLibrary.Tests.GoogleMapElevationTest
{
    [TestClass]
    public class GoogleMapElevationClientTest
    {
        GoogleMapElevationClient gmec;

        [TestInitialize]
        public void Setup()
        {
            gmec = new GoogleMapElevationClient("AIzaSyCSpneEWisPNL0ZP7W6ayidLegkn-8MxaY");
        }

        [TestMethod]
        public void GetAltitudeTest()
        {
            Task<List<GMEAltitude>> altitudeList = gmec.GetAltitude(39.7391536, -104.9847034);

            Check.That(altitudeList.Result[0].Location.Latitude).IsEqualTo(39.7391536);
            Check.That(altitudeList.Result[0].Location.Longitude).IsEqualTo(-104.9847034);
            Check.That(altitudeList.Result[0].Elevation).IsCloseTo(1608.63793945313, altitudeList.Result[0].Elevation);

            gmec.Dispose();

        }
    }
}
