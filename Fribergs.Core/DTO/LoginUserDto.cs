using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Fribergs.Core.DTO
{
    public class LoginUserDto
    {
        [Required]
        [EmailAddress]
     
        public string Email { get; set; }
       
   
        public string Password { get; set; }
    }
}
