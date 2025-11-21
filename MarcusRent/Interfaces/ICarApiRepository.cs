using Fribergs.Core.DTO;

namespace MarcusRent.Interfaces
{
    public interface ICarApiRepository
    {
        Task<List<CarDto>> GetCarsAsync();
        Task<CarDto> GetCarByIdAsync(int id);
        Task<CarDto?> CreateCarAsync(CarDto car);
        Task<bool> UpdateCarAsync(CarDto car);
        Task<bool> DeleteCarAsync(int id);
        Task<bool> IsCarInAnyOrderAsync(int carId);

    }
}
