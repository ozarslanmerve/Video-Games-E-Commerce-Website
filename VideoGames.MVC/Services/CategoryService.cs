using System.Text.Json;
using VideoGames.MVC.Abstract;
using VideoGames.MVC.Models;

namespace VideoGames.MVC.Services
{
    public class CategoryService : BaseService, ICategoryService
    {
        public CategoryService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
            : base(httpClientFactory, httpContextAccessor)
        {
        }

        public async Task<IEnumerable<CategoryModel>> GetCategoriesAsync()
        {
            try
            {
                var client = GetHttpClient();
                var response = await client.GetAsync("categories");
                var jsonString = await response.Content.ReadAsStringAsync();

                ResponseModel<IEnumerable<CategoryModel>> result;

                try
                {
                    result = JsonSerializer.Deserialize<ResponseModel<IEnumerable<CategoryModel>>>(jsonString, _jsonSerializerOptions);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Json Deserialize Error: {ex.Message}");
                    return new List<CategoryModel>();
                }

                if (result != null && (result.Errors == null || result.Errors.Count == 0))
                {
                    return result.Data;
                }
                else
                {
                    Console.WriteLine($"Request Error: {result.Errors}");
                    return new List<CategoryModel>();
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Http Request Ex: {ex.Message}");
                return new List<CategoryModel>();
            }
        }
        public async Task<CategoryModel> GetCategoryAsync(int id)
        {
            var client = GetHttpClient();
            var response = await client.GetAsync($"categories/{id}");
            var json = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<ResponseModel<CategoryModel>>(json, _jsonSerializerOptions);
            return result?.Data!;
        }

        public async Task<int> GetCategoryCountAsync()
        {
            var client = GetHttpClient();
            var response = await client.GetAsync("categories/count");
            var json = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<ResponseModel<int>>(json, _jsonSerializerOptions);
            return result?.Data ?? 0;
        }

        public async Task<CategoryModel> AddCategoryAsync(CategoryModel model)
        {
            var client = GetHttpClient();
            var response = await client.PostAsJsonAsync("categories", model);
            var json = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<ResponseModel<CategoryModel>>(json, _jsonSerializerOptions);
            return result?.Data!;
        }

        public async Task UpdateCategoryAsync(CategoryModel model)
        {
            var client = GetHttpClient();
            await client.PutAsJsonAsync($"categories/{model.Id}", model);
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var client = GetHttpClient();
            await client.DeleteAsync($"categories/{id}");
        }
    }
}
