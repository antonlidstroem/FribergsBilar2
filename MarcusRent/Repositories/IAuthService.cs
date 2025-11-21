using Fribergs.Core.DTO;

namespace MarcusRent.Repositories
{
    public interface IAuthService
    {
        void AddJwtToken();
        string GetJwtToken();
        Task<bool> LoginAsync(string email, string password);
        Task<AuthResponse?> RegisterAsync(string email, string password);


    }

}