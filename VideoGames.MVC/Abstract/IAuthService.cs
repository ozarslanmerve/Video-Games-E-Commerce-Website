using VideoGames.MVC.Models;

namespace VideoGames.MVC.Abstract
{
    public interface IAuthService
    {
        Task<ResponseModel<string>> RegisterAsnyc(RegisterModel registerModel);

        Task<ResponseModel<TokenModel>> LoginAsync(LoginModel loginModel);
    }
}
