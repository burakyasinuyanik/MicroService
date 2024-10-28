using KeyApplication1.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KeyApplication1.Controllers
{
    public class AutController(AutService autService): Controller
    {
        public async Task<IActionResult> Index()
        {
            var userName = "burak";
            var password = "Password12*";
            await autService.SignIn(userName, password);
            return View();
        }
        //role bazlı yetkilendirme
        [Authorize(Roles ="admin")]
        public async Task<IActionResult> Admin()
        {
            var userName=User.Claims.First(c=>c.Type==ClaimTypes.Name);
            var userName2 = User.Identity.Name;
            
            return View();
        }

        //claim bazlı yetkilendirme
        
        [Authorize(Policy ="city")]
        public IActionResult getCity()
        {
            return View();
        }

    }
}
