using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VideoGames.MVC.Abstract;
using VideoGames.MVC.Models;

namespace VideoGames.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class CDKeyController : Controller
    {
        private readonly IVideoGameCDkeyService _cdKeyService;

        public CDKeyController(IVideoGameCDkeyService cdKeyService)
        {
            _cdKeyService = cdKeyService;
        }

        public async Task<IActionResult> Index(int videoGameId)
        {
            var keys = await _cdKeyService.GetCDKeysByGameIdAsync(videoGameId);
            ViewBag.VideoGameId = videoGameId;
            return View(keys);
        }

        public IActionResult Add(int videoGameId)
        {
            var model = new VideoGameCDkeyModel { VideoGameId = videoGameId };
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var key = await _cdKeyService.GetAsync(id);
            if (key == null)
            {
                return NotFound();
            }

            ViewBag.VideoGameId = key.VideoGameId;
            return View(key);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(VideoGameCDkeyModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await _cdKeyService.UpdateCDKeyAsync(model);
            return RedirectToAction("Index", new { videoGameId = model.VideoGameId });
        }



    }
}
