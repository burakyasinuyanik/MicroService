using Microsoft.AspNetCore.Mvc;

namespace Docker2.Api.Controllers
{

    [ApiController]
    [Route("api/[controller]/[action]")]
    public class StockController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetStock()
        {
            return Ok(new { count = 100 });


        }
    }
}
