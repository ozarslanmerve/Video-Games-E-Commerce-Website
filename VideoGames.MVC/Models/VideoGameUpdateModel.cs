namespace VideoGames.MVC.Models
{
    public class VideoGameUpdateModel
    {
        public int Id { get; set; }  
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool HasAgeLimit { get; set; }

        public IFormFile? Image { get; set; } 
        public List<int> CategoryIds { get; set; } = new();
       
    }
}
