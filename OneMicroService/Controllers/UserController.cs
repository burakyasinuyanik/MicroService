using Bus.Shared;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OneMicroService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IPublishEndpoint publishEndpoint) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateUser()
        {
            //User to create

            await publishEndpoint.Publish(new UserCreatedEvent(Guid.NewGuid(),"burak@burak.com","555"));
            return Ok();
        }
    }
}
