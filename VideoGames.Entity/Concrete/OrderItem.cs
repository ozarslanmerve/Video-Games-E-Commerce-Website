using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoGames.Entity.Abstract;

namespace VideoGames.Entity.Concrete
{
    public class OrderItem:BaseEntity
    {
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public int VideoGameId { get; set; }
        public VideoGame VideoGame { get; set; }

    
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }

        public ICollection<OrderItemCDKey> OrderItemCDKeys { get; set; } = new List<OrderItemCDKey>();



    }
}
