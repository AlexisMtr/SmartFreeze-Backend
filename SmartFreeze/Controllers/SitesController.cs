using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SmartFreeze.Dtos;
using SmartFreeze.Filters;
using SmartFreeze.Services;
using System.Net;
using System.Threading.Tasks;

namespace SmartFreeze.Controllers
{
    [Route("api/[controller]")]
    public class SitesController : Controller
    {
        private readonly SiteService siteService;

        public SitesController(SiteService siteService)
        {
            this.siteService = siteService;
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PaginatedItemsDto<SiteOverviewDto>))]
        public async Task<IActionResult> Get([FromQuery]SiteFilter filter, int rowsPerPage = 0, int pageNumber = 1)
        {
            var sites = siteService.Get(filter, rowsPerPage, pageNumber);
            return Ok(Mapper.Map<PaginatedItemsDto<SiteOverviewDto>>(sites));
        }

        [HttpGet("{siteId}")]
        public async Task<IActionResult> Get(string siteId)
        {
            return Ok();
        }
    }
}
