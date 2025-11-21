using DAL.Classes;
using Microsoft.AspNetCore.Identity;

namespace DAL.Repositories
{
    public interface IApplicationUserRepository
    {
        Task<List<ApplicationUser>> GetAllUsersAsync();
        Task<ApplicationUser?> GetUserByIdAsync(string id);
        Task ApproveUserAsync(string id);
        Task<bool> DeleteUserAsync(string id);
        Task<bool> UpdateUserAsync(ApplicationUser user);
        Task<ApplicationUser?> AddAsync(string firstName, string lastName, string email, string password, string role);
       
    }
}