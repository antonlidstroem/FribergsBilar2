using System.Net.Http.Headers;
using System.Net.Http.Json;
using DAL.Classes;
using MarcusRent.Models;

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

        private void AddJwtToken()
        {
            var token = _authService.GetJwtTokenFromSession();
            _httpClient.DefaultRequestHeaders.Authorization =
                !string.IsNullOrEmpty(token)
                    ? new AuthenticationHeaderValue("Bearer", token)
                    : null;
        }

        // GET all orders
        public async Task<List<OrderDtoClient>> GetOrdersAsync()
        {
            AddJwtToken();
            var response = await _httpClient.GetFromJsonAsync<List<OrderDtoClient>>("api/orders");
            return response ?? new List<OrderDtoClient>();
        }

        // GET orders by user
        public async Task<List<OrderDtoClient>> GetOrdersByUserIdAsync(string userId)
        {
            AddJwtToken();
            var response = await _httpClient.GetFromJsonAsync<List<OrderDtoClient>>($"api/orders/user/{userId}");
            return response ?? new List<OrderDtoClient>();
        }

        // GET single order
        public async Task<OrderDtoClient?> GetOrderByIdAsync(int id)
        {
            AddJwtToken();
            return await _httpClient.GetFromJsonAsync<OrderDtoClient>($"api/orders/{id}");
        }

        // POST new order
        public async Task AddOrderAsync(OrderDtoClient order)
        {
            AddJwtToken();
            await _httpClient.PostAsJsonAsync("api/orders", order);
        }

        // PUT update order
        public async Task UpdateOrderAsync(OrderDtoClient order)
        {
            AddJwtToken();
            await _httpClient.PutAsJsonAsync($"api/orders/{order.OrderId}", order);
        }

        // DELETE order
        public async Task DeleteOrderAsync(int id)
        {
            AddJwtToken();
            await _httpClient.DeleteAsync($"api/orders/{id}");
        }

        // Check if car is booked
        public async Task<bool> IsCarBookedAsync(int carId, DateTime startDate, DateTime endDate)
        {
            AddJwtToken();
            var response = await _httpClient.GetFromJsonAsync<bool>(
                $"api/orders/isCarBooked?carId={carId}&startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}"
            );
            return response;
        }

        public async Task<decimal> GetTotalEarningsForCarAsync(int carId)
        {
            AddJwtToken();
            var response = await _httpClient.GetFromJsonAsync<decimal>($"api/orders/totalEarnings/{carId}");
            return response;
        }
    }
}
