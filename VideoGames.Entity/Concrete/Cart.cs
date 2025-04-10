using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoGames.Entity.Abstract;

namespace VideoGames.Entity.Concrete
{
    public class Cart: BaseEntity
    {
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public List<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
