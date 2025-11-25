using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Fribergs.Core.DTO
{
    public class UserDto : LoginUserDto
    {
      
        public string UserId { get; set; } = string.Empty;
   
        public List<string> Roles { get; set; } = new();
     
        [Required]
        public string FirstName { get; set; }
       
        [Required]
        public string LastName { get; set; }

        
        public bool ApprovedByAdmin { get; set; }

    }
}
