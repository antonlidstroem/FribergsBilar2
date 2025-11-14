
using Fribergs.Core.DTO;


namespace MarcusRent.Repositories
{
    public interface ICarApiRepository
    {
        Task<List<CarDto>> GetCarsAsync();
        Task<CarDto> GetCarByIdAsync(int id);
    }
}
