using Key.Web.Models;
using Key.Web.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Diagnostics;

namespace Key.Web.Controllers
{
    public class HomeController( WeatherService weatherService) : Controller
    {
        

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

       

        [Authorize]
        public IActionResult Secured()
        {
            var claims = User.Claims;


            var token = HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken).Result;





            var refreshToken = HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken).Result;
            return View();
        }

        public IActionResult WeatherForecastPage()
        {
            var response = weatherService.GetWeatherForecast();
            return View();
        }
    }
}
