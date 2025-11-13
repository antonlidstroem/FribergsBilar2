using System.Net.Http.Headers;
using System.Net.Http.Json;
using MarcusRent.Models;

namespace MarcusRent.Repositories
{
    public class UserApiRepository : IUserApiRepository
    {
        private readonly HttpClient _httpClient;
        private readonly IAuthService _authService;

        public UserApiRepository(HttpClient httpClient, IAuthService authService)
        {
            _httpClient = httpClient;
            _authService = authService;
        }

        private void AddJwtToken()
        {
            var token = _authService.GetJwtTokenFromSession();
            _httpClient.DefaultRequestHeaders.Authorization =
                !string.IsNullOrEmpty(token)
                    ? new AuthenticationHeaderValue("Bearer", token)
                    : null;
        }

        public async Task<MeResponseClient?> GetMeAsync()
        {
            AddJwtToken();
            return await _httpClient.GetFromJsonAsync<Models.MeResponseClient>("api/auth/me");
        }

        public async Task<MeResponseClient?> GetMeAsync(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                !string.IsNullOrEmpty(token)
                    ? new AuthenticationHeaderValue("Bearer", token)
                    : null;

            return await _httpClient.GetFromJsonAsync<Models.MeResponseClient>("api/auth/me");
        }

        public async Task<UserDtoClient?> GetUserByIdAsync(string id)
        {
            AddJwtToken();
            var response = await _httpClient.GetAsync($"api/users/{id}");
            if (!response.IsSuccessStatusCode) return null;

            return await response.Content.ReadFromJsonAsync<UserDtoClient>();
        }

        public async Task<List<UserDtoClient>> GetAllUsersAsync()
        {
            AddJwtToken();
            var response = await _httpClient.GetFromJsonAsync<List<UserDtoClient>>("api/users");
            return response ?? new List<UserDtoClient>();
        }

        public async Task ApproveUserAsync(string id)
        {
            AddJwtToken();
            var response = await _httpClient.PostAsync($"api/users/{id}/approve", null);
            if (!response.IsSuccessStatusCode)
                throw new Exception("Could not approve user");
        }

       
        public async Task<bool> DeleteUserAsync(string id)
        {
            AddJwtToken();
            var response = await _httpClient.DeleteAsync($"api/users/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateUserAsync(UserDtoClient user)
        {
            AddJwtToken();
            var response = await _httpClient.PutAsJsonAsync($"api/users/{user.UserId}", user);
            return response.IsSuccessStatusCode;
        }

        public async Task<CurrentUserDto> GetCurrentUserAsync()
        {
            AddJwtToken();
            var response = await _httpClient.GetAsync("api/auth/me");
            if (!response.IsSuccessStatusCode) return null;

            return await response.Content.ReadFromJsonAsync<CurrentUserDto>();
        }

    }
}
