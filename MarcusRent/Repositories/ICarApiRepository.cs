
using MarcusRent.Models;

namespace MarcusRent.Repositories
{
    public interface ICarApiRepository
    {
        Task<List<CarDtoClient>> GetCarsAsync();
        Task<CarDtoClient> GetCarByIdAsync(int id);
    }
}
