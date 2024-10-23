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
            throw new Exception("db hatası");
            return Ok(new { count = 100 });


        }
    }
}
