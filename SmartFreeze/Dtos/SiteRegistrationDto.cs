using SmartFreeze.Models;
using System.Collections.Generic;

namespace SmartFreeze.Dtos
{
    public class SiteRegistrationDto
    {
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Altitude { get; set; }
        public bool IsFavorite { get; set; }
        public double SurfaceArea { get; set; }
        public string SurfaceAreaUnit { get; set; }
        public IEnumerable<string> Zones { get; set; }
        public string Department { get; set; }
        public string Region { get; set; }
        public string Description { get; set; }
        public string ImageUri { get; set; }
    }
}
