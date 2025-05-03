using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace VideoGames.MVC.Models
{
    public class VideoGameCreateModel
    {
        [JsonProperty("name")]
        [Required]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("price")]
        [Required]
        public decimal Price { get; set; }

        [JsonProperty("hasAgeLimit")]
        [Required]
        public bool HasAgeLimit { get; set; }

        public IFormFile Image { get; set; }

        [JsonProperty("categoryIds")]
        public List<int> CategoryIds { get; set; } = new List<int>();

        [JsonProperty("cdKeyCount")]
        public int CDKeyCount { get; set; }

        [JsonProperty("cdKeys")]
        public List<VideoGameCDkeyModel> CDKeys { get; set; } 
    }
}
