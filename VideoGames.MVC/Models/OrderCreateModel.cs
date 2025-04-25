using Newtonsoft.Json;

namespace VideoGames.MVC.Models
{
    public class OrderCreateModel
    {
        [JsonProperty("applicationUserId")]
        public string ApplicationUserId { get; set; }

        [JsonProperty("orderItems")]
        public List<OrderItemCreateModel> OrderItems { get; set; } = new();
    }
}
