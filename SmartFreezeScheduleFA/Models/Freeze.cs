using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SmartFreezeScheduleFA.Models
{
    public class Freeze
    {
        public Freeze()
        {
            this._id = ObjectId.GenerateNewId();
        }

        [BsonId]
        private ObjectId _id { get; set; }

        public string DeviceId { get; set; }
        [BsonDateTimeOptions(Representation = BsonType.DateTime)]
        public DateTime Date { get; set; }
        public int TrustIndication { get; set; }
    }
}
