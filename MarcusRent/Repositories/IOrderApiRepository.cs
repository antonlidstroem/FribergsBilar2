using DAL.Classes;
using MarcusRent.Models;

namespace MarcusRent.Repositories
{
    public interface IOrderApiRepository
    {
        Task<OrderDtoClient?> GetOrderByIdAsync(int id);
        Task<List<OrderDtoClient>> GetOrdersAsync();
        Task<List<OrderDtoClient>> GetOrdersByUserIdAsync(string userId);
        Task AddOrderAsync(OrderDtoClient order);
        Task UpdateOrderAsync(OrderDtoClient order);
        Task DeleteOrderAsync(int id);
        Task<bool> IsCarBookedAsync(int carId, DateTime startDate, DateTime endDate);
        Task<decimal> GetTotalEarningsForCarAsync(int carId);
    }
}
