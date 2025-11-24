using MarcusRent.Interfaces;
using MarcusRent.Services.Base;
using Microsoft.AspNetCore.Http;


namespace MarcusRent.Repositories
{
    public class AuthService : IAuthService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IClient _client;

        public AuthService(IHttpContextAccessor contextAccessor, IClient client)
        {
            _contextAccessor = contextAccessor;
            _client = client;
        }

        public async Task<bool> LoginAsync(string email, string password)
        {
            try
            {
                var dto = new LoginUserDto { Email = email, Password = password };
                var response = await _client.LoginAsync(dto);

                _contextAccessor.HttpContext!.Session.SetString("jwtToken", response.Token);
                return true;
            }
            
                catch (ApiException ex)
            {
                Console.WriteLine("LOGIN ERROR:");
                Console.WriteLine(ex.StatusCode);
                Console.WriteLine(ex.Response);    // <-- viktig!
                throw;
           
            
            }
        }

        public string GetJwtToken()
        {
            return _contextAccessor.HttpContext?.Session?.GetString("jwtToken") ?? "";
        }

        public Task LogoutAsync()
        {
            _contextAccessor.HttpContext?.Session.Clear();
            return Task.CompletedTask;
        }
    }
}
