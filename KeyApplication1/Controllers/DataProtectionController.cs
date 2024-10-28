using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KeyApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataProtectionController(IDataProtectionProvider dataProtectionProvider) : ControllerBase
    {
        public async Task<IActionResult> Get()
        {
            var dataProtection = dataProtectionProvider.CreateProtector("salting");
            var dataProtionTime = dataProtection.ToTimeLimitedDataProtector();

           var protecdetData= dataProtionTime.Protect("hello world",TimeSpan.FromSeconds(30));

            var unprotectedData = dataProtection.Unprotect(protecdetData);
            return Ok();
        }
    }
}
