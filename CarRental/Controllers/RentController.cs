using System.Security.Claims;
using System.Threading.Tasks;
using CarRental.DBContext;
using CarRental.Models;
using CarRental.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Controllers
{
    [Authorize(Roles = "User")]
    public class RentController : Controller
    {
        private readonly ICarRepository _carRepo;
        private readonly Context _context;

        public RentController(ICarRepository carRepo, Context context)
        {
           _carRepo = carRepo;
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var cars = await _carRepo.GetAllAvailableAsync();
            return View(cars);
        }

        [Authorize]
        public IActionResult RentCar(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Check if the user has already rented a car
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user != null && user.HasRentedCar)
            {
                // If the user has already rented a car, redirect with a message or alert
                TempData["ErrorMessage"] = "You can only rent one car at a time!";
                return RedirectToAction("Index");
            }

            // Check if car exists and is available
            var car = _context.Cars.FirstOrDefault(c => c.Id == id && !c.IsRented);
            if (car == null)
            {
                // Car is either not found or already rented
                TempData["ErrorMessage"] = "Car is either not available or already rented!";
                return RedirectToAction("Index");
            }

            // Mark car as rented
            car.IsRented = true;

            // Create a new RentedCar record
            var rentedCar = new RentedCar
            {
                CarId = car.Id,
                RenterId = userId,
                RentedAt = DateTime.UtcNow
            };

            // Add RentedCar record to the database
            _context.RentedCars.Add(rentedCar);

            // Update user’s HasRentedCar flag to true
            if (user != null)
            {
                user.HasRentedCar = true;
                _context.Update(user); // Update the user entity
            }

            // Save all changes to the database
            _context.SaveChanges();

            // Redirect to the index or a success page
            TempData["SuccessMessage"] = "Car rented successfully!";
            return RedirectToAction("Index");
        }


    }
}
