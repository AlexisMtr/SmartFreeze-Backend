using System.Collections.Generic;

namespace SmartFreeze.Dtos
{
    public class DeviceDetailsDto : DeviceOverviewDto
    {
        public IEnumerable<AlarmDetailsDto> Alarms { get; set; }
    }
}
