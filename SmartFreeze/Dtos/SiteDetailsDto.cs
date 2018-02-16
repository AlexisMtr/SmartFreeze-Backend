using SmartFreeze.Models;
using System.Collections.Generic;

namespace SmartFreeze.Dtos
{
    public class SiteDetailsDto : SiteOverviewDto
    {
        public IEnumerable<DeviceOverviewDto> Devices { get; set; }
        public double SurfaceArea { get; set; }
        public string SurfaceAreaUnit { get; set; }
        public ApplicationContext SiteType { get; set; }
        public IEnumerable<string> Zones { get; set; }
        public string Description { get; set; }
    }
}
