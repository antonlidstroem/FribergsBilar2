using DAL.Classes;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class ApplicationUserRepository : IApplicationUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ApplicationUserRepository(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ApplicationUser?> AddAsync(string firstName, string lastName, string email, string password, string role)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);

            if (existingUser != null)
            {
                return null;
            }

            var newUser = new ApplicationUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true,
                ApprovedByAdmin = true,
                FirstName = firstName,
                LastName = lastName
            };

            var result = await _userManager.CreateAsync(newUser, password);

            if (!result.Succeeded)
            {
                return null;
            }

            if (!string.IsNullOrWhiteSpace(role))
            {
                var addToRoleResult = await _userManager.AddToRoleAsync(newUser, role);

                if (!addToRoleResult.Succeeded)
                {
                    return null;
                }
            }
            return newUser;
        }

        public async Task<List<ApplicationUser>> GetAllUsersAsync()
        {
            return await _userManager.Users.ToListAsync();
        }
        public async Task<ApplicationUser?> GetUserByIdAsync(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }
        public async Task ApproveUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                user.ApprovedByAdmin = true;
                await _userManager.UpdateAsync(user);
            }
        }

        public async Task DeleteUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }
        }
        //public async Task UpdateUserAsync(ApplicationUser user)
        //{
        //    await _userManager.UpdateAsync(user);
        //}

        public async Task<bool> UpdateUserAsync(ApplicationUser user)
        {
            var existingUser = await _userManager.FindByIdAsync(user.Id);
            if (existingUser == null)
            {
                return false; // Om användaren inte finns
            }

            // Uppdatera användarens fält
            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.Email = user.Email;
            existingUser.ApprovedByAdmin = user.ApprovedByAdmin;

            // Sätt andra fält om det behövs

            var result = await _userManager.UpdateAsync(existingUser);
            return result.Succeeded;
        }

    }

}

