using MarcusRent.Models;


namespace MarcusRent.Repositories
{
    public interface IUserApiRepository
    {
        Task<MeResponseClient?> GetMeAsync();
        Task<MeResponseClient?> GetMeAsync(string token);
        Task<UserDtoClient?> GetUserByIdAsync(string id);
        Task<List<UserDtoClient>> GetAllUsersAsync();
        Task ApproveUserAsync(string id);

        Task<bool> UpdateUserAsync(UserDtoClient user);
        Task<bool> DeleteUserAsync(string id);
        Task<CurrentUserDto> GetCurrentUserAsync();

    }


}
