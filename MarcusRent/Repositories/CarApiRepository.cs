using System.Net.Http.Headers;
using System.Net.Http.Json;
using MarcusRent.Models;

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

        private void AddJwtToken()
        {
            var token = _authService.GetJwtTokenFromSession();
            _httpClient.DefaultRequestHeaders.Authorization =
                !string.IsNullOrEmpty(token)
                    ? new AuthenticationHeaderValue("Bearer", token)
                    : null;
        }

        // GET all cars
        public async Task<List<CarDtoClient>> GetCarsAsync()
        {
            AddJwtToken();
            var response = await _httpClient.GetFromJsonAsync<List<CarDtoClient>>("api/cars");
            return response ?? new List<CarDtoClient>();
        }

        // GET single car
        public async Task<CarDtoClient?> GetCarByIdAsync(int id)
        {
            AddJwtToken();
            return await _httpClient.GetFromJsonAsync<CarDtoClient>($"api/cars/{id}");
        }

        // POST new car
        public async Task<CarDtoClient?> CreateCarAsync(CarDtoClient car)
        {
            AddJwtToken();
            var response = await _httpClient.PostAsJsonAsync("api/cars", car);
            if (!response.IsSuccessStatusCode) return null;

            return await response.Content.ReadFromJsonAsync<CarDtoClient>();
        }

        // PUT update car
        public async Task<bool> UpdateCarAsync(CarDtoClient car)
        {
            AddJwtToken();
            var response = await _httpClient.PutAsJsonAsync($"api/cars/{car.CarId}", car);
            return response.IsSuccessStatusCode;
        }

        // DELETE car
        public async Task<bool> DeleteCarAsync(int id)
        {
            AddJwtToken();
            var response = await _httpClient.DeleteAsync($"api/cars/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
