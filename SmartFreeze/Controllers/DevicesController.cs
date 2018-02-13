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
        private readonly AlarmService alarmService;

        public DevicesController(DeviceService deviceService, TelemetryService telemetryService, AlarmService alarmService)
        {
            this.deviceService = deviceService;
            this.telemetryService = telemetryService;
            this.alarmService = alarmService;
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PaginatedItemsDto<DeviceOverviewDto>))]
        public async Task<IActionResult> Get([FromQuery]DeviceFilter filter, [FromQuery]int rowsPerPage = 0, [FromQuery]int pageNumber = 1)
        {
            var devices = deviceService.GetAll(filter, rowsPerPage, pageNumber);
            return Ok(Mapper.Map<PaginatedItemsDto<DeviceOverviewDto>>(devices));
        }

        [HttpGet("{deviceId}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(DeviceDetailsDto))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Get(string deviceId)
        {
            var device = deviceService.Get(deviceId);
            if (device == null) return NotFound();

            return Ok(Mapper.Map<DeviceDetailsDto>(device));
        }

        [HttpGet("{deviceId}/telemetry")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PaginatedItemsDto<TelemetryDto>))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetTelemetry(string deviceId, [FromQuery]DateTime? from = null, [FromQuery]DateTime? to = null,
            [FromQuery]int rowsPerPage = 0, [FromQuery]int pageNumber = 1, [FromQuery]TemperatureUnit tempUnit = TemperatureUnit.Celsius)
        {
            var telemetry = telemetryService.GetByDevice(deviceId, from, to, rowsPerPage, pageNumber);
            if (telemetry == null) return NotFound();

            if(tempUnit == TemperatureUnit.Fahrenheit)
            {
                return Ok(Mapper.Map<PaginatedItemsDto<TelemetryFahrenheitDto>>(telemetry));
            }
            return Ok(Mapper.Map<PaginatedItemsDto<TelemetryDto>>(telemetry));
        }

        [HttpGet("{deviceId}/alarms")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PaginatedItemsDto<AlarmDetailsDto>))]
        public async Task<IActionResult> GetAlarms(string deviceId, [FromQuery]AlarmFilter filter, int rowsPerPage = 0, int pageNumber = 1)
        {
            PaginatedItems<Alarm> alarms = alarmService.GetByDevice(deviceId, filter, rowsPerPage, pageNumber);
            return Ok(Mapper.Map<PaginatedItemsDto<AlarmDetailsDto>>(alarms));
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<IActionResult> RegisterDevice([FromQuery] string idSite, [FromBody]DeviceRegistrationDto deviceRegistration)
        {

            Device device = Mapper.Map<Device>(deviceRegistration);
            Device newDevice = deviceService.Create(device, idSite);

            return Ok(Mapper.Map<DeviceRegistrationDto>(newDevice));
        }
        
        [HttpPut("{deviceId}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> UpdateDevice(string deviceId, [FromBody]DeviceRegistrationDto deviceRegistrationDto)
        {
            //TODO : Create DTO for update (with only allowed fields)
            Device device = Mapper.Map<Device>(deviceRegistrationDto);
            device.Id = deviceId;

            var isUpdated = deviceService.Update(device);

            if (isUpdated) return Ok();

            return NoContent();
        }

        [HttpDelete("{deviceId}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeleteDevice(string deviceId)
        {
            if (deviceService.Delete(deviceId)) return Ok();
            return NotFound();
        }

        [HttpPut("{deviceId}/favorite")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> ManageFavorite(string deviceId, [FromQuery]bool isFavorite)
        {
            deviceService.Managefavorite(deviceId, isFavorite);
            return Ok();
        }

        [HttpGet("{deviceId}/freeze")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(WeekFreezeDto))]
        public async Task<IActionResult> GetFreezeForecast(string deviceId)
        {
            return Ok();
        }
    }
}
