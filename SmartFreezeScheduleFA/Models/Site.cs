using System.Collections.Generic;

namespace SmartFreezeScheduleFA.Models
{
    public class Site
    {
        public string Id { get; set; }
        public IEnumerable<Device> Devices { get; set; }
    }
}
