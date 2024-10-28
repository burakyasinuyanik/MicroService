using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Key.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataProtectionController(IDataProtectionProvider dataProtectionProvider) : ControllerBase
    {
        public async Task<IActionResult> Get()
        {
            var dataProtection = dataProtectionProvider.CreateProtector("salting");


            var dataProtectionTime = dataProtection.ToTimeLimitedDataProtector();

            dataProtectionTime.Protect("Hello world", TimeSpan.FromSeconds(30));


            var protectedData = dataProtection.Protect("Hello World!");

            var unprotectedData = dataProtection.Unprotect(protectedData);

            return Ok();
        }
    }
}
