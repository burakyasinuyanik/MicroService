using Docker.API.Service;
using Microsoft.AspNetCore.Mvc;

namespace Docker.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class WeatherForecastController (StockService stockService): ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetStock()
        {
            var result = await stockService.GetStock();
            return Ok(result);
        }
    }
}
