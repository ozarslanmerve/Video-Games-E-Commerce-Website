using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace VideoGames.MVC.Models
{
    public class RegisterModel
    {
        [JsonProperty("firstName")]
        [Display(Name = "Ad")]
        [Required(ErrorMessage = "Ad alanı boş bırakılamaz")]
        public string FirstName { get; set; }



        [JsonProperty("lastName")]
        [Display(Name = "Soyad")]
        [Required(ErrorMessage = "Soyad alanı boş bırakılamaz")]
        public string LastName { get; set; }



        [JsonProperty("address")]
        [Display(Name = "Adres")]
        [Required(ErrorMessage = "Adres alanı boş bırakılamaz")]
        public string Address { get; set; }



        [JsonProperty("city")]
        [Display(Name = "Şehir")]
        [Required(ErrorMessage = "Şehir alanı boş bırakılamaz")]
        public string City { get; set; }




        [JsonProperty("gender")]
        [Display(Name = "Cinsiyet")]
        [Required(ErrorMessage = "Cinsiyet alanı boş bırakılamaz")]
        public int Gender { get; set; }




        [JsonProperty("dateOfBirth")]
        [Display(Name = "Doğum Tarihi")]
        [Required(ErrorMessage = "Doğum Tarihi alanı boş bırakılamaz")]
        public string DateOfBirth { get; set; }




        [JsonProperty("email")]
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email alanı boş bırakılamaz")]
        [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz")]
        public string Email { get; set; }




        [JsonProperty("password")]
        [Display(Name = "Şifre")]
        [Required(ErrorMessage = "Şifre alanı boş bırakılamaz")]
        [DataType(DataType.Password)]
        public string Password { get; set; }



        [JsonProperty("confirmPassword")]
        [Display(Name = "Şifre Onay")]
        [Required(ErrorMessage = "Şifre Onay alanı boş bırakılamaz")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Şifreler uyuşmuyor")]
        public string ConfirmPassword { get; set; }
    }
}
