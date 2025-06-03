using VideoGames.MVC.Models;

namespace VideoGames.MVC.Abstract
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryModel>> GetCategoriesAsync();
        Task<CategoryModel> GetCategoryAsync(int id);
        Task<int> GetCategoryCountAsync();
        Task<CategoryModel> AddCategoryAsync(CategoryCreateModel model);
        Task UpdateCategoryAsync(CategoryModel model);
        Task DeleteCategoryAsync(int id);
    }
}
