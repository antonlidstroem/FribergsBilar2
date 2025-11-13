
using Fribergs.Core.Models;


namespace MarcusRent.Repositories
{
    public interface ICarApiRepository
    {
        Task<List<CarDto>> GetCarsAsync();
        Task<CarDto> GetCarByIdAsync(int id);
    }
}
