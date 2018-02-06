using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using SmartFreeze.Dtos;
using SmartFreeze.Filters;
using SmartFreeze.Models;
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
        public async Task<IActionResult> Get([FromQuery]SiteFilter filter, int rowsPerPage = 20, int pageNumber = 1)
        {
            var sites = siteService.Get(filter, rowsPerPage, pageNumber);
            return Ok(Mapper.Map<PaginatedItemsDto<SiteOverviewDto>>(sites));
        }

        [HttpGet("{siteId}")]
        public async Task<IActionResult> Get(string siteId)
        {
            return Ok();
        }


        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<IActionResult> RegisterSite([FromQuery]ApplicationContext context, [FromBody]SiteRegistration siteRegistration)
        {
            ObjectId newId = ObjectId.GenerateNewId();

            Site site = Mapper.Map<SiteRegistration, Site>(siteRegistration);
            site.Id = newId.ToString();


            var isCreated =  siteService.Create(site);

            if (isCreated) return  Ok();
            
            return NoContent();
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateSite([FromQuery]ApplicationContext context, [FromBody]SiteRegistration siteRegistration)
        {
            Site site = Mapper.Map<SiteRegistration, Site>(siteRegistration);

            var isUpdated = siteService.Update(site);

            if (isUpdated) return Ok();

            return NoContent();
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateSite([FromQuery]ApplicationContext context, [FromBody]SiteRegistration siteRegistration)
        {

            var isUpdated = siteService.Delete(siteRegistration.)

            if (isUpdated) return Ok();

            return NoContent();
        }
    }
}
