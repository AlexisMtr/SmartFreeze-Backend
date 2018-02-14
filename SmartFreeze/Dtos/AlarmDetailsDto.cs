using SmartFreeze.Models;
using System;

namespace SmartFreeze.Dtos
{
    public class AlarmDetailsDto
    {
        public string Id { get; set; }
        public bool IsActive { get; set; }
        public string DeviceId { get; set; }
        public string SiteId { get; set; }
        public DateTime OccuredAt { get; set; }
        public int Type { get; set; }
        public int Gravity { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
    }
}
