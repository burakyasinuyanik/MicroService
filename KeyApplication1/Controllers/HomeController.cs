using KeyApplication1.Models;
using KeyApplication1.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Diagnostics;

namespace KeyApplication1.Controllers
{
    public class HomeController(WeatherService weatherService) : Controller
    {


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> WeatherForecastPage()
        {
            var list = await weatherService.GetWeather();

            return View();
        }

        [Authorize]
        public async Task< IActionResult> Secured()
        {
            var claims = User.Claims;
            
            var accessToken =  HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken).Result;
            var idToken =  HttpContext.GetTokenAsync(OpenIdConnectParameterNames.IdToken).Result;
            
            return View();
        }
    }
}
