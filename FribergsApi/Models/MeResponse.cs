namespace FribergsApi.Models
{
    public class MeResponse
    {
        public string  Email { get; set; }
        public List<string> Roles { get; set; } = new();
    }
}
