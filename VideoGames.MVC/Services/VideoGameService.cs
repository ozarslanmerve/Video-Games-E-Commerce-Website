
using System.Text.Json;
using VideoGames.MVC.Abstract;
using VideoGames.MVC.Models;

namespace VideoGames.MVC.Services
{
    public class VideoGameService : BaseService, IVideoGameService
    {
        public VideoGameService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor) : base(httpClientFactory, httpContextAccessor)
        {
        }

        public async Task<VideoGameModel> AddAsync(VideoGameModel model)
        {
            throw new NotImplementedException();
        }

        public Task<int> CountAsync()
        {
            throw new NotImplementedException();
        }

        public Task<int> CountByCategoryAsync(int categoryId)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<VideoGameModel>> GetAllAsync()
        {
            try
            {
                var client = GetHttpClient();
                var response = await client.GetAsync("videoGames");
                var jsonString = await response.Content.ReadAsStringAsync();
                ResponseModel<IEnumerable<VideoGameModel>> result;
                try
                {
                    result = JsonSerializer.Deserialize<ResponseModel<IEnumerable<VideoGameModel>>>(jsonString, _jsonSerializerOptions);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Bir hata var");
                    return new List<VideoGameModel>();
                }
                if (result != null && (result.Errors == null || result.Errors.Count == 0))
                {
                    return result.Data;
                }
                else
                {
                    Console.WriteLine("Bir hata var");
                    return new List<VideoGameModel>();
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
                return new List<VideoGameModel>();
            }
        }

        public async Task<IEnumerable<VideoGameModel>> GetAllByCategoryAsync(int categoryId)
        {
            try
            {
                var client = GetHttpClient();
                var response = await client.GetAsync($"videoGames/bycategory/{categoryId}");
                var jsonString = await response.Content.ReadAsStringAsync();
                ResponseModel<IEnumerable<VideoGameModel>> result;
                try
                {
                    result = JsonSerializer.Deserialize<ResponseModel<IEnumerable<VideoGameModel>>>(jsonString, _jsonSerializerOptions);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return new List<VideoGameModel>();
                }
                if (result != null && (result.Errors == null || result.Errors.Count == 0))
                {
                    return result.Data;
                }
                else
                {
                    Console.WriteLine("Bir hata var");
                    return new List<VideoGameModel>();
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
                return new List<VideoGameModel>();
            }
        }

        public async  Task<VideoGameModel> GetAsync(int id)
        {
            try
            {
                var client = GetHttpClient();
                var response = await client.GetAsync($"videoGames/get/{id}");
                var jsonString = await response.Content.ReadAsStringAsync();
                ResponseModel<VideoGameModel> result;
                try
                {
                    result = JsonSerializer.Deserialize<ResponseModel<VideoGameModel>>(jsonString, _jsonSerializerOptions);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return new VideoGameModel();
                }
                if (result != null && (result.Errors == null || result.Errors.Count == 0))
                {
                    return result.Data;
                }
                else
                {
                    Console.WriteLine("Bir hata var");
                    return new VideoGameModel();
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
                return new VideoGameModel();
            }
        }

        public Task UpdateAsync(VideoGameModel model)
        {
            throw new NotImplementedException();
        }
    }
}
