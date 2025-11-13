namespace MarcusRent.Models
{
    public class UserDtoClient
    {
        public string UserId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = new();
        public string FullName { get; set; } = string.Empty;
        public bool ApprovedByAdmin { get; set; }
    }
}
