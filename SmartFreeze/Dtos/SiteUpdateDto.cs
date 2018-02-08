using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartFreeze.Dtos
{
    public class SiteUpdateDto
    {
        public string Name { get; set; }
        public bool IsFavorite { get; set; }
        public double SurfaceArea { get; set; }
        public IEnumerable<string> Zones { get; set; }
        public string Description { get; set; }
        public string ImageUri { get; set; }
    }
}
