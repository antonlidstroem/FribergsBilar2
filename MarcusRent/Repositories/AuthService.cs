namespace MarcusRent.Repositories
{
    using FribergsApi.Models;
    using Microsoft.AspNetCore.Http;

    public class AuthService : IAuthService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly HttpClient _httpClient;

        public AuthService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
            _httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:7251/") };
        }

        public async Task<bool> LoginAsync(string email, string password)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", new { Email = email, Password = password });
            if (!response.IsSuccessStatusCode)
                return false;

            var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
            var session = _contextAccessor.HttpContext.Session;

            session.SetString("jwtToken", result.Token);
            session.SetString("userEmail", result.Email);
            session.SetString("userId", result.UserId);

            return true;
        }

        public void Logout()
        {
            var session = _contextAccessor.HttpContext.Session;
            session.Remove("jwtToken");
            session.Remove("userEmail");
            session.Remove("userId");
        }

        public string GetJwtTokenFromSession()
        {
            return _contextAccessor.HttpContext?.Session?.GetString("jwtToken");
        }

        
    }

}