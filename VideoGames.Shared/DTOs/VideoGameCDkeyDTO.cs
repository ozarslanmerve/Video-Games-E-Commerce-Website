using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoGames.Shared.DTOs
{
    public class VideoGameCDkeyDTO
    {
        public int? Id { get; set; }
        public string CDkey { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsUsed { get; set; }
    }
}
