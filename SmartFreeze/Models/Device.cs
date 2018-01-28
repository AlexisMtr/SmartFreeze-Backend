using System;
using System.Collections.Generic;

namespace SmartFreeze.Models
{
    public class Device
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsFavorite { get; set; }
        public IEnumerable<Alarm> Alarms { get; set; }
        public DateTime LastCommunication { get; set; }
    }
}
