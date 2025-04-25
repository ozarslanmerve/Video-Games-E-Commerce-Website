using Newtonsoft.Json;

namespace VideoGames.MVC.Models
{
    public class OrderItemCreateModel
    {
        [JsonProperty("videoGameId")]
        public int VideoGameId { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }
    }
}
