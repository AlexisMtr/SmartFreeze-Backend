using Microsoft.AspNetCore.Mvc;
using SmartFreeze.Services;
using System.Threading.Tasks;

namespace SmartFreeze.Controllers
{
    public class SitesController : Controller
    {
        private readonly SiteService siteService;

        public SitesController(SiteService siteService)
        {
            this.siteService = siteService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok();
        }

        [HttpGet("{siteId}")]
        public async Task<IActionResult> Get(string siteId)
        {
            return Ok();
        }
    }
}
