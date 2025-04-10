using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoGames.Shared.DTOs
{
    public class CartItemCreateDTO
    {
        public int CartId { get; set; }
        public int VideoGameId { get; set; }
        public int Quantity { get; set; }
    }
}
