using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SmartFreeze.Dtos;
using SmartFreeze.Filters;
using SmartFreeze.Models;
using SmartFreeze.Services;
using System.Net;
using System.Threading.Tasks;

namespace SmartFreeze.Controllers
{
    [Route("api/[controller]")]
    public class AlarmsController : Controller
    {
        private readonly AlarmService alarmService;

        public AlarmsController(AlarmService alarmService)
        {
            this.alarmService = alarmService;
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PaginatedItemsDto<AlarmDetailsDto>))]
        public async Task<IActionResult> Get([FromQuery]AlarmFilter filter, [FromQuery]int rowsPerPage = 0, [FromQuery]int pageNumber = 1)
        {
            PaginatedItems<Alarm> alarms = alarmService.GetAll(filter, rowsPerPage, pageNumber);
            return Ok(Mapper.Map<PaginatedItemsDto<AlarmDetailsDto>>(alarms));
        }

        [HttpPut("{alarmId}/ack")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotModified)]
        public async Task<IActionResult> Ack([FromRoute]string alarmId)
        {
            return alarmService.Ack(alarmId) ? Ok() : StatusCode((int)HttpStatusCode.NotModified);
        }
    }
}
