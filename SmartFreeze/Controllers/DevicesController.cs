using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SmartFreeze.Dtos;
using SmartFreeze.Filters;
using SmartFreeze.Models;
using SmartFreeze.Services;
using System;
using System.Net;
using System.Threading.Tasks;

namespace SmartFreeze.Controllers
{
    [Route("api/[controller]")]
    public class DevicesController : Controller
    {
        private readonly DeviceService deviceService;
        private readonly TelemetryService telemetryService;

        public DevicesController(DeviceService deviceService, TelemetryService telemetryService)
        {
            this.deviceService = deviceService;
            this.telemetryService = telemetryService;
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PaginatedItemsDto<DeviceOverviewDto>))]
        public async Task<IActionResult> Get([FromQuery]DeviceFilter filter, [FromQuery]int rowsPerPage = 20, [FromQuery]int pageNumber = 1)
        {
            var devices = deviceService.GetAll(filter, rowsPerPage, pageNumber);
            return Ok(Mapper.Map<PaginatedItemsDto<DeviceOverviewDto>>(devices));
        }

        [HttpGet("{deviceId}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Get(string deviceId)
        {
            var device = deviceService.Get(deviceId);
            if (device == null) return NotFound();

            return Ok(device);
        }

        [HttpGet("{deviceId}/telemetry")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetTelemetry(string deviceId, [FromQuery]DateTime? from = null, [FromQuery]DateTime? to = null,
            [FromQuery]int rowsPerPage = 20, [FromQuery]int pageNumber = 1)
        {
            var telemetry = telemetryService.GetByDevice(deviceId, from, to, rowsPerPage, pageNumber);
            if (telemetry == null) return NotFound();

            return Ok(telemetry);
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<IActionResult> RegisterDevice([FromQuery]ApplicationContext context, [FromBody]DeviceRegistrationDto deviceRegistration)
        {
            return StatusCode((int)HttpStatusCode.NotImplemented);
        }
    }
}
