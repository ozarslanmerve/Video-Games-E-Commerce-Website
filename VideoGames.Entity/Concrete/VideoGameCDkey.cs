using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoGames.Entity.Abstract;

namespace VideoGames.Entity.Concrete
{
    public class VideoGameCDkey: BaseEntity
    {

        public string CDkey { get; set; }
        public bool IsUsed { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
        public int VideoGameId { get; set; }
        public VideoGame VideoGame { get; set; }

    }
}
