using Newtonsoft.Json;

namespace VideoGames.MVC.Models
{
    public class VideoGameCDkeyModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }            

        [JsonProperty("videoGameId")]
        public int VideoGameId { get; set; }   

        [JsonProperty("cdkey")]
        public string CDkey { get; set; }     

        [JsonProperty("isUsed")]
        public bool IsUsed { get; set; }       

        [JsonProperty("isDeleted")]
        public bool IsDeleted { get; set; }    
    }
}
