namespace SmartFreeze.Models
{
    public class Alarm
    {
        public enum Type
        {
            FreezeWarning,
            DeviceFailure
        }

        public enum Gravity
        {
            All = 0,
            Critical = 1,
            Serious = 2,
            Information = 3
        }

        public string Id { get; set; }
        public string DeviceId { get; set; }
        public string SiteId { get; set; }
        public bool IsActive { get; set; }
        public Type AlarmType { get; set; }
        public Gravity AlarmGravity { get; set; }
    }
}
