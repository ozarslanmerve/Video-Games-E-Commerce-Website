using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using VideoGames.MVC.Abstract;

namespace VideoGames.MVC.Controllers
{
    public class VideoGamesController : Controller
    {
        private readonly IVideoGameService _videoGameService;

        public VideoGamesController(IVideoGameService videoGameService)
        {
            _videoGameService = videoGameService;
        }

        public async Task<IActionResult> Index()
        {

            var videoGames = await _videoGameService.GetAllAsync();

            return View(videoGames);
        }
        public async Task<IActionResult> GetAllByCategory(int id, string category)
        {

            var videoGames = await _videoGameService.GetAllByCategoryAsync(id);
            ViewData["CategoryName"] = category;
            return View(videoGames);

         
        }


        public async Task<IActionResult> Details(int id)
        {
            var game = await _videoGameService.GetAsync(id);
            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }


    }
}
