using DAL.Classes;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public interface ICarRepository
    {
        Task<List<Car>> GetAllAsync();
        Task<Car?> GetByIdAsync(int id);
        Task AddAsync(Car car);
        Task UpdateAsync(Car car);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);

        //public IQueryable<Car> GetAllAvailable();
        Task<List<Car>> GetAllAvailableAsync();


    }
}

