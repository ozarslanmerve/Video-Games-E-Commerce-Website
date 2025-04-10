using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoGames.Shared.DTOs
{
    public class CartItemRemoveDTO
    {
        public int CartId { get; set; }
        public int VideoGameId { get; set; }
    }
}

