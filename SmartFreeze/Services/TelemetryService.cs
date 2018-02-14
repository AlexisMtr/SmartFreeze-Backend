using SmartFreeze.Models;
using SmartFreeze.Repositories;
using System;

namespace SmartFreeze.Services
{
    public class TelemetryService
    {
        private readonly ITelemetryRepository telemetryRepository;

        public TelemetryService(ITelemetryRepository telemetryRepository)
        {
            this.telemetryRepository = telemetryRepository;
        }

        public PaginatedItems<Telemetry> GetByDevice(string deviceId, DateTime? from, DateTime? to, int rowsPerPage, int pageNumber)
        {
            return telemetryRepository.GetByDevice(deviceId, rowsPerPage, pageNumber, from, to);
        }
    }
}
