namespace SmartFreeze.Dtos
{
    public class SiteOverviewDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool HasActiveAlarms { get; set; }
        public int ActiveAlarmsCount { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Region { get; set; }
        public string Department { get; set; }
    }
}
