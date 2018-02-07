using Newtonsoft.Json;
using SmartFreezeFA.Models;
using System;
using System.Collections.Generic;

namespace SmartFreezeFA.Parsers
{
    public static class FrameParser
    {
        public static IEnumerable<Telemetry> Parse(string frame)
        {
            List<Telemetry> telemetries = new List<Telemetry>();

            dynamic dynamicFrame = JsonConvert.DeserializeObject<dynamic>(frame);
            dynamic datas = dynamicFrame.decoded.data;

            foreach(var item in datas)
            {
                telemetries.Add(new Telemetry
                {
                    Id = $"{dynamicFrame.DevEUI.Value}-{DateTime.UtcNow.ToString("yyyymmddhhmm")}",
                    DeviceId = dynamicFrame.DevEUI.Value,
                    OccuredAt = DateTime.UtcNow.AddSeconds((int)(item.ts.Value / 1_000)),
                    BatteryVoltage = item.battery.Value,
                    Humidity = item.humidity.Value,
                    Pressure = item.pressure.Value,
                    Temperature = item.temperature.Value
                });
            }

            return telemetries;
        }
    }
}
