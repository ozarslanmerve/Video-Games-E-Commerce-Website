
using System.Text.Json;
using VideoGames.MVC.Abstract;
using VideoGames.MVC.Models;

namespace VideoGames.MVC.Services
{
    public class AuthService : BaseService, IAuthService
    {
        public AuthService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor) : base(httpClientFactory, httpContextAccessor)
        {
        }

        public async Task<ResponseModel<TokenModel>> LoginAsync(LoginModel loginModel)
        {
            var client = GetHttpClient();
            var response = await client.PostAsJsonAsync("Authorization/login", loginModel);
            var responseBody = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ResponseModel<TokenModel>>(responseBody, _jsonSerializerOptions);
            return result;
        }

        public async Task<ResponseModel<string>> RegisterAsync(RegisterModel registerModel)
        {
            var client = GetHttpClient();
            var response = await client.PostAsJsonAsync("Authorization/register", registerModel);
            var responseBody = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ResponseModel<string>>(responseBody, _jsonSerializerOptions);
            return result;
        }
    }
}
