using SmartFreeze.Models;
using System;

namespace SmartFreeze.Dtos
{
    public class AlarmDetailsDto
    {
        public string Id { get; set; }
        public bool IsActive { get; set; }
        public DateTime OccuredAt { get; set; }
        public Alarm.Type Type { get; set; }
        public Alarm.Gravity Gravity { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
    }
}
