using Microsoft.AspNetCore.Mvc;
using Shop.Models.Car;
using ShopTARgv21.Data;
using Microsoft.EntityFrameworkCore;
using Shop.Core.ServiceInterface;
using Shop.Core.Dto;

namespace Shop.Controllers
{
    public class CarController : Controller
    {
        private readonly ShopDbContext _dbcontext;
        private readonly ICarServices _carServices;
        private readonly IFileServices _fileServices;

        public CarController
            (
                ShopDbContext dbcontext, 
                ICarServices carServices,
                IFileServices fileServices
            )
        {
            _dbcontext = dbcontext;
            _carServices = carServices;
            _fileServices = fileServices;
        }

        [HttpGet]

        public IActionResult Index()
        {
            var result = _dbcontext.Car
                .OrderByDescending(y => y.DateOfRegistration)
                .Select(x => new CarListViewModel
                {
                    Id = x.Id,
                    OwnerName = x.OwnerName,
                    NumberOfRegistration = x.NumberOfRegistration,
                    Brand = x.Brand,
                    Model = x.Model,
                    Color = x.Color
                });
            return View(result);
        }
        [HttpGet]
        public IActionResult Add()
        {
            CarEditViewModel spaceship = new CarEditViewModel();

            return View("Edit", spaceship);
        }

        [HttpPost]
        public async Task<IActionResult> Add(CarViewModel vm)
        {
            var dto = new CarDto()
            {
                Id = vm.Id,
                OwnerName = vm.OwnerName,
                NumberOfRegistration = vm.NumberOfRegistration,
                VINCode= vm.VINCode,
                Brand = vm.Brand,
                Model = vm.Model,
                Color = vm.Color,
                Fuel= vm.Fuel,
                Capacity= vm.Capacity,
                NumberOfCarDoors= vm.NumberOfCarDoors,
                NumberOfPassangersWithDriver= vm.NumberOfPassangersWithDriver,
                CarWeight= vm.CarWeight,
                BuildOfDate= vm.BuildOfDate,
                DateOfRegistration  = vm.DateOfRegistration,
                Files = vm.Files,
                Image = vm.Image.Select(x => new FileToDatabaseDto
                {
                    Id = x.ImageId,
                    ImageData = x.ImageData,
                    ImageTitle = x.ImageTitle,
                    CarId = x.CarId
                }).ToArray()
            };

            var result = await _carServices.Add(dto);

            if (result is null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index), vm);
        }

        [HttpGet]

        public async Task<IActionResult> Edit(Guid id)
        {
            var car = await _carServices.GetAsync(id);

            if (car == null)
            {
                return NotFound();
            }
            var photos = await _dbcontext.FileToDatabase
                .Where(x => x.CarId == id)
                .Select(y => new ImageViewModel
                {
                    ImageData = y.ImageData,
                    ImageId = y.Id,
                    Image = string.Format("data:image/gif;base64,{0}", Convert.ToBase64String(y.ImageData)),
                    ImageTitle = y.ImageTitle,
                    CarId = y.Id
                }).ToArrayAsync();

            var vm = new CarEditViewModel();

            vm.Id = car.Id;
            vm.OwnerName = car.OwnerName;
            vm.NumberOfRegistration = car.NumberOfRegistration;
            vm.VINCode = car.VINCode;
            vm.Brand = car.Brand;
            vm.Model = car.Model;
            vm.Color = car.Color;
            vm.Fuel = car.Fuel;
            vm.Capacity = car.Capacity;
            vm.NumberOfCarDoors = car.NumberOfCarDoors;
            vm.NumberOfPassangersWithDriver = car.NumberOfPassangersWithDriver;
            vm.CarWeight = car.CarWeight;
            vm.BuildOfDate = car.BuildOfDate;
            vm.DateOfRegistration = car.DateOfRegistration;
            vm.Image.AddRange(photos);

            return View("Edit", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CarEditViewModel vm)
        {
            var dto = new CarDto()
            {
                Id = vm.Id,
                OwnerName = vm.OwnerName,
                NumberOfRegistration = vm.NumberOfRegistration,
                VINCode = vm.VINCode,
                Brand = vm.Brand,
                Model = vm.Model,
                Color = vm.Color,
                Fuel = vm.Fuel,
                Capacity = vm.Capacity,
                NumberOfCarDoors = vm.NumberOfCarDoors,
                NumberOfPassangersWithDriver = vm.NumberOfPassangersWithDriver,
                CarWeight = vm.CarWeight,
                BuildOfDate = vm.BuildOfDate,
                DateOfRegistration = vm.DateOfRegistration,
                Files = vm.Files,
                Image = vm.Image.Select(x => new FileToDatabaseDto
                {
                    Id = x.ImageId,
                    ImageData = x.ImageData,
                    ImageTitle = x.ImageTitle,
                    CarId = x.CarId
                })
            };

            var result = await _carServices.Update(dto);

            if (result == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index), vm);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var car = await _carServices.GetAsync(id);

            if (car == null)
            {
                return NotFound();
            }

            var photos = await _dbcontext.FileToDatabase
                .Where(x => x.CarId == id)
                .Select(y => new ImageViewModel
                {
                    ImageData = y.ImageData,
                    ImageId = y.Id,
                    Image = string.Format("data:image/gif;base64,{0}", Convert.ToBase64String(y.ImageData)),
                    ImageTitle = y.ImageTitle,
                    CarId = y.Id
                }).ToArrayAsync();

            var vm = new CarViewModel();

            vm.Id = car.Id;
            vm.OwnerName = car.OwnerName;
            vm.NumberOfRegistration = car.NumberOfRegistration;
            vm.VINCode = car.VINCode;
            vm.Brand = car.Brand;
            vm.Model = car.Model;
            vm.Color = car.Color;
            vm.Fuel = car.Fuel;
            vm.Capacity = car.Capacity;
            vm.NumberOfCarDoors = car.NumberOfCarDoors;
            vm.NumberOfPassangersWithDriver = car.NumberOfPassangersWithDriver;
            vm.CarWeight = car.CarWeight;
            vm.BuildOfDate = car.BuildOfDate;
            vm.DateOfRegistration = car.DateOfRegistration;
            vm.Image.AddRange(photos);

            return View("Delete", vm);

        }

        [HttpPost]

        public async Task<IActionResult> DeleteConfirmation(Guid id)
        {
            var product = await _carServices.Delete(id);

            if (product == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveImage(ImageViewModel file)
        {
            var dto = new FileToDatabaseDto()
            {
                Id = file.ImageId
            };

            var image = await _fileServices.RemoveImage(dto);

            if (image == null)
            {
                return RedirectToAction("Edit");
            }

            return RedirectToAction(nameof(Index));
        }
    }
}

 