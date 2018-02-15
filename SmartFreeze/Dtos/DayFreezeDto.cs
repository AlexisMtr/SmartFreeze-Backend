using System;

namespace SmartFreeze.Dtos
{
    public class DayFreezeDto
    {
        public DateTime Date { get; set; }
        public FreezeDto Morning { get; set; }
        public FreezeDto Afternoon { get; set; }
    }
}
