using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoGames.Shared.DTOs
{
    public class CartItemDTO
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public int VideoGameId { get; set; }
        public VideoGameDTO VideoGame { get; set; }
        public int Quantity { get; set; }
    }
}   
