using Microsoft.AspNetCore.Mvc;
using SmartFreeze.Dtos;
using SmartFreeze.Services;
using System.Net;
using System.Threading.Tasks;

namespace SmartFreeze.Controllers
{
    public class AlarmsController : Controller
    {
        private readonly AlarmService alarmService;

        public AlarmsController(AlarmService alarmService)
        {
            this.alarmService = alarmService;
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PaginatedItemsDto<AlarmDetailsDto>))]
        public async Task<IActionResult> Get(int rowsPerPage = 0, int pageNumber = 1)
        {
            return Ok();
        }
    }
}
