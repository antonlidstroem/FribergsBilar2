using System.ComponentModel.DataAnnotations;

namespace MarcusRent.Models
{
    public class LoginUserDtoClient
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
