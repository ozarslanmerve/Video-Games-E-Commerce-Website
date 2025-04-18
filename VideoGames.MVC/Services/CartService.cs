using System.Text.Json;
using VideoGames.MVC.Abstract;
using VideoGames.MVC.Models;

namespace VideoGames.MVC.Services
{
    public class CartService : BaseService, ICartService
    {
        public CartService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor) : base(httpClientFactory, httpContextAccessor)
        {
        }

        public async Task AddToCartAsync(CartItemModel cartItemModel)
        {
            try
            {
                var client = GetHttpClient();
                var response = await client.PostAsJsonAsync("Carts/addtocard", cartItemModel);
                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException("API hata verdi");
                }
                var jsonString = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<ResponseModel<CartItemModel>>(jsonString, _jsonSerializerOptions);
                if (result?.Errors != null || result?.Errors?.Count > 0)
                {
                    throw new Exception($"Hata var: {string.Join(",", result.Errors)}");
                }

                if (result.Data == null)
                {
                    throw new Exception("Veri gelmedi");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata: {ex.Message}");
                throw;
            }
        }
        

        public async Task<string> ChangeQuantityAsync(int cartItemId, int quantity)
        {
            var client = GetHttpClient();
            var response = await client.PutAsJsonAsync("carts", new CartItemModel
            {
                Id = cartItemId, 
                Quantity = quantity
            });
            var jsonString = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ResponseModel<string>>(jsonString, _jsonSerializerOptions);
            if (result.Errors != null && result.Errors.Count > 0)
            {
                Console.WriteLine(string.Join(",", result.Errors));
            }
            return result.Data;
        }


        public async Task<string> ClearCartAsync(string applicationUserId)
        {
            var client = GetHttpClient();
            var response = await client.GetAsync($"Carts/clear/{applicationUserId}");
            var jsonString = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ResponseModel<string>>(jsonString, _jsonSerializerOptions);
            if (result.Errors != null && result.Errors.Count > 0)
            {
                Console.WriteLine(string.Join(",", result.Errors));
            }
            return result.Data;
        }

        public Task<bool> CreateCartAsync(CartModel cartModel)
        {
            throw new NotImplementedException();
        }

        public async Task<CartModel> GetCartAsync(string applicationUserId)
        {
            try
            {
                var client = GetHttpClient();
                var request = new HttpRequestMessage(HttpMethod.Get, $"Carts/{applicationUserId}");
                var response = await client.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    return new CartModel();
                }

                var jsonString = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<ResponseModel<CartModel>>(jsonString, _jsonSerializerOptions);

                return result.Data;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new CartModel();
            }
        }

        public async Task RemoveFromCartAsync(int cartItemId)
        {
            var client = GetHttpClient();
            var response = await client.DeleteAsync($"Carts/removefromcart/{cartItemId}");
            var jsonString = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ResponseModel<bool>>(jsonString, _jsonSerializerOptions);

            if (result.Errors != null && result.Errors.Count > 0)
            {
                Console.WriteLine(string.Join(",", result.Errors));
            }
        }
    }
}
