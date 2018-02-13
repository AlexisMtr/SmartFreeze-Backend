using System;

namespace SmartFreeze.Dtos
{
    public class FreezeDto
    {
        public string DeviceId { get; set; }
        public DateTime Date { get; set; }
        public int Morning { get; set; }
        public int Afternoon { get; set; }
    }
}
