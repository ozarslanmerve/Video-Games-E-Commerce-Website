using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoGames.Shared.DTOs
{
    public class VideoGameCreateDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool HasAgeLimit { get; set; }
        public int[] CategoryIds { get; set; } = [];
        public int CDKeyCount { get; set; }
        public List<string> VideoGameCDkeys { get; set; } = new();
        public IFormFile Image { get; set; }
    }
}
