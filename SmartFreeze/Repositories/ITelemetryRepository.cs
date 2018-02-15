using SmartFreeze.Models;
using System;

namespace SmartFreeze.Repositories
{
    public interface ITelemetryRepository
    {
       PaginatedItems<Telemetry> GetByDevice(string deviceId, int rowsPerPage, int pageNumber, DateTime? from = null, DateTime? to = null);
    }
}
