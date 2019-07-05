using System.ComponentModel.DataAnnotations;

namespace Inventory.WebApp.Models
{
    // public class TokenRequest
    // {
    //     [Required]
    //     [JsonProperty("username")]
    //     public string Username { get; set; }


    //     [Required]
    //     [JsonProperty("password")]
    //     public string Password { get; set; }
    // }

    public class LoginViewModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }

}