using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoGames.Shared.DTOs
{
    public class CartDTO
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUserDTO ApplicationUser { get; set; }
        public IEnumerable<CartItemDTO> CartItems { get; set; } = new List<CartItemDTO>();
    }
}
