using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DAL.Classes;
using DAL.Repositories;
using Fribergs.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MarcusRent.Api.Controllers
{
    [ApiController]
    [Route("api/admins")]
    [Authorize(Roles = "Admin")]
    public class AdminsController : ControllerBase
    {
        private readonly ICarRepository _carRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IApplicationUserRepository _userService;
        private readonly IMapper _mapper;
        private readonly ILogger<AdminsController> _logger;

        public AdminsController(
            ICarRepository carRepository,
            IOrderRepository orderRepository,
            IApplicationUserRepository userService,
            IMapper mapper,
            ILogger<AdminsController> logger)
        {
            _carRepository = carRepository;
            _orderRepository = orderRepository;
            _userService = userService;
            _mapper = mapper;
            _logger = logger;
        }

        #region Cars

        [HttpGet("cars")]
        public async Task<IActionResult> GetCarsAsync()
        {
            try
            {
                var cars = await _carRepository.GetAllAsync();
                if (cars == null || !cars.Any()) return NotFound("No cars found.");

                var carVms = _mapper.Map<List<CarViewModel>>(cars);

                foreach (var carVm in carVms)
                {
                    carVm.TotalEarnings = await _orderRepository.GetTotalEarningsForCarAsync(carVm.CarId);

                    var currentRental = (await _orderRepository.GetOrdersByCarIdAsync(carVm.CarId))
                    .FirstOrDefault(o => o.EndDate >= DateTime.Today);
                    if (currentRental != null)
                    {
                        carVm.CurrentRentalEndDate = currentRental.EndDate;
                        carVm.CurrentCustomerName = currentRental.Customer?.FullName;  
                        carVm.Available = false;
                    }

                }

                return Ok(carVms);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching cars for admin");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("cars")]
        public async Task<IActionResult> CreateCarAsync([FromBody] CarViewModel carVm)
        {
            if (carVm == null) return BadRequest("Car data is missing.");
            var car = _mapper.Map<Car>(carVm);
            await _carRepository.AddAsync(car);
            return CreatedAtAction(nameof(GetCarsAsync), new { id = car.CarId }, car);
        }

        [HttpPut("cars/{id}")]
        public async Task<IActionResult> UpdateCarAsync(int id, [FromBody] CarViewModel carVm)
        {
            if (carVm == null || id != carVm.CarId) return BadRequest("Invalid car data.");
            var car = _mapper.Map<Car>(carVm);
            await _carRepository.UpdateAsync(car);
            return NoContent();
        }

        [HttpDelete("cars/{id}")]
        public async Task<IActionResult> DeleteCarAsync(int id)
        {
            var car = await _carRepository.GetByIdAsync(id);
            if (car == null) return NotFound("Car not found.");
            await _carRepository.DeleteAsync(id);
            return Ok(new { message = "Car deleted successfully." });
        }

        #endregion

        #region Orders

        [HttpGet("orders")]
        public async Task<IActionResult> GetOrdersAsync()
        {
            try
            {
                var orders = await _orderRepository.GetAllOrdersAsync();
                var orderVms = _mapper.Map<List<OrderViewModel>>(orders);
                return Ok(orderVms);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching orders for admin");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("orders/{id}")]
        public async Task<IActionResult> GetOrderByIdAsync(int id)
        {
            var order = await _orderRepository.GetOrderByIdAsync(id);
            if (order == null) return NotFound("Order not found.");
            var orderVm = _mapper.Map<OrderViewModel>(order);
            return Ok(orderVm);
        }

        #endregion

        #region Users (Customers)

        [HttpGet("users")]
        public async Task<IActionResult> GetUsersAsync()
        {
            var users = await _userService.GetAllUsersAsync();
            if (users == null || !users.Any()) return NotFound("No users found.");
            var userVms = _mapper.Map<List<CustomerViewModel>>(users);
            return Ok(userVms);
        }

        [HttpGet("users/{id}")]
        public async Task<IActionResult> GetUserByIdAsync(string id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound("User not found.");
            var userVm = _mapper.Map<CustomerViewModel>(user);
            return Ok(userVm);
        }

        [HttpPost("users/approve/{id}")]
        public async Task<IActionResult> ApproveUserAsync(string id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound("User not found.");

            user.ApprovedByAdmin = true;
            await _userService.UpdateUserAsync(user);

            return Ok(new { message = "User approved successfully." });
        }

        [HttpPut("users/{id}")]
        public async Task<IActionResult> UpdateUserAsync(string id, [FromBody] CustomerViewModel userVm)
        {
            if (userVm == null || id != userVm.UserId) return BadRequest("Invalid user data.");
            var user = _mapper.Map<ApplicationUser>(userVm);
            await _userService.UpdateUserAsync(user);
            return NoContent();
        }

        [HttpDelete("users/{id}")]
        public async Task<IActionResult> DeleteUserAsync(string id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound("User not found.");
            await _userService.DeleteUserAsync(id);
            return Ok(new { message = "User deleted successfully." });
        }

        #endregion
    }
}
