using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoGames.Shared.DTOs.Auth
{
    public class TokenDTO
    {
        public string AccessToken { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
