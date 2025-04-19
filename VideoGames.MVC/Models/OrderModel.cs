using Newtonsoft.Json;
using VideoGames.MVC.ComplexTypes;

namespace VideoGames.MVC.Models
{
    public class OrderModel
    {

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("applicationUserId")]
        public string ApplicationUserId { get; set; }

        [JsonProperty("orderItems")]
        public List<OrderItemModel> OrderItems { get; set; }

        [JsonProperty("orderStatus")]
        public OrderStatus OrderStatus { get; set; }

        [JsonProperty("createdDate")]
        public DateTime CreatedDate { get; set; }

        [JsonProperty("totalAmount")]
        public decimal TotalAmount { get; set; }
    }
}
