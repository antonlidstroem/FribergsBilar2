using System;
using System.Diagnostics;
using AutoMapper;
using Fribergs.Core;
using Fribergs.Core.DTO;
using Fribergs.Core.ViewModels;
using MarcusRent.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace MarcusRent.Controllers
{
    public class CarController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ICarApiRepository _carRepository;

        public CarController(IMapper mapper, ICarApiRepository carRepository)
        {
            _mapper = mapper;
            _carRepository = carRepository;
        }

        public async Task<IActionResult> Index()
        {
            var availableCars = await _carRepository.GetCarsAsync();
            var model = _mapper.Map<List<CarViewModel>>(availableCars);
            return View(model);
        }

        // GET: Car/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var car = await _carRepository.GetCarByIdAsync(id.Value);
            if (car == null) return NotFound();

            var model = _mapper.Map<CarViewModel>(car);
            return View(model);
        }

        // GET: Car/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var car = await _carRepository.GetCarByIdAsync(id.Value);
            if (car == null) return NotFound();

            var model = _mapper.Map<CarViewModel>(car);

            if (model.ImageUrls == null)
                model.ImageUrls = new List<string>();

            Console.WriteLine(model.CarId);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromForm] CarViewModel cVModel)
        {

            Console.WriteLine(cVModel.CarId);

            if (cVModel.ImageUrls == null)
                cVModel.ImageUrls = new List<string>();

            if (!ModelState.IsValid)
                return View(cVModel);

            var carDto = _mapper.Map<CarDto>(cVModel);
            var success = await _carRepository.UpdateCarAsync(carDto);

            if (!success)
            {
                ModelState.AddModelError("", "Kunde inte uppdatera bilen.");
                return View(cVModel);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            bool inOrder = await _carRepository.IsCarInAnyOrderAsync(id);

            if (inOrder)
            {
                TempData["ErrorMessage"] = "Bilen kan inte raderas eftersom den finns i en order.";
                return RedirectToAction("Index", "Admin");
            }

            var success = await _carRepository.DeleteCarAsync(id);

            if (!success)
                TempData["ErrorMessage"] = "Kunde inte radera bilen.";
            else
                TempData["SuccessMessage"] = "Bilen raderades.";

            return RedirectToAction("Index", "Admin");
        }
    }
}
