using Microsoft.AspNetCore.Mvc;
using VideoGames.Business.Abstract;
using VideoGames.Shared.DTOs;
using VideoGames.Shared.Helpers;

namespace VideoGames.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoGameCDkeysController : CustomControllerBase
    {
        private readonly IVideoGameService _videoGameService;

        public VideoGameCDkeysController(IVideoGameService videoGameService)
        {
            _videoGameService = videoGameService;
        }

        [HttpGet("{videoGameId}/cdkeys")]
        public async Task<IActionResult> GetCDkeysByGameId(int videoGameId)
        {
            var response = await _videoGameService.GetCDkeysByGameIdAsync(videoGameId);
            return CreateResponse(response);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] VideoGameCDkeyAddDTO dto)
        {
            var response = await _videoGameService.AddCDkeyAsync(dto);
            return CreateResponse(response);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] VideoGameCDkeyUpdateDTO dto)
        {
            var response = await _videoGameService.UpdateCDkeyAsync(dto);
            return CreateResponse(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _videoGameService.DeleteCDkeyAsync(id);
            return CreateResponse(response);
        }
    }

}
