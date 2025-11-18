
using Fribergs.Core.DTO;


namespace MarcusRent.Interfaces
{
    public interface ICarApiRepository
    {
        Task<List<CarDto>> GetCarsAsync();
        Task<CarDto> GetCarByIdAsync(int id);
    }
}
