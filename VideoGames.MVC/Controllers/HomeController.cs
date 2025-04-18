using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using VideoGames.MVC.Abstract;
using VideoGames.MVC.Models;

namespace VideoGames.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IVideoGameService _videoGameService;

        public HomeController(ICategoryService categoryService, IVideoGameService videoGameService)
        {
            _categoryService = categoryService;
            _videoGameService = videoGameService;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetCategoriesAsync();
            var videoGames = await _videoGameService.GetAllAsync();
            HomeIndexModel model = new HomeIndexModel
            {
                VideoGames = videoGames,
                Categories = categories
            };
            return View(model);
        }
    }
}
