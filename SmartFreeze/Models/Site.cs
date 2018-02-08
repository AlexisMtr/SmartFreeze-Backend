using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace SmartFreeze.Models
{
    [BsonIgnoreExtraElements]
    public class Site
    {
        [BsonId]
        private ObjectId ObjectId { get; set; }

        public string Id { get; set; }
        public string Name { get; set; }
        public Position Position { get; set; }
        public IEnumerable<Device> Devices { get; set; }
        public double SurfaceArea { get; set; }
        public string SurfaceAreaUnit { get; set; }
        public ApplicationContext SiteType { get; set; }
        public IEnumerable<string> Zones { get; set; }
        public string Department { get; set; }
        public string Region { get; set; }
        public string Description { get; set; }
        public string ImageUri { get; set; }
    }
}
