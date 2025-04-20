using System.Text.Json;
using VideoGames.MVC.Abstract;
using VideoGames.MVC.Models;

namespace VideoGames.MVC.Services
{
    public class VideoGameCDkeyService : BaseService, IVideoGameCDkeyService
    {
        public VideoGameCDkeyService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor) : base(httpClientFactory, httpContextAccessor)
        {
        }

        public async Task AddCDKeyAsync(VideoGameCDkeyModel model)
        {
            var client = GetHttpClient();
            var response = await client.PostAsJsonAsync("videogamecdkeys", model);
            var jsonString = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<ResponseModel<string>>(jsonString, _jsonSerializerOptions);
            if (result?.Errors != null && result.Errors.Count > 0)
            {
                throw new Exception(string.Join(", ", result.Errors));
            }
        }

        public async Task DeleteCDKeyAsync(int id)
        {
            var client = GetHttpClient();
            var response = await client.DeleteAsync($"videogamecdkeys/{id}");
            var jsonString = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<ResponseModel<string>>(jsonString, _jsonSerializerOptions);
            if (result?.Errors != null && result.Errors.Count > 0)
            {
                throw new Exception(string.Join(", ", result.Errors));
            }
        }
        

        public async Task<IEnumerable<VideoGameCDkeyModel>> GetCDKeysByGameIdAsync(int videoGameId)
        {
            var client = GetHttpClient();
            var response = await client.GetAsync($"videogamecdkeys/{videoGameId}");
            var jsonString = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<ResponseModel<IEnumerable<VideoGameCDkeyModel>>>(jsonString, _jsonSerializerOptions);
            return result?.Data ?? new List<VideoGameCDkeyModel>();
        }

        public async Task UpdateCDKeyAsync(VideoGameCDkeyModel model)
        {
            var client = GetHttpClient();
            var response = await client.PutAsJsonAsync("videogamecdkeys", model);
            var jsonString = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<ResponseModel<string>>(jsonString, _jsonSerializerOptions);
            if (result?.Errors != null && result.Errors.Count > 0)
            {
                throw new Exception(string.Join(", ", result.Errors));
            }
        }
    }
}
