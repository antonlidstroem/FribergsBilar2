using System.Net.Http.Headers;
using System.Net.Http.Json;
using Fribergs.Core.DTO;
using MarcusRent.Interfaces;

namespace MarcusRent.Repositories
{
    public class OrderApiRepository : IOrderApiRepository
    {
        private readonly HttpClient _httpClient;
        private readonly IAuthService _authService;

        public OrderApiRepository(HttpClient httpClient, IAuthService authService)
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

        public async Task<List<OrderDto>> GetOrdersAsync()
        {
            SetJwtToken();
            var response = await _httpClient.GetFromJsonAsync<List<OrderDto>>("api/orders");
            return response ?? new List<OrderDto>();
        }

        public async Task<List<OrderDto>> GetOrdersByUserIdAsync(string userId)
        {
            SetJwtToken();
            var response = await _httpClient.GetFromJsonAsync<List<OrderDto>>($"api/orders/user/{userId}");
            return response ?? new List<OrderDto>();
        }

        public async Task<OrderDto?> GetOrderByIdAsync(int id)
        {
            SetJwtToken();
            return await _httpClient.GetFromJsonAsync<OrderDto>($"api/orders/{id}");
        }

        public async Task AddOrderAsync(OrderDto order)
        {
            SetJwtToken();
            await _httpClient.PostAsJsonAsync("api/orders", order);
        }

        public async Task UpdateOrderAsync(OrderDto order)
        {
            SetJwtToken();
            await _httpClient.PutAsJsonAsync($"api/orders/{order.OrderId}", order);
        }

        public async Task DeleteOrderAsync(int id)
        {
            SetJwtToken();
            await _httpClient.DeleteAsync($"api/orders/{id}");
        }

        public async Task<bool> IsCarBookedAsync(int carId, DateTime startDate, DateTime endDate)
        {
            SetJwtToken();
            var response = await _httpClient.GetFromJsonAsync<bool>(
                $"api/orders/isCarBooked?carId={carId}&startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}"
            );
            return response;
        }

        public async Task<decimal> GetTotalEarningsForCarAsync(int carId)
        {
            SetJwtToken();
            var response = await _httpClient.GetFromJsonAsync<decimal>($"api/orders/totalEarnings/{carId}");
            return response;
        }
    }
}
