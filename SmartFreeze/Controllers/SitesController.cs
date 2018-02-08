﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SmartFreeze.Dtos;
using SmartFreeze.Filters;
using SmartFreeze.Models;
using SmartFreeze.Services;
using System.Linq;
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
        public async Task<IActionResult> RegisterSite([FromQuery]ApplicationContext context, [FromBody]SiteRegistrationDto siteRegistration)
        {
  
            Site site = Mapper.Map<Site>(siteRegistration);
            site.SiteType = context;
            site.Devices = Enumerable.Empty<Device>();
            if(site.Zones == null)
            {
                site.Zones = Enumerable.Empty<string>();
            }
            Site newSite = siteService.Create(site);

            return Ok(Mapper.Map<SiteOverviewDto>(newSite));
        }

        [HttpPut("{siteId}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> UpdateSite(string siteId, [FromBody]SiteUpdateDto siteUpdateDto)
        {
            Site site = Mapper.Map<Site>(siteUpdateDto);

            var isUpdated = siteService.Update(siteId, site);

            if (isUpdated) return Ok();

            return NoContent();
        }

        [HttpDelete("{siteId}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeleteSite(string siteId)
        {
            if(siteService.Delete(siteId)) return Ok();
            return NotFound();
        }

    }
}
