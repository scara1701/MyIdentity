using System.ComponentModel.DataAnnotations;

namespace MyIdentity.API.Authentication.DTOModels
{
    public class DTOLogin
    {
        [Required(ErrorMessage = "User name is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
