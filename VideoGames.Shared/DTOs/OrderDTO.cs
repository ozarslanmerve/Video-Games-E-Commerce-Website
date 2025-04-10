using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoGames.Shared.ComplexTypes;

namespace VideoGames.Shared.DTOs
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUserDTO ApplicationUser { get; set; }
        public List<OrderItemDTO> OrderItems { get; set; } = new List<OrderItemDTO>();
        public OrderStatus OrderStatus { get; set; }
        public decimal TotalAmount => OrderItems.Sum(item => item.UnitPrice * item.Quantity);
    }
}
