using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VideoGames.Business.Abstract;
using VideoGames.Shared.DTOs;
using VideoGames.Shared.Helpers;

namespace VideoGames.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoGamesController : CustomControllerBase
    {
        private readonly IVideoGameService _videoGameService;

        public VideoGamesController(IVideoGameService videoGameService)
        {
            _videoGameService = videoGameService;
        }



        [HttpPost("add")]
        public async Task<IActionResult> CreateVideoGame(VideoGameCreateDTO videoGameCreateDTO)
        {
            var response = await _videoGameService.AddAsync(videoGameCreateDTO);
            return CreateResponse(response);
        }



        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _videoGameService.GetAllAsync();
            return CreateResponse(response);
        }

        [HttpGet("alldetailed")]
        public async Task<IActionResult> GetAllDetailed()
        {
            var response = await _videoGameService.GetAllDetailedAsync();
            return CreateResponse(response);
        }



        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _videoGameService.GetAsync(id);
            return CreateResponse(response);
        }

        [HttpGet("admin/{id}")]
        public async Task<IActionResult> GetDetailsForAdmin(int id)
        {
            var response = await _videoGameService.GetWithCategoriesAsync(id);
            return CreateResponse(response);
        }

        [HttpGet("bycategory/{categoryId}")]
        public async Task<IActionResult> GetByCategory(int categoryId)
        {
            var response = await _videoGameService.GetByCategoryAsync(categoryId);
            return CreateResponse(response);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromForm] VideoGameUpdateDTO videoGameUpdateDTO)
        {
            var response = await _videoGameService.UpdateAsync(videoGameUpdateDTO);
            return CreateResponse(response);
        }

        [HttpGet("count")]
        public async Task<IActionResult> GetCount()
        {
            var response = await _videoGameService.GetCountAsync();
            return CreateResponse(response);
        }

        [HttpGet("countbycategory/{categoryId}")]
        public async Task<IActionResult> GetCountByCategory(int categoryId)
        {
            var response = await _videoGameService.GetCountByCategoryAsync(categoryId);
            return CreateResponse(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _videoGameService.DeleteAsync(id);
            return CreateResponse(response);
        }



    }
}
