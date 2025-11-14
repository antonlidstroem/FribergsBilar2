using Microsoft.AspNetCore.Identity;

namespace DAL.Classes
{
    public class ApplicationUser : IdentityUser
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public string Email { get; set; }
        public bool ApprovedByAdmin { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
    }
}
