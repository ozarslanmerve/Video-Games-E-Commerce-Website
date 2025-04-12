using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoGames.Shared.DTOs
{
    public class OrderItemDTO
    {
        public int Id { get; set; }
        public int OrderId { get; set; }       
        public int VideoGameId { get; set; }
        public VideoGameDTO VideoGame { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public List<string> CDKeys { get; set; }

    }
}
