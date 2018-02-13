﻿using WeatherLibrary.Abstraction;

namespace WeatherLibrary.GoogleMapElevation
{
    public class GmeElevation : IStationPosition
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Altitude { get; set; }
    }
}
