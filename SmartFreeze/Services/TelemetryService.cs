﻿using SmartFreeze.Models;
using SmartFreeze.Repositories;
using System;

namespace SmartFreeze.Services
{
    public class TelemetryService
    {
        private readonly TelemetryRepository telemetryRepository;

        public TelemetryService(TelemetryRepository telemetryRepository)
        {
            this.telemetryRepository = telemetryRepository;
        }

        public PaginatedItems<Telemetry> GetByDevice(string deviceId, DateTime? from, DateTime? to, int rowsPerPage, int pageNumber)
        {
            return telemetryRepository.GetByDevice(deviceId, rowsPerPage, pageNumber, from, to);
        }
    }
}