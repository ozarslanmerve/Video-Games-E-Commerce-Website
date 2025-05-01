using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoGames.Business.Helpers
{
    public static class CDKeyGenerator
    {
        public static string Generate()
        {
            var segments = new List<string>();
            for (int i = 0; i < 4; i++)
            {
                segments.Add(Guid.NewGuid().ToString("N").Substring(0, 5).ToUpper());
            }
            return string.Join("-", segments);
        }
    }

}
