using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace SmartFreezeFA.Models
{
    [BsonIgnoreExtraElements]
    public class Site
    {
        [BsonId]
        private ObjectId ObjectId { get; set; }

        public string Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<Device> Devices { get; set; }
    }
}
