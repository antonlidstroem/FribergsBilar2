namespace MarcusRent.Repositories
{
    using FribergsApi.Models;
    using Microsoft.AspNetCore.Http;

    public class AuthService : IAuthService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly HttpClient _httpClient;

        public AuthService(IHttpContextAccessor contextAccessor, HttpClient httpClient)
        {
            _contextAccessor = contextAccessor;
            _httpClient = httpClient;
        }

        public async Task<bool> LoginAsync(string email, string password)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/auth/login", new { Email = email, Password = password });
                if (!response.IsSuccessStatusCode)
                    return false;

                var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
                if (result == null || string.IsNullOrEmpty(result.Token))
                    return false;

                var session = _contextAccessor.HttpContext.Session;

                session.SetString("jwtToken", result.Token);
                session.SetString("userEmail", result.Email);
                session.SetString("userId", result.UserId);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"LoginAsync error: {ex.Message}");
                return false;
            }
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