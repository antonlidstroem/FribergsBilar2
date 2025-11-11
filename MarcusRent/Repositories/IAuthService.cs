namespace MarcusRent.Repositories
{
    public interface IAuthService
    {
        Task<bool> LoginAsync(string email, string password);
        void Logout();
        string GetJwtTokenFromSession();
    }

}