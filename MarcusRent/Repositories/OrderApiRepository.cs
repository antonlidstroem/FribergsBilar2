using System.Net.Http.Headers;
using System.Net.Http.Json;
using DAL.Classes;
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

       
        // GET all orders
        public async Task<List<OrderDto>> GetOrdersAsync()
        {
            AddJwtToken();
            var response = await _httpClient.GetFromJsonAsync<List<OrderDto>>("api/orders");
            return response ?? new List<OrderDto>();
        }

        // GET orders by user
        public async Task<List<OrderDto>> GetOrdersByUserIdAsync(string userId)
        {
            AddJwtToken();
            var response = await _httpClient.GetFromJsonAsync<List<OrderDto>>($"api/orders/user/{userId}");
            return response ?? new List<OrderDto>();
        }

        // GET single order
        public async Task<OrderDto?> GetOrderByIdAsync(int id)
        {
            AddJwtToken();
            return await _httpClient.GetFromJsonAsync<OrderDto>($"api/orders/{id}");
        }

        // POST new order
        public async Task AddOrderAsync(OrderDto order)
        {
            AddJwtToken();
            await _httpClient.PostAsJsonAsync("api/orders", order);
        }

        // PUT update order
        public async Task UpdateOrderAsync(OrderDto order)
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
        private void AddJwtToken()
        {
            _authService.AddJwtToken();
        }
    }
}
