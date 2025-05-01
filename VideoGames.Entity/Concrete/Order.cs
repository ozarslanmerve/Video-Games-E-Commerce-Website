using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoGames.Entity.Abstract;
using VideoGames.Shared.ComplexTypes;

namespace VideoGames.Entity.Concrete
{
    public class Order: BaseEntity
    {
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
        public DateTime CreatedDate { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;

    }
}
