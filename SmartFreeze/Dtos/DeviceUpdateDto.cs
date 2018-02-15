namespace SmartFreeze.Dtos
{
    public class DeviceUpdateDto
    {
        public string Name { get; set; }
        public bool IsFavorite { get; set; }
        public string Zone { get; set; }
        public string SiteId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
