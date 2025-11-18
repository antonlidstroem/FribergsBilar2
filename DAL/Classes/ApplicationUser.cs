using Microsoft.AspNetCore.Identity;

namespace DAL.Classes
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";
        public string Email { get; set; }
        public bool ApprovedByAdmin { get; set; }
    }
}
