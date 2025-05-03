using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace VideoGames.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "AdminUser")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        [AllowAnonymous]
        public IActionResult Login() // www.aaa.com/admin/login
        {
            return View();
        }

   

    }
}