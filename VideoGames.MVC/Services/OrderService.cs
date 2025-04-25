using System.Text.Json;
using VideoGames.MVC.Abstract;
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
        public async  Task<IEnumerable<OrderModel>> GetOrdersAsync()
        {
            var client = GetHttpClient();
            var response = await client.GetAsync("orders");
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ResponseModel<IEnumerable<OrderModel>>>(json, _jsonSerializerOptions);
            return result?.Data ?? new List<OrderModel>();
        }
        public async Task<OrderModel> CreateOrderAsync(OrderCreateModel model)
        {
            var client = GetHttpClient();
            var response = await client.PostAsJsonAsync("orders", model);
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ResponseModel<OrderModel>>(json, _jsonSerializerOptions);
            return result?.Data!;
        }   
    


    }
}
