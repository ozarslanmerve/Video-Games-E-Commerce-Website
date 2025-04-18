using Microsoft.AspNetCore.Mvc;

namespace VideoGames.MVC.ViewComponents
{

    public class NavbarViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}