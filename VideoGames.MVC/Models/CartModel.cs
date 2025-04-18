using Newtonsoft.Json;

namespace VideoGames.MVC.Models
{
    public class CartModel
    {

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("applicationUserId")]
        public string ApplicationUserId { get; set; }

        [JsonProperty("cartItems")]
        public List<CartItemModel> CartItems { get; set; }
    }
}
