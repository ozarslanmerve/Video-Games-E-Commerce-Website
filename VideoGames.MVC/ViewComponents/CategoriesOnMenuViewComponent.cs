using Microsoft.AspNetCore.Mvc;
using VideoGames.MVC.Abstract;

namespace VideoGames.MVC.ViewComponents
{
    public class CategoriesOnMenuViewComponent : ViewComponent
    {
        private readonly ICategoryService _categoryService;

        public CategoriesOnMenuViewComponent(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _categoryService.GetCategoriesAsync();
            return View(categories);
        }
    }
}
