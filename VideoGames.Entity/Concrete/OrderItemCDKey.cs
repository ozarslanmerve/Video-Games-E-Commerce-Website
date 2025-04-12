using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoGames.Entity.Abstract;

namespace VideoGames.Entity.Concrete
{
    public class OrderItemCDKey:BaseEntity
    {
        public int OrderItemId { get; set; }
        public OrderItem OrderItem { get; set; }

        public int VideoGameCDkeyId { get; set; }
        public VideoGameCDkey VideoGameCDkey { get; set; }
    }
}
