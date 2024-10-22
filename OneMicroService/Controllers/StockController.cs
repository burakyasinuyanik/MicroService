using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OneMicroService.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetStock()
        {
            return Ok(new { count = 100 });


        }

    }
}
