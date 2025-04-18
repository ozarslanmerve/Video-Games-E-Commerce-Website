using System.Text.Json.Serialization;

namespace VideoGames.MVC.Models
{
    public class CartItemModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("cartId")]
        public int CartId { get; set; }

        [JsonPropertyName("videoGameId")]
        public int VideoGameId { get; set; }

        [JsonPropertyName("videoGame")]
        public VideoGameModel VideoGame { get; set; }

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }
    }
}
