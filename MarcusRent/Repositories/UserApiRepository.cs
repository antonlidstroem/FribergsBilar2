using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Fribergs.Core.DTO;
using MarcusRent.Interfaces;
using Microsoft.AspNetCore.Mvc;



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
        public async Task<MeResponse?> GetMeAsync()
        {
            AddJwtToken();
            return await _httpClient.GetFromJsonAsync<MeResponse>("api/auth/me");
        }
        public async Task<MeResponse?> GetMeAsync(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                !string.IsNullOrEmpty(token)
                    ? new AuthenticationHeaderValue("Bearer", token)
                    : null;

            return await _httpClient.GetFromJsonAsync<MeResponse>("api/auth/me");
        }
        public async Task<UserDto?> GetUserByIdAsync(string id)
        {
            AddJwtToken();
            var response = await _httpClient.GetAsync($"api/users/{id}");
            if (!response.IsSuccessStatusCode) return null;

            return await response.Content.ReadFromJsonAsync<UserDto>();
        }

        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            AddJwtToken();
            var response = await _httpClient.GetFromJsonAsync<List<UserDto>>("api/users");
            return response ?? new List<UserDto>();
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
        public async Task<bool> UpdateUserAsync(UserDto user)
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

        private void AddJwtToken()
        {
            _authService.AddJwtToken();
        }
    }
}

