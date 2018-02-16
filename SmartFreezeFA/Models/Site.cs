using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace SmartFreezeFA.Models
{
    [BsonIgnoreExtraElements]
    public class Site
    {
        public Site()
        {
            this._id = ObjectId.GenerateNewId();
        }

        [BsonId]
        private ObjectId _id { get; set; }

        public string Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<Device> Devices { get; set; }
    }
}
