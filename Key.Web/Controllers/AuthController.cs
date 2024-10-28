using Key.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Key.Web.Controllers
{
    public class AuthController(AuthService authService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var username = "mehmet16";
            var password = "Password12*";

            var response = await authService.SignIn(username, password);


            return View();
        }

        //Role based Authorization
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AdminPage()
        {
            var username = User.Claims.First(x => x.Type == ClaimTypes.Name);

            var userName2 = User.Identity.Name;


            return View();
        }

        //Claim based Authorization

        [Authorize(Policy = "CityIstanbul")]
        public async Task<IActionResult> IstanbulPage()
        {
            return View();
        }
    }
}
