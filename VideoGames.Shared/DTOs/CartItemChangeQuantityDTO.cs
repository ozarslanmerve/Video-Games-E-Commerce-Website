using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoGames.Shared.DTOs
{
    public class CartItemChangeQuantityDTO
    {
        public int CartItemId { get; set; }
        public int Quantity { get; set; }
    }
}
