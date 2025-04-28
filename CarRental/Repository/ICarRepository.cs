using CarRental.Models;

namespace CarRental.Repository
{
    public interface ICarRepository
    {
        Task<List<Car>> GetAllAsync();
        Task<List<Car>> GetAllAvailableAsync();
        Task<List<Car>> GetByModelAsync(string model);
        Task<List<Car>> GetByBrandAsync(string brand);
        Task<Car> GetByIdAsync(int id);
        Task UpdateByIdAsync(int id, Car newCar);
        Task DeleteByIdAsync(int id);
        Task AddAsync(Car newCar);



    }
}
