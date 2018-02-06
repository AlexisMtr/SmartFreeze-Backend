using System;
using System.Collections.Generic;

namespace SmartFreezeScheduleFA.Models
{
    public class Device
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsFavorite { get; set; }
        public string Zone { get; set; }
        public IEnumerable<Alarm> Alarms { get; set; }
        public DateTime LastCommunication { get; set; }
    }
}
