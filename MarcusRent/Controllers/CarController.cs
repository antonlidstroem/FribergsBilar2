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

            // Säkerställ att ImageUrls-listan alltid finns
            if (model.ImageUrls == null)
                model.ImageUrls = new List<string>();

            // Lägg till tomma strängar så att det alltid finns minst 3 platser
            while (model.ImageUrls.Count < 3)
            {
                model.ImageUrls.Add(string.Empty);
            }

            Console.WriteLine(model.CarId);

            return View(model);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int CarId, [FromForm] CarViewModel model)
        public async Task<IActionResult> Edit([FromForm] CarViewModel cVModel)
        {
            DebugHelper.DebugModelStatePostCreate(ModelState);

            //if (CarId != model.CarId) return BadRequest();

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
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(CarViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //        return View(model);

        //    var carDto = _mapper.Map<CarDto>(model);
        //    var success = await _carRepository.UpdateCarAsync(carDto);

        //    if (!success)
        //    {
        //        ModelState.AddModelError("", "Kunde inte uppdatera bilen.");
        //        return View(model);
        //    }

        //    return RedirectToAction(nameof(Index));
        //}

        //public async Task<IActionResult> Delete(int id)
        //{
        //    var isInOrder = await _carRepository.IsCarInAnyOrderAsync(id);

        //    if (isInOrder)
        //    {
        //        TempData["Error"] = "Bilen kan inte raderas eftersom den finns i en order.";
        //        return RedirectToAction(nameof(Index));
        //    }

        //    var success = await _carRepository.DeleteCarAsync(id);

        //    if (!success)
        //    {
        //        TempData["Error"] = "Kunde inte radera bilen.";
        //    }

        //    return RedirectToAction(nameof(Index));
        //}


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
