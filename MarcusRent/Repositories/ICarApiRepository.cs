using Fribergs.Core.DTO;

namespace MarcusRent.Repositories
{
    public interface ICarApiRepository
    {
        Task<List<CarDto>> GetCarsAsync();
        Task<CarDto> GetCarByIdAsync(int id);
        Task<CarDto?> CreateCarAsync(CarDto car);
        Task<bool> UpdateCarAsync(CarDto car);
        Task<bool> DeleteCarAsync(int id);
    }
}
