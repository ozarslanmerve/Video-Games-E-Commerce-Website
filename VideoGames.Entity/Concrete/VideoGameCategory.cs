using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoGames.Entity.Concrete
{
    public class VideoGameCategory
    {

        public int VideoGameId { get; set; }
        public VideoGame VideoGame { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
