using VideoGames.MVC.ComplexTypes;
using VideoGames.MVC.Models;

namespace VideoGames.MVC.Abstract
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderModel>> GetByUserAsync(string userId);    
        Task<OrderModel> GetOrderAsync(int id);
        Task<OrderModel> CreateOrderAsync(OrderCreateModel model);
        Task DeleteOrderAsync(int id);
        Task<IEnumerable<OrderModel>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, string? userId = null);

    }
}
