using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using TempEventHubProcessingFA.Models;

namespace TempEventHubProcessingFA.Parsers
{
    public static class FrameParser
    {
        public static IEnumerable<Telemetry> Parse(string frame)
        {
            if (string.IsNullOrEmpty(frame) || string.IsNullOrWhiteSpace(frame)) throw new ArgumentException("Empty string is not a valid argument", nameof(frame));

            try
            {
                List<Telemetry> telemetries = new List<Telemetry>();

                IoTMessage dynamicFrame = JsonConvert.DeserializeObject<IoTMessage>(frame);
                IEnumerable<DataObject> datas = dynamicFrame.Data;

                foreach (var item in datas)
                {
                    telemetries.Add(new Telemetry
                    {
                        Id = $"{dynamicFrame.DevEUI}-{item.Ts}",
                        DeviceId = dynamicFrame.DevEUI,
                        OccuredAt = new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds((int)(item.Ts / 1_000)),
                        BatteryVoltage = item.Battery,
                        Humidity = item.Humidity,
                        Pressure = item.Pressure,
                        Temperature = item.Temperature
                    });
                }

                return telemetries;
            }
            catch (Exception e)
            {
                throw new FormatException("Failed to parse frame", e);
            }
        }

        private class IoTMessage
        {
            public IEnumerable<DataObject> Data { get; set; }
            public string DevEUI { get; set; }
        }

        private class DataObject
        {
            public long Ts { get; set; }
            public double Temperature { get; set; }
            public int Motion { get; set; }
            public double Humidity { get; set; }
            public int Light { get; set; }
            public double Battery { get; set; }
            public double Pressure { get; set; }
        }

    }
}
