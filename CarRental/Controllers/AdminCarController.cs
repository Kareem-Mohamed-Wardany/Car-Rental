using System.Threading.Tasks;
using CarRental.Models;
using CarRental.Repository;
using CarRental.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CarRental.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminCarController : Controller
    {
        private readonly ICarRepository _carRepo;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AdminCarController(ICarRepository carRepo, IWebHostEnvironment webHostEnvironment)
        {
            _carRepo = carRepo;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            ViewBag.RentedOptions = new List<SelectListItem>
            {
                new SelectListItem { Value = "", Text = "All Cars" },
                new SelectListItem { Value = "true", Text = "Rented" },
                new SelectListItem { Value = "false", Text = "Available" }
            };

            var cars = await _carRepo.GetAllAsync();
            return View(cars);
        }
        [HttpGet]
        public IActionResult AddCar()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCar(CarToAdd NewCar)
        {
            if (ModelState.IsValid)
            {
                if (NewCar.ImageURL != null)
                {
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
                    return View(NewCar);
                Car car = new Car();
                car.Brand = NewCar.Brand;
                car.Model = NewCar.Model;
                car.YearOfMake = NewCar.YearOfMake;
                car.PricePerDay = NewCar.PricePerDay;
                car.color = NewCar.color;
                car.Brand = NewCar.Brand;
                car.PlateNumber = NewCar.PlateNumber;
                car.ImageName = NewCar.ImageName;
                car.ImageURL = NewCar.ImageURL;

                await _carRepo.AddAsync(car);
                return RedirectToAction("index");
            }
            return View(NewCar);
        }
        [HttpGet]
        public async Task<IActionResult> EditCar(int id)
        {
            var car = await _carRepo.GetByIdAsync(id);
            return View(car);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCar(Car NewCar)
        {
            if (ModelState.IsValid)
            {
                await _carRepo.UpdateByIdAsync(NewCar.Id,NewCar);
                return RedirectToAction("index");
            }
            return View(NewCar);
        }
        [HttpGet]
        public async Task<IActionResult> DeleteCar(int id)
        {
            await _carRepo.DeleteByIdAsync(id);
            return RedirectToAction("index");
        }
    }
}
