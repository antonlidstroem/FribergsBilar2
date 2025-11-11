namespace MarcusRent.Models
{
    public class MeResponseClient
    {
        public string Email { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = new();
    }
}
