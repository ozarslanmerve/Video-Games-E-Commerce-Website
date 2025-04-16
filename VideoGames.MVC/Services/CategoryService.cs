using ECommerce.MVC.Abstract;
using System.Net.Http.Json;
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
            var client = GetHttpClient();
            var response = await client.GetAsync("categories");
            var json = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<ResponseModel<IEnumerable<CategoryModel>>>(json, _jsonSerializerOptions);
            return result?.Data ?? new List<CategoryModel>();
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
