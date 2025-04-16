using Newtonsoft.Json;

namespace VideoGames.MVC.Models
{
    public class TokenModel
    {
        [JsonProperty("accessToken")]
        public string AccessToken { get; set; }

        [JsonProperty("expirationDate")]
        public DateTime ExpirationDate { get; set; }
    }

}
