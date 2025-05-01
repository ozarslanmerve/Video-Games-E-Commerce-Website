using System.Text.Json;
using VideoGames.MVC.Abstract;
using VideoGames.MVC.ComplexTypes;
using VideoGames.MVC.Models;

namespace VideoGames.MVC.Services
{
    public class OrderService:BaseService, IOrderService
    {
        public OrderService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor) : base(httpClientFactory, httpContextAccessor)
        {
        }
        public async Task DeleteOrderAsync(int id)
        {
            var client = GetHttpClient();
            await client.DeleteAsync($"orders/{id}");
        }
        public async Task<OrderModel> GetOrderAsync(int id)
        {
            var client = GetHttpClient();
            var response = await client.GetAsync($"orders/{id}");
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ResponseModel<OrderModel>>(json, _jsonSerializerOptions);
            return result?.Data!;
        }
        public async Task<IEnumerable<OrderModel>> GetByUserAsync(string userId)
        {
            var client = GetHttpClient();
            var response = await client.GetAsync($"orders/getbyuserId/{userId}");
            var jsonString = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ResponseModel<IEnumerable<OrderModel>>>(jsonString, _jsonSerializerOptions);
            if (result?.Errors != null)
            {
                Console.WriteLine(string.Join(",", result.Errors));
                return null;
            }
            return result.Data;
        }
        public async Task<OrderModel> CreateOrderAsync(OrderCreateModel model)
        {
            var client = GetHttpClient();
            var response = await client.PostAsJsonAsync("orders", model);
            var json = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("🚨 API Hatası: " + json);
                return null;
            }

            try
            {
                var result = JsonSerializer.Deserialize<ResponseModel<OrderModel>>(json, _jsonSerializerOptions);
                return result?.Data!;
            }
            catch (Exception ex)
            {
                Console.WriteLine("⚠️ JSON parse hatası: " + ex.Message);
                Console.WriteLine("Gelen içerik: " + json);
                return null;
            }


        }


        public async Task<IEnumerable<OrderModel>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, string? userId = null)
        {
            var client = GetHttpClient();
            var response = await client.GetAsync($"orders/daterange?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}");
            var jsonString = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ResponseModel<IEnumerable<OrderModel>>>(jsonString, _jsonSerializerOptions);
            if (result?.Errors != null)
            {
                Console.WriteLine(string.Join(",", result.Errors));
                return null;
            }
            var orders = result.Data;
            IEnumerable<OrderModel> ordersOfUser = orders.Where(x => x.ApplicationUserId == userId).ToList();
            return ordersOfUser;
        }

    }
}
