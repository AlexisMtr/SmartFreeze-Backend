namespace SmartFreeze.Dtos
{
    public class DeviceRegistrationDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsFavorite { get; set; }
        public string Zone { get; set; }
        public string SiteId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Altitude { get; set; }
    }
}
