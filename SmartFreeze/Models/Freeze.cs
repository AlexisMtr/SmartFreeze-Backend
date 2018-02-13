using System;

namespace SmartFreeze.Models
{
    public class Freeze
    {
        public string DeviceId { get; set; }
        public DateTime Date { get; set; }
        public int TrustIndication { get; set; }
    }
}
