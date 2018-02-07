using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using NFluent;
using SmartFreezeFA.Models;
using SmartFreezeFA.Parsers;
using System;
using System.Collections.Generic;

namespace SmartFreezeFA.Tests
{
    [TestClass]
    public class ParserTests
    {
        [TestMethod]
        public void Parse_ThrowArgumentException()
        {
            Check.ThatCode(() => FrameParser.Parse("")).Throws<ArgumentException>();
        }

        [TestMethod]
        public void Parse_ThrowFormatException()
        {
            Check.ThatCode(() => FrameParser.Parse("invalid frame")).Throws<FormatException>();
        }

        [TestMethod]
        public void Parse_ValidFrame()
        {
            string deviceId = string.Empty;

            object frame = new
            {
                DevEUI = deviceId,
                Data = "123123",
                Decoded = new
                {
                    data = new object[]
                    {
                        new
                        {
                            ts = 1512669900771,
                            temperature = 14.5,
                            humidity = 45,
                            pressure = 97300,
                            battery = 3.5
                        },
                        new
                        {
                            ts = 1512670900771,
                            temperature = 14.5,
                            humidity = 45,
                            pressure = 97300,
                            battery = 3.5
                        },
                        new
                        {
                            ts = 1512671900771,
                            temperature = 14.5,
                            humidity = 45,
                            pressure = 97300,
                            battery = 3.5
                        }
                    }
                }
            };
            
            IEnumerable<Telemetry> telemetries = FrameParser.Parse(JsonConvert.SerializeObject(frame));

            Check.That(telemetries).HasSize(3);
            Check.That(telemetries).ContainsOnlyElementsThatMatch(e => e.DeviceId == deviceId);
        }
    }
}
