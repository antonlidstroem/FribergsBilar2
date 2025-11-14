using DAL.Classes;
using Fribergs.Core.DTO;


namespace MarcusRent.Repositories
{
    public interface IOrderApiRepository
    {
        Task<OrderDto?> GetOrderByIdAsync(int id);
        Task<List<OrderDto>> GetOrdersAsync();
        Task<List<OrderDto>> GetOrdersByUserIdAsync(string userId);
        Task AddOrderAsync(OrderDto order);
        Task UpdateOrderAsync(OrderDto order);
        Task DeleteOrderAsync(int id);
        Task<bool> IsCarBookedAsync(int carId, DateTime startDate, DateTime endDate);
        Task<decimal> GetTotalEarningsForCarAsync(int carId);
    }
}
