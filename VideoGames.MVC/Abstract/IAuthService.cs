using VideoGames.MVC.Models;

namespace VideoGames.MVC.Abstract
{
    public interface IAuthService
    {
        Task<ResponseModel<string>> RegisterAsync(RegisterModel registerModel);

        Task<ResponseModel<TokenModel>> LoginAsync(LoginModel loginModel);
    }
}
