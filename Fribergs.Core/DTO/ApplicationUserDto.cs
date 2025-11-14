
namespace Fribergs.Core.DTO
{
    public class ApplicationUserDto
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


