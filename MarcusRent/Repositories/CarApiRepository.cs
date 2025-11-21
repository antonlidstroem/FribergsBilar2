using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Fribergs.Core.DTO;
using Microsoft.EntityFrameworkCore;


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

        // GET all cars
        public async Task<List<CarDto>> GetCarsAsync()
        {
            AddJwtToken();
            var response = await _httpClient.GetFromJsonAsync<List<CarDto>>("api/cars");
            return response ?? new List<CarDto>();
        }

        // GET single car
        public async Task<CarDto?> GetCarByIdAsync(int id)
        {
            AddJwtToken();
            return await _httpClient.GetFromJsonAsync<CarDto>($"api/cars/{id}");
        }

        // POST new car
        public async Task<CarDto?> CreateCarAsync(CarDto car)
        {
            AddJwtToken();
            var response = await _httpClient.PostAsJsonAsync("api/cars", car);
            if (!response.IsSuccessStatusCode) return null;

            return await response.Content.ReadFromJsonAsync<CarDto>();
        }

        //// PUT update car
        public async Task<bool> UpdateCarAsync(CarDto car)
        {
            AddJwtToken();
            Debug.WriteLine($"PUT /api/cars/{car.CarId}");
            Debug.WriteLine($"DTO CarId: {car.CarId}");

            var response = await _httpClient.PutAsJsonAsync($"api/cars/{car.CarId}", car);
            var content = await response.Content.ReadAsStringAsync();
            Debug.WriteLine($"Response: {response.StatusCode}, {content}");

            return response.IsSuccessStatusCode;
        }

        // DELETE car
        public async Task<bool> DeleteCarAsync(int id)
        {
            AddJwtToken();
            var response = await _httpClient.DeleteAsync($"api/cars/{id}");
            return response.IsSuccessStatusCode;
        }

        private void AddJwtToken()
        {
            _authService.AddJwtToken();
        }

        public async Task<bool> IsCarInAnyOrderAsync(int carId)
        {
            AddJwtToken();

            var response = await _httpClient.GetAsync($"api/cars/{carId}/isinorder");

            if (!response.IsSuccessStatusCode)
                return false;

            return await response.Content.ReadFromJsonAsync<bool>();
        }
    }
}
