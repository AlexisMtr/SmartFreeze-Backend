using MongoDB.Driver.Linq;
using SmartFreeze.Models;
using System.Collections.Generic;
using System.Linq;

namespace SmartFreeze.Sorters
{
    public class DeviceSorter : IMongoSorter<Device>
    {
        private const string alarmCount = "AlarmCount";

        private static Dictionary<string, string> stringToPropertyName = new Dictionary<string, string>
        {
            { "deviceId", nameof(Device.Id) },
            { "alarmCount", alarmCount },
            { "date", nameof(Device.LastCommunication) }
        };
        
        public IEnumerable<string> Sort { get; set; }

        public IOrderedMongoQueryable<Device> SortSource(IMongoQueryable<Device> source)
        {
            IOrderedMongoQueryable<Device> orderedSource = source.OrderBy(e => e.Id);
            
            string property = string.Empty;
            bool alreadyOrdered = false;
            bool asc = true;

            foreach(var item in Sort)
            {
                property = stringToPropertyName.FirstOrDefault(e => e.Key == item || e.Key == item.Substring(1)).Value.Trim();
                asc = !item.StartsWith("-");

                if (alreadyOrdered)
                {
                    orderedSource = asc ? OrderThenAsc(orderedSource, property) : OrderThenDesc(orderedSource, property);
                }
                else
                {
                    orderedSource = asc ? OrderAsc(orderedSource, property) : OrderDesc(orderedSource, property);
                    alreadyOrdered = true;
                }
                
            }

            return orderedSource;
        }

        private IOrderedMongoQueryable<Device> OrderAsc(IOrderedMongoQueryable<Device> source, string property)
        {
            switch (property)
            {
                case nameof(Device.Id):
                    source = source.OrderBy(e => e.Id);
                    break;
                case nameof(Device.IsFavorite):
                    source = source.OrderBy(e => e.IsFavorite);
                    break;
                case nameof(Device.LastCommunication):
                    source = source.OrderBy(e => e.LastCommunication);
                    break;
                case alarmCount:
                    source = source.OrderBy(e => e.Alarms.Count());
                    break;
                default:
                    break;
            }

            return source;
        }

        private IOrderedMongoQueryable<Device> OrderDesc(IOrderedMongoQueryable<Device> source, string property)
        {
            switch (property)
            {
                case nameof(Device.Id):
                    source = source.OrderByDescending(e => e.Id);
                    break;
                case nameof(Device.IsFavorite):
                    source = source.OrderByDescending(e => e.IsFavorite);
                    break;
                case nameof(Device.LastCommunication):
                    source = source.OrderByDescending(e => e.LastCommunication);
                    break;
                case alarmCount:
                    source = source.OrderByDescending(e => e.Alarms.Count());
                    break;
                default:
                    break;
            }

            return source;
        }

        private IOrderedMongoQueryable<Device> OrderThenAsc(IOrderedMongoQueryable<Device> source, string property)
        {
            switch (property)
            {
                case nameof(Device.Id):
                    source = source.ThenBy(e => e.Id);
                    break;
                case nameof(Device.IsFavorite):
                    source = source.ThenBy(e => e.IsFavorite);
                    break;
                case nameof(Device.LastCommunication):
                    source = source.ThenBy(e => e.LastCommunication);
                    break;
                case alarmCount:
                    source = source.ThenBy(e => e.Alarms.Count());
                    break;
                default:
                    break;
            }

            return source;
        }

        private IOrderedMongoQueryable<Device> OrderThenDesc(IOrderedMongoQueryable<Device> source, string property)
        {
            switch (property)
            {
                case nameof(Device.Id):
                    source = source.ThenByDescending(e => e.Id);
                    break;
                case nameof(Device.IsFavorite):
                    source = source.ThenByDescending(e => e.IsFavorite);
                    break;
                case nameof(Device.LastCommunication):
                    source = source.ThenByDescending(e => e.LastCommunication);
                    break;
                case alarmCount:
                    source = source.ThenByDescending(e => e.Alarms.Count());
                    break;
                default:
                    break;
            }

            return source;
        }
    }
}
