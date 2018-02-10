using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace SmartFreeze.Controllers
{
    [Route("api")]
    public class InfosController : Controller
    {
        [HttpGet("ping")]
        public async Task<IActionResult> Ping()
        {
            return Ok("pong");
        }
    }
}
