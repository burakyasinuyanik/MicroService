using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;

namespace RabbitMQExample.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            var connectionFactory = new ConnectionFactory();
            connectionFactory.Uri = new Uri("");

            var connection=connectionFactory.CreateConnection();
            var channel = connection.CreateModel();
            channel.ConfirmSelect();// kuyru�a kesin kaydedilme do�rulamas�

            channel.QueueDeclare("Weather", true, false,false, null);
            //mandotory true se�ilirse consumer taraf�na da iletildi�i bilgisi gelir
            channel.BasicPublish("","",false,null,null);
            channel.WaitForConfirms();


            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
