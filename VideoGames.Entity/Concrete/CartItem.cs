using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoGames.Entity.Abstract;

namespace VideoGames.Entity.Concrete
{
    public class CartItem:BaseEntity

    {
        public int CartId { get; set; }
        public Cart Cart { get; set; }
        public int VideoGameId { get; set; }
        public VideoGame VideoGame { get; set; }
        public int Quantity { get; set; } = 1;
    }
}

