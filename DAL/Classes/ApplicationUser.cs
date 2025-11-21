using Microsoft.AspNetCore.Identity;

namespace DAL.Classes
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";
    
        public bool ApprovedByAdmin { get; set; }
    }
}
