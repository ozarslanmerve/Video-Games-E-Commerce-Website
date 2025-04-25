using Microsoft.AspNetCore.Mvc;

namespace VideoGames.MVC.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
