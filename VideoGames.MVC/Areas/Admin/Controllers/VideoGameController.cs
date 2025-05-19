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

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var game = await _videoGameService.GetAsync(id);
            if (game == null)
                return NotFound();

            var model = new VideoGameUpdateModel
            {
                Id = game.Id,
                Name = game.Name,
                Description = game.Description,
                Price = game.Price,
                HasAgeLimit = game.HasAgeLimit,
                CategoryIds = game.Categories.Select(c => c.Id).ToList()
            };

            ViewBag.ImageUrl = game.ImageUrl; // Opsiyonel: Mevcut görseli göstermek için
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(VideoGameUpdateModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await _videoGameService.UpdateAsync(model);
            return RedirectToAction("Index");
        }
    }
}
