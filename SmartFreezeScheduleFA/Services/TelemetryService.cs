using SmartFreezeScheduleFA.Models;
using SmartFreezeScheduleFA.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFreezeScheduleFA.Services
{
    public class TelemetryService
    {
        private readonly TelemetryRepository telemetryRepository;

        public TelemetryService(TelemetryRepository telemetryRepository)
        {
            this.telemetryRepository = telemetryRepository;
        }
    }
}
