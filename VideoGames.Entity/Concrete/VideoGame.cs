using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoGames.Entity.Abstract;

namespace VideoGames.Entity.Concrete
{
    public class VideoGame : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public DateTime ReleaseDate { get; set; } = DateTime.UtcNow;
        public bool HasAgeLimit { get; set; }
        public string ImageUrl { get; set; }
        public ICollection<VideoGameCategory> VideoGameCategories { get; set; } = new HashSet<VideoGameCategory>();
        public ICollection<VideoGameCDkey> CDkeys { get; set; } = new HashSet<VideoGameCDkey>(); // 💡 CDKey ilişkisi

    }

}
