using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFreezeScheduleFA.Models
{
    public class Site
    {
        public IEnumerable<Device> Devices { get; set; }
    }
}
