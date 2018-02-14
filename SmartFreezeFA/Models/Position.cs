using MongoDB.Bson.Serialization.Attributes;

namespace SmartFreezeFA.Models
{
    [BsonNoId]
    public class Position
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Altitude { get; set; }
    }
}
