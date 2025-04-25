using VideoGames.MVC.Models;

namespace VideoGames.MVC.Abstract
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderModel>> GetOrdersAsync();
        Task<OrderModel> GetOrderAsync(int id);
        Task<OrderModel> CreateOrderAsync(OrderCreateModel model);
        Task DeleteOrderAsync(int id);
    }
}
