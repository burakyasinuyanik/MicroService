using Docker.API.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Docker.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StockController(StockService stockService) : ControllerBase
    {

        [HttpGet]
        public async Task<IActionResult> GetStock()
        {
            var result = await stockService.GetStock();
            return Ok(result);
        }
    }
}
