
namespace FribergsApi.Models
{
    public class ApplicationUserDto
    {
        public string Id { get; set; }             // användar-id
        public string UserName { get; set; }       // e-post eller användarnamn
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";
        public string Email { get; set; }          // e-post
        public bool ApprovedByAdmin { get; set; }  // om användaren är godkänd
    }
}


