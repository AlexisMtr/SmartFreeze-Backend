using MongoDB.Bson;
using SmartFreeze.Models;
using System;
using System.Collections.Generic;

namespace SmartFreeze.Filters
{
    public class DeviceAlarmFilter
    {
        public ApplicationContext Context { get; set; }
        public Alarm.Gravity Gravity { get; set; }
        public Alarm.Type AlarmType { get; set; }
        public string DeviceId { get; set; }

        public IList<BsonDocument> CountAlarmsPipeline()
        {
            IList<BsonDocument> pipeline = GetPipelineDocuments();
            BsonDocument countPipeline = new BsonDocument("$group", new BsonDocument
            {
                { "Count", new BsonDocument("$sum", 1) },
                { "_id", "null" }
            });
            pipeline.Add(countPipeline);
            return pipeline;
        }

        public IList<BsonDocument> SkipedAlarmsPipeline(int rowsPerPage, int pageNumber)
        {
            var skip = Math.Max(0, pageNumber - 1) * rowsPerPage;

            IList<BsonDocument> pipeline = GetPipelineDocuments();

            if (rowsPerPage == 0) return pipeline;

            BsonDocument skipPipeline = new BsonDocument("$skip", skip);
            BsonDocument limitPipeline = new BsonDocument("$limit", rowsPerPage);

            pipeline.Add(skipPipeline);
            pipeline.Add(limitPipeline);

            return pipeline;
        }


        private IList<BsonDocument> GetPipelineDocuments()
        {
            List<BsonDocument> pipeline = new List<BsonDocument>();

            BsonDocument contextStage = new BsonDocument("$match", new BsonDocument
            {
                { "SiteType", Context }
            });
            pipeline.Add(contextStage);

            BsonDocument unwindDevices = new BsonDocument("$unwind", "$Devices");
            pipeline.Add(unwindDevices);
            BsonDocument projectDevices = new BsonDocument("$project", new BsonDocument
            {
                { "Devices", "$Devices" },
                { "_id", 0 }
            });
            pipeline.Add(projectDevices);

            if (!string.IsNullOrEmpty(DeviceId) && !string.IsNullOrWhiteSpace(DeviceId))
            {
                BsonDocument matchDevice = new BsonDocument("$match", new BsonDocument
                {
                    { "Devices.Id", DeviceId }
                });

                pipeline.Add(matchDevice);
            }

            BsonDocument unwindAlarms = new BsonDocument("$unwind", "$Devices.Alarms");
            pipeline.Add(unwindAlarms);
            BsonDocument projectAlarms = new BsonDocument("$project", new BsonDocument
            {
                { "Alarms", "$Devices.Alarms" },
                { "_id", 0 }
            });
            pipeline.Add(projectAlarms);

            if(Gravity != Alarm.Gravity.All && AlarmType != Alarm.Type.All)
            {
                BsonDocument matchAlarms = new BsonDocument("$match", new BsonDocument
                {
                    { "Alarms.AlarmType", AlarmType },
                    { "Alarms.AlarmGravity", Gravity }
                });
                pipeline.Add(matchAlarms);
            }
            else if(Gravity != Alarm.Gravity.All)
            {
                BsonDocument matchAlarms = new BsonDocument("$match", new BsonDocument
                {
                    { "Alarms.AlarmGravity", Gravity }
                });
                pipeline.Add(matchAlarms);
            }
            else if(AlarmType != Alarm.Type.All)
            {
                BsonDocument matchAlarms = new BsonDocument("$match", new BsonDocument
                {
                    { "Alarms.AlarmType", AlarmType }
                });
                pipeline.Add(matchAlarms);
            }

            return pipeline;
        }
    }
}
