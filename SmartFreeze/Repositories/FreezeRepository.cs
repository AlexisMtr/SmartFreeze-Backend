using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using SmartFreeze.Context;
using SmartFreeze.Models;

namespace SmartFreeze.Repositories
{
    public class FreezeRepository : IFreezeRepository
    {
        private readonly IMongoCollection<Freeze> collection;

        public FreezeRepository(SmartFreezeContext context)
        {
            this.collection = context.Database
                .GetCollection<Freeze>(nameof(Freeze));
        }

        public IEnumerable<Freeze> GetByDevice(string deviceId, DateTime? from = null)
        {
            return collection.AsQueryable()
                .Where(e => e.DeviceId == deviceId);
        }

        public Dictionary<string, IEnumerable<Freeze>> GetByDevice(IEnumerable<string> devicesId, DateTime? from = null)
        {
            return collection.AsQueryable()
                .Where(e => devicesId.Contains(e.DeviceId))
                .GroupBy(k => k.DeviceId, v => v)
                .ToDictionary(k => k.Key, v => v.AsEnumerable());
        }

        public Dictionary<string, IEnumerable<Freeze>> GetByDevice(DateTime? from = null)
        {
            return collection.AsQueryable()
                .GroupBy(k => k.DeviceId, v => v)
                .ToDictionary(k => k.Key, v => v.AsEnumerable());
        }
    }
}
