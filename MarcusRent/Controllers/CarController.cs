using System;
using Microsoft.AspNetCore.Mvc;
using MarcusRent.Models;
using AutoMapper;
using MarcusRent.Repositories;
using Fribergs.Core.ViewModels;


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
    }
}


