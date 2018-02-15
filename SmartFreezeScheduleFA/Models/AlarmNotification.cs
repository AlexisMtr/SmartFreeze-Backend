using MongoDB.Bson.Serialization.Attributes;

namespace SmartFreezeScheduleFA.Models
{
    [BsonNoId]
    public class AlarmNotification
    {
        public string SiteName { get; set; }
        public string SiteId { get; set; }
        public string DeviceName { get; set; }
        public string DeviceId { get; set; }
        public Alarm Alarm { get; set; }
    }
}
