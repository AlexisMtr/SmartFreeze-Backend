using AutoMapper;
using SmartFreeze.Dtos;
using SmartFreeze.Extensions;
using SmartFreeze.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartFreeze.Profiles
{
    public class FreezeProfile : Profile
    {
        public FreezeProfile()
        {
        }

        public static WeekFreezeDto Merge(IEnumerable<Freeze> freezeList)
        {
            WeekFreezeDto item = new WeekFreezeDto
            {
                Forecast = new List<DayFreezeDto>()
            };

            foreach (var freeze in freezeList)
            {
                bool exist = item.Forecast.Any(e => e.Date.IsSameDay(freeze.Date));
                if(exist)
                {
                    if(freeze.Date.Hour < 12)
                    {
                        item.Forecast.First(e => e.Date.IsSameDay(freeze.Date)).Morning = new FreezeDto
                        {
                            DeviceId = freeze.DeviceId,
                            TrustIndication = freeze.TrustIndication
                        };
                    }
                    else
                    {
                        item.Forecast.First(e => e.Date.IsSameDay(freeze.Date)).Afternoon = new FreezeDto
                        {
                            DeviceId = freeze.DeviceId,
                            TrustIndication = freeze.TrustIndication
                        };
                    }
                }
                else
                {
                    DayFreezeDto day = new DayFreezeDto { Date = new DateTime(freeze.Date.Year, freeze.Date.Month, freeze.Date.Day, 0, 0, 0) };
                    if (freeze.Date.Hour <= 12)
                    {
                        day.Morning = new FreezeDto
                        {
                            DeviceId = freeze.DeviceId,
                            TrustIndication = freeze.TrustIndication
                        };
                    }
                    else
                    {
                        day.Afternoon = new FreezeDto
                        {
                            DeviceId = freeze.DeviceId,
                            TrustIndication = freeze.TrustIndication
                        };
                    }
                    item.Forecast.Add(day);
                }
            }

            return item;
        }
    }
}
