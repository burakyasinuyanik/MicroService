using Bus.Shared;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OneMicroService.API.Service;

namespace OneMicroService.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController(IPublishEndpoint publishEndpoint,StockService stockService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetStock() { 
        var result=await stockService.GetStock();
            return Ok(result);
                }

        [HttpPost]
        public async Task<IActionResult> CreateUser()
        {

            //outbox design
            //inbox design
            //transaction begin
            //User to create
            //outbox(created,message,sql server)
            //transaction end
            //retry=> count ya da timeout masstransit time out var sadece

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(60));

            await publishEndpoint.Publish(new UserCreatedEvent(Guid.NewGuid(), "burak@burak.com", "555"), 
                pipline =>
            {
                //ack gönderilme bilgisi açılma deffault
                pipline.SetAwaitAck(true);
                pipline.Durable=true;//kalıcı kaydetme
                pipline.TimeToLive=TimeSpan.FromSeconds(60);//kuyrukta bulunma süresi
            },cancellationTokenSource.Token);
            return Ok();
        }
    }
}
