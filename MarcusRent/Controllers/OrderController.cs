using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using MarcusRent.Repositories;
using System.IdentityModel.Tokens.Jwt;
using Fribergs.Core.ViewModels;
using Fribergs.Core.DTO;


namespace MarcusRent.Controllers
{
    public class OrderController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IOrderApiRepository _orderRepository;
        private readonly ICarApiRepository _carRepository;
        private readonly IAuthService _authService;

        public OrderController(IMapper mapper, IOrderApiRepository orderRepository, 
            ICarApiRepository carRepository, IAuthService authService)
        {
            _mapper = mapper;
            _orderRepository = orderRepository;
            _carRepository = carRepository;
            _authService = authService;
        }

        // GET: Order
        public async Task<IActionResult> Index()
        {
            var userId = GetUserIdFromToken();
            if (string.IsNullOrEmpty(userId))
            {
                TempData["TempData"] = "Du måste vara inloggad för att se dina bokningar.";
                return Redirect("/Identity/Account/Login");
            }

            var orders = await _orderRepository.GetOrdersByUserIdAsync(userId);

            var model = _mapper.Map<List<OrderViewModel>>(orders);
            TempData["CarId"] = null;
            return View(model);
        }

        // GET: Order/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var order = await _orderRepository.GetOrderByIdAsync(id.Value);
            if (order == null) return NotFound();

            return View(order);
        }

        // GET: Order/Create
        public async Task<IActionResult> Create(int carId)
        {
            if (!await PrepareCarViewDataAsync(carId))
                return NotFound();

            var car = await _carRepository.GetCarByIdAsync(carId);

            var viewModel = _mapper.Map<OrderViewModel>(car);
            viewModel.StartDate = DateTime.Today;
            viewModel.EndDate = DateTime.Today.AddDays(1);

            TempData["CarId"] = carId;
            return View(viewModel);
        }

        // POST: Order/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                await PrepareCarViewDataAsync(viewModel.CarId);
                return View(viewModel);
            }

            var userId = GetUserIdFromToken();
            if (string.IsNullOrEmpty(userId))
            {
                TempData["TempData"] = "Du måste vara inloggad för att boka.";
                await PrepareCarViewDataAsync(viewModel.CarId);
                return View(viewModel);
            }

            // Validera datum
            var endDateValidationResult = await ValidateEndDateAsync(viewModel);
            if (endDateValidationResult != null)
                return endDateValidationResult;

            // Kontrollera att bilen finns
            var car = await _carRepository.GetCarByIdAsync(viewModel.CarId);
            var carExistValidationResult = await DoesCarExistAsync(viewModel, car);
            if (carExistValidationResult != null)
                return carExistValidationResult;

            // Kontrollera att bilen inte redan är bokad
            var availabilityValidationResult = await ValidateCarAvailabilityAsync(viewModel);
            if (availabilityValidationResult != null)
                return availabilityValidationResult;

            // Beräkna pris och skapa order
            var days = CountDaysDifference(viewModel);
            var order = _mapper.Map<OrderDto>(viewModel);
            order.UserId = userId;
            order.Price = days * car.PricePerDay;

            await _orderRepository.AddOrderAsync(order);

            TempData["TempData"] = "Du har nu bokat bilen!";
            return RedirectToAction("Index", "Car");
        }

        private async Task<IActionResult?> DoesCarExistAsync(OrderViewModel viewModel, CarDto? car)
        {
            if (car == null)
            {
                TempData["TempData"] = "Bilen kunde inte hittas.";
                await PrepareCarViewDataAsync(viewModel.CarId);
                return RedirectToAction("Index", "Order");
            }
            return null;
        }

        // GET: Order/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var order = await _orderRepository.GetOrderByIdAsync(id.Value);
            if (order == null) return NotFound();

            var viewModel = _mapper.Map<OrderViewModel>(order);
            return View(viewModel);
        }

        // POST: Order/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, OrderViewModel viewModel)
        {
            if (id != viewModel.OrderId || !ModelState.IsValid)
                return View(viewModel);

            if (viewModel.StartDate >= viewModel.EndDate)
            {
                ModelState.AddModelError("", "Slutdatum måste vara efter startdatum.");
                return View(viewModel);
            }

            var order = await _orderRepository.GetOrderByIdAsync(viewModel.OrderId);
            if (order == null) return NotFound();

            var car = await _carRepository.GetCarByIdAsync(viewModel.CarId);
            if (car == null)
            {
                ModelState.AddModelError("CarId", "Bilen kunde inte hittas.");
                return View(viewModel);
            }

            _mapper.Map(viewModel, order);
            await _orderRepository.UpdateOrderAsync(order);

            TempData["TempData"] = "Ordern har uppdaterats";
            return RedirectToAction("Index", "Order");
        }

        // POST: Order/Delete/5
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _orderRepository.GetOrderByIdAsync(id);
            if (order == null) return NotFound();

            await _orderRepository.DeleteOrderAsync(id);
            return RedirectToAction("Index", "Order");
        }

        // Uppdaterar ViewBag för bilen
        private async Task<bool> PrepareCarViewDataAsync(int carId)
        {
            var car = await _carRepository.GetCarByIdAsync(carId);
            if (car == null) return false;

            ViewBag.PricePerDay = car.PricePerDay;
            ViewBag.CarInfo = $"{car.Brand} {car.Model} ({car.Year})";
            ViewBag.ImageUrls = car.CarImages.Select(i => i.Url).ToList();

            return true;
        }

        private async Task<IActionResult?> ValidateEndDateAsync(OrderViewModel viewModel)
        {
            if (CountDaysDifference(viewModel) <= 0)
            {
                TempData["TempData"] = "Slutdatum måste vara efter startdatum.";
                await PrepareCarViewDataAsync(viewModel.CarId);
                return View(viewModel);
            }
            return null;
        }

        private int CountDaysDifference(OrderViewModel viewModel)
        {
            return (viewModel.EndDate - viewModel.StartDate).Days;
        }

        private async Task<IActionResult?> ValidateCarAvailabilityAsync(OrderViewModel viewModel)
        {
            var isBooked = await _orderRepository.IsCarBookedAsync(
                viewModel.CarId,
                viewModel.StartDate,
                viewModel.EndDate
            );

            if (isBooked)
            {
                TempData["TempData"] = "Bilen är tyvärr upptagen under denna tidsperiod.";
                await PrepareCarViewDataAsync(viewModel.CarId);
                return View(viewModel);
            }
            return null;
        }

        private string GetUserIdFromToken()
        {
            var token = _authService.GetJwtToken();
            if (string.IsNullOrEmpty(token)) return null;

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);
            var userIdClaim = jwt.Claims.FirstOrDefault(c => c.Type == "id" || c.Type == "sub");
            return userIdClaim?.Value;
        }
    }
}
