using DAL.Classes;
using DAL.Interfaces;
using Microsoft.AspNetCore.Identity;

public class UserService
{
    private readonly IApplicationUserRepository _repository;
    private readonly UserManager<ApplicationUser> _userManager;

    public UserService(IApplicationUserRepository repository, UserManager<ApplicationUser> userManager)
    {
        _repository = repository;
        _userManager = userManager;
    }
    public async Task<ApplicationUser?> RegisterAsync(string firstName, string lastName, string email, string password, string role)
    {
        var user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            FirstName = firstName,
            LastName = lastName
        };

        var result = await _userManager.CreateAsync(user, password);
        if (!result.Succeeded) return null;

        if (!string.IsNullOrEmpty(role))
            await _userManager.AddToRoleAsync(user, role);

        return user;
    }
    public Task<List<ApplicationUser>> GetAllUsersAsync()
    {
        return _repository.GetAllUsersAsync();
    }
    public async Task<ApplicationUser?> ValidateUserAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) return null;

        var isValid = await _userManager.CheckPasswordAsync(user, password);
        return isValid ? user : null;
    }
    public async Task<IList<string>> GetRolesAsync(ApplicationUser user)
    {
        if (user == null) return new List<string>();
        return await _userManager.GetRolesAsync(user);
    }
    public async Task<ApplicationUser?> GetUserByIdAsync(string userId)
    {
        return await _userManager.FindByIdAsync(userId);
    }
    public async Task<ApplicationUser> GetUserByEmailAsync(string email)
    {
        return await _userManager.FindByEmailAsync(email);
    }
}
