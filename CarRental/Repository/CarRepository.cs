using CarRental.DBContext;
using CarRental.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Repository
{
    public class CarRepository : ICarRepository
    {
        private readonly Context _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CarRepository(Context context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<List<Car>> GetAllAsync()
        {
            return await _context.Cars.ToListAsync();
        }

        public async Task<List<Car>> GetByModelAsync(string model)
        {
            return await _context.Cars
                .Where(c => c.Model.ToLower().Contains(model.ToLower()))
                .ToListAsync();
        }

        public async Task<List<Car>> GetByBrandAsync(string brand)
        {
            return await _context.Cars
                .Where(c => c.Brand.ToLower().Contains(brand.ToLower()))
                .ToListAsync();
        }

        public async Task<Car> GetByIdAsync(int id)
        {
            return await _context.Cars.FindAsync(id);
        }

        public async Task AddAsync(Car newCar)
        {
            await _context.Cars.AddAsync(newCar);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateByIdAsync(int id, Car NewCar)
        {

            var car = await _context.Cars.FindAsync(id);
            if (car == null) return;

            if (NewCar.ImageURL != null)
            {
                if (!string.IsNullOrEmpty(car.ImageName))
                {
                    // Define the file path for the old image
                    var oldFilePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", car.ImageName);

                    // Check if the old file exists, then delete it
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }
                // Generate a unique name for the uploaded file
                var fileName = Path.GetFileNameWithoutExtension(NewCar.ImageURL.FileName) +
                               Guid.NewGuid().ToString() +
                               Path.GetExtension(NewCar.ImageURL.FileName);

                // Set the image name in the model
                NewCar.ImageName = fileName;

                // Define the file path where to save the uploaded image
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", fileName);

                // Save the file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await NewCar.ImageURL.CopyToAsync(stream);
                }
            }
            else
                return;

            car.Brand = NewCar.Brand;
            car.Model = NewCar.Model;
            car.YearOfMake = NewCar.YearOfMake;
            car.PricePerDay = NewCar.PricePerDay;
            car.color = NewCar.color;
            car.Brand = NewCar.Brand;
            car.PlateNumber = NewCar.PlateNumber;
            car.ImageName = NewCar.ImageName;
            car.ImageURL = NewCar.ImageURL;
            _context.Cars.Update(car);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(int id)
        {
            // Find the rented car record
            var rentedCar = _context.RentedCars
                .FirstOrDefault(rc => rc.CarId == id);

            if (rentedCar == null)
            {
                return;
            }

            // Mark car as available again
            var car = await _context.Cars.FindAsync(id);
            if (car != null)
            {
                car.IsRented = false;
                _context.Update(car);
            }

            // Update the user's HasRentedCar
            var user = _context.Users.FirstOrDefault(u => u.Id == rentedCar.RenterId);
            if (user != null)
            {
                user.HasRentedCar = false;
                _context.Update(user);
            }
            if (car == null) return;
            if (!string.IsNullOrEmpty(car.ImageName))
            {
                // Define the file path for the old image
                var oldFilePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", car.ImageName);

                // Check if the old file exists, then delete it
                if (System.IO.File.Exists(oldFilePath))
                {
                    System.IO.File.Delete(oldFilePath);
                }
            }
            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Car>> GetAllAvailableAsync() 
        => await _context.Cars.Where(c => c.IsRented == false).ToListAsync();
        
    }
}
