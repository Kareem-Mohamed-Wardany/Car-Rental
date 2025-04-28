using System.Security.Claims;
using CarRental.DBContext;
using CarRental.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Controllers
{
    [Authorize] // Optional: Only allow logged-in users to access
    public class RentedCarsController : Controller
    {
        private readonly Context _context;

        public RentedCarsController(Context context)
        {
            _context = context;
        }
    public IActionResult Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var isAdmin = User.IsInRole("Admin");

        List<RentedCar> rentedCars;

        if (isAdmin)
        {
            // Admin sees all rented cars
            rentedCars = _context.RentedCars
                .Include(rc => rc.Car)
                .Include(rc => rc.Renter)
                .ToList();
        }
        else
        {
            // Normal user sees only their rented cars
            rentedCars = _context.RentedCars
                .Include(rc => rc.Car)
                .Include(rc => rc.Renter)
                .Where(rc => rc.RenterId == userId)
                .ToList();
        }

        return View(rentedCars);
    }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CancelRent(int carId, string userId)
        {
            // Find the rented car record
            var rentedCar = _context.RentedCars
                .FirstOrDefault(rc => rc.CarId == carId && rc.RenterId == userId);

            if (rentedCar == null)
            {
                return NotFound();
            }

            // Mark car as available again
            var car = _context.Cars.FirstOrDefault(c => c.Id == carId);
            if (car != null)
            {
                car.IsRented = false;
                _context.Update(car);
            }

            // Update the user's HasRentedCar
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user != null)
            {
                user.HasRentedCar = false;
                _context.Update(user);
            }

            // Remove rented car record
            _context.RentedCars.Remove(rentedCar);

            _context.SaveChanges(); // Save everything

            return RedirectToAction("Index");
        }

    }
}
