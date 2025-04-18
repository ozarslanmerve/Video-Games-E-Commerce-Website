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

        
    }
}
