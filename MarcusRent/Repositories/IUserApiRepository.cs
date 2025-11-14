


using Fribergs.Core.DTO;

namespace MarcusRent.Repositories
{
    public interface IUserApiRepository
    {
        //Task<MeResponse?> GetMeAsync();
        //Task<MeResponse?> GetMeAsync(string token);
        Task<UserDto?> GetUserByIdAsync(string id);
        Task<List<UserDto>> GetAllUsersAsync();
        Task ApproveUserAsync(string id);

        Task<bool> UpdateUserAsync(UserDto user);
        Task<bool> DeleteUserAsync(string id);
        Task<CurrentUserDto> GetCurrentUserAsync();
        Task<MeResponse?> GetMeAsync();
        Task<MeResponse?> GetMeAsync(string token);

    }


}
