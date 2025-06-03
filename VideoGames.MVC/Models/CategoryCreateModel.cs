using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace VideoGames.MVC.Models
{
    public class CategoryCreateModel
    {

        [JsonProperty("name")]
        [Required]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
