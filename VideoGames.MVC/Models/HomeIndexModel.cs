namespace VideoGames.MVC.Models
{
    public class HomeIndexModel
    {
        public IEnumerable<CategoryModel> Categories { get; set; }
        public IEnumerable<VideoGameModel> VideoGames { get; set; }
    }
}
