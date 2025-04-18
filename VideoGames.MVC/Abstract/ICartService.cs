using VideoGames.MVC.Models;

namespace VideoGames.MVC.Abstract
{
    public interface ICartService
    {
        Task<CartModel> GetCartAsync(string applicationUserId);
        Task AddToCartAsync(CartItemModel cartItemModel);
        Task RemoveFromCartAsync(int cartItemId);
        Task<string> ClearCartAsync(string applicationUserId);
        Task<string> ChangeQuantityAsync(int cartItemId, int quantity);
        Task<bool> CreateCartAsync(CartModel cartModel);
    }
}
