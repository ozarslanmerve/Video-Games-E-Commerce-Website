using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoGames.Shared.DTOs
{
    public class VideoGameAdminDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Properties { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public IEnumerable<CategoryDTO> Categories { get; set; }
        public IEnumerable<VideoGameCDkeyDTO> CDkeys { get; set; }
    }
}
