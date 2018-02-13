using System.Collections.Generic;

namespace SmartFreeze.Dtos
{
    public class WeekFreezeDto
    {
        public string Id { get; set; }
        public IEnumerable<FreezeDto> Forecast { get; set; }
    }
}
