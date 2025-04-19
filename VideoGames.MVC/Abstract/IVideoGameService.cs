using VideoGames.MVC.Models;

namespace VideoGames.MVC.Abstract
{
    public interface IVideoGameService
    {
        Task<IEnumerable<VideoGameModel>> GetAllAsync();
        Task<IEnumerable<VideoGameModel>> GetAllByCategoryAsync(int categoryId);
        Task<VideoGameModel> GetAsync(int id);
        Task<int> CountAsync();
        Task<int> CountByCategoryAsync(int categoryId);
        Task<VideoGameModel> AddAsync(VideoGameCreateModel model);
        Task UpdateAsync(VideoGameModel model);
        Task DeleteAsync(int id);
    }
}
