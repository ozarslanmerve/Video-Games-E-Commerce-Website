using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace VideoGames.MVC.Models
{
    public class LoginModel
    {
        [JsonProperty("email")]
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email boş bırakılamaz")]
        [EmailAddress(ErrorMessage = "Geçersiz email adresi")]
        public string Email { get; set; }


        [JsonProperty("password")]
        [Display(Name = "Şifre")]
        [Required(ErrorMessage = "Şifre boş bırakılamaz")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
