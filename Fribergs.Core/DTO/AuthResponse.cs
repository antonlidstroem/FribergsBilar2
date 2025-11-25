namespace Fribergs.Core.DTO

{
    public class AuthResponse
    {
        public string UserId { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; } = new();
    }
}
