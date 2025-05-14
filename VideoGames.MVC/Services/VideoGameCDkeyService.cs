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

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new Exception($"Silme başarısız: {response.StatusCode} - {errorMessage}");
            }

            var content = await response.Content.ReadAsStringAsync();
        
            if (string.IsNullOrWhiteSpace(content))
            {
                return; 
            }
            var result = JsonSerializer.Deserialize<ResponseModel<string>>(content, _jsonSerializerOptions);
            if (result?.Errors != null && result.Errors.Count > 0)
            {
                throw new Exception(string.Join(", ", result.Errors));
            }
        }


        public async Task<VideoGameCDkeyModel> GetAsync(int id)
        {
            var client = GetHttpClient();
            var response = await client.GetAsync($"videogamecdkeys/get/{id}");
            var jsonString = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ResponseModel<VideoGameCDkeyModel>>(jsonString, _jsonSerializerOptions);
            return result?.Data;
        }

        public async Task<IEnumerable<VideoGameCDkeyModel>> GetCDKeysByGameIdAsync(int videoGameId)
        {
            var client = GetHttpClient();
            var response = await client.GetAsync($"videogamecdkeys/bygame/{videoGameId}");

            var jsonString = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"API başarısız yanıt verdi: {response.StatusCode} - {jsonString}");
            }

            if (string.IsNullOrWhiteSpace(jsonString))
            {
                return new List<VideoGameCDkeyModel>();
            }

            try
            {
                var result = JsonSerializer.Deserialize<ResponseModel<IEnumerable<VideoGameCDkeyModel>>>(jsonString, _jsonSerializerOptions);
                return result?.Data ?? new List<VideoGameCDkeyModel>();
            }
            catch (JsonException ex)
            {
                throw new Exception("JSON parse hatası: " + ex.Message);
            }
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
