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
        public async Task<IActionResult> Get([FromQuery]SiteFilter filter, int rowsPerPage = 0, int pageNumber = 1)
        {
            var sites = siteService.Get(filter, rowsPerPage, pageNumber);
            return Ok(Mapper.Map<PaginatedItemsDto<SiteOverviewDto>>(sites));
        }

        [HttpGet("{siteId}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(SiteDetailsDto))]
        public async Task<IActionResult> Get(string siteId)
        {
            var site = siteService.Get(siteId);
            return Ok(Mapper.Map<SiteDetailsDto>(site));
        }


        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> RegisterSite([FromQuery]ApplicationContext context, [FromBody]SiteRegistration siteRegistration)
        {
            ObjectId newId = ObjectId.GenerateNewId();

            Site site = Mapper.Map<Site>(siteRegistration);
            site.Id = newId.ToString();
            Site newSite = siteService.Create(site);

            return Ok(Mapper.Map<SiteOverviewDto>(newSite));
        }

        [HttpPut("{siteId}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> UpdateSite(string siteId, [FromBody]SiteRegistration siteRegistration)
        {
            //TODO : Create DTO for update (with only allowed fields)
            Site site = Mapper.Map<SiteRegistration, Site>(siteRegistration);

            var isUpdated = siteService.Update(siteId, site);

            if (isUpdated) return Ok();

            return NoContent();
        }

        [HttpDelete("{siteId}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> UpdateSite(string siteId)
        {
            if(siteService.Delete(siteId)) return Ok();
            return NotFound();
        }
    }
}
