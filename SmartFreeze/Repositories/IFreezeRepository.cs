using SmartFreeze.Models;
using System;
using System.Collections.Generic;

namespace SmartFreeze.Repositories
{
    public interface IFreezeRepository
    {
        IEnumerable<Freeze> GetByDevice(string deviceId, DateTime? from = null);
        Dictionary<string, IEnumerable<Freeze>> GetByDevice(IEnumerable<string> devicesId, DateTime? from = null)
        Dictionary<string, IEnumerable<Freeze>> GetByDevice(DateTime? from = null);
    }
}
