using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Fribergs.Core.DTO;
using MarcusRent.Interfaces;

namespace MarcusRent.Repositories
{
    public class CarApiRepository : ICarApiRepository
    {
        private readonly HttpClient _httpClient;
        private readonly IAuthService _authService;

        public CarApiRepository(HttpClient httpClient, IAuthService authService)
        {
            _httpClient = httpClient;
            _authService = authService;
        }

        private void SetJwtToken()
        {
            var token = _authService.GetJwtToken();
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
            else
            {
                _httpClient.DefaultRequestHeaders.Authorization = null;
            }
        }

        public async Task<List<CarDto>> GetCarsAsync()
        {
            SetJwtToken();
            var response = await _httpClient.GetFromJsonAsync<List<CarDto>>("api/cars");
            return response ?? new List<CarDto>();
        }

        public async Task<CarDto?> GetCarByIdAsync(int id)
        {
            SetJwtToken();
            return await _httpClient.GetFromJsonAsync<CarDto>($"api/cars/{id}");
        }

        public async Task<CarDto?> CreateCarAsync(CarDto car)
        {
            SetJwtToken();
            var response = await _httpClient.PostAsJsonAsync("api/cars", car);
            if (!response.IsSuccessStatusCode) return null;

            return await response.Content.ReadFromJsonAsync<CarDto>();
        }

        public async Task<bool> UpdateCarAsync(CarDto car)
        {
            SetJwtToken();
            Debug.WriteLine($"PUT /api/cars/{car.CarId}");

            var response = await _httpClient.PutAsJsonAsync($"api/cars/{car.CarId}", car);
            var content = await response.Content.ReadAsStringAsync();
            Debug.WriteLine($"Response: {response.StatusCode}, {content}");

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteCarAsync(int id)
        {
            SetJwtToken();
            var response = await _httpClient.DeleteAsync($"api/cars/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> IsCarInAnyOrderAsync(int carId)
        {
            SetJwtToken();
            var response = await _httpClient.GetAsync($"api/cars/{carId}/isinorder");

            if (!response.IsSuccessStatusCode)
                return false;

            return await response.Content.ReadFromJsonAsync<bool>();
        }
    }
}
