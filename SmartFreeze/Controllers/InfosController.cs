using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace SmartFreeze.Controllers
{
    [Route("api")]
    public class InfosController : Controller
    {
        [HttpGet("ping")]
        public async Task<IActionResult> Ping()
        {
            return Ok(new
            {
                Message = "pong",
                ServerDate = DateTime.UtcNow
            });
        }
    }
}
