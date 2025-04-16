using Newtonsoft.Json;

namespace VideoGames.MVC.Models
{
    public class ResponseModel<T>
    {
        [JsonProperty("data")]
        public T? Data { get; set; }

        [JsonProperty("errors")]
        public List<string>? Errors { get; set; }
    }
}
