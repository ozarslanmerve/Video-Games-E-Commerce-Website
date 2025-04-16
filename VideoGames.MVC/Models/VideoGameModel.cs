using Newtonsoft.Json;

namespace VideoGames.MVC.Models
{
    public class VideoGameModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }

        [JsonProperty("imageUrl")]
        public string ImageUrl { get; set; }

        [JsonProperty("categories")]
        public IEnumerable<CategoryModel> Categories { get; set; }
    }
}
