using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VideoGames.MVC.Abstract;
using VideoGames.MVC.Models;

namespace VideoGames.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "AdminUser")]
    public class VideoGameController : Controller
    {
        private readonly IVideoGameService _videoGameService;

        public VideoGameController(IVideoGameService videoGameService)
        {
            _videoGameService = videoGameService;
        }

        public async Task<IActionResult> Index()
        {
            var games = await _videoGameService.GetAllAsync();
            return View(games);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(VideoGameCreateModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await _videoGameService.AddAsync(model);
            return RedirectToAction("Create");
        }
    }
}
