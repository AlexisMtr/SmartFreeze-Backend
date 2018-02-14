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
            if (string.IsNullOrEmpty(frame) || string.IsNullOrWhiteSpace(frame)) throw new ArgumentException("Empty string is not a valid argument", nameof(frame));

            try
            {
                List<Telemetry> telemetries = new List<Telemetry>();

                dynamic dynamicFrame = JsonConvert.DeserializeObject<dynamic>(frame);
                dynamic datas = dynamicFrame.Decoded.data;

                foreach (var item in datas)
                {
                    telemetries.Add(new Telemetry
                    {
                        Id = $"{dynamicFrame.DevEUI.Value}-{DateTime.UtcNow.ToString("yyyyMMddHHmm")}",
                        DeviceId = dynamicFrame.DevEUI.Value,
                        OccuredAt = new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds((int)(item.ts.Value / 1_000)),
                        BatteryVoltage = item.battery.Value,
                        Humidity = item.humidity.Value,
                        Pressure = item.pressure.Value,
                        Temperature = item.temperature.Value
                    });
                }

                return telemetries;
            }
            catch(Exception e)
            {
                throw new FormatException("Failed to parse frame", e);
            }
        }
    }
}
