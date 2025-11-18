namespace MarcusRent.Interfaces
{
    public interface IAuthService
    {
        void AddJwtToken();
        string GetJwtToken();
        Task<bool> LoginAsync(string email, string password);
        //void Logout();
        //string GetJwtTokenFromSession();
        
    }

}