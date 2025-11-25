using Fribergs.Core.DTO;

namespace MarcusRent.Interfaces
{
    public interface IAuthService
    {
        Task<bool> LoginAsync(string email, string password);
        string GetJwtToken();
        Task LogoutAsync();
    }
}
