using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFreezeScheduleFA.Models
{
    public class Freeze
    {
        public string DeviceId { get; set; }
        public DateTime Date { get; set; }
        public int TrustIndication { get; set; }
    }
}
