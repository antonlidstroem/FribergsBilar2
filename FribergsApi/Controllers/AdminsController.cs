using System.Threading.Tasks;
using AutoMapper;
using DAL.Classes;
using DAL.Repositories;
using Fribergs.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MarcusRent.Api.Controllers
{
    [Route("api/admins")]
    [ApiController]
    [Authorize(Roles = "Admin")] 
    public class AdminsController : ControllerBase
    {
        private readonly ICarRepository _carRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IApplicationUserRepository _userService;
        private readonly IMapper _mapper;

        public AdminsController(
            ICarRepository carRepository,
            IOrderRepository orderRepository,
            IApplicationUserRepository userService,
            IMapper mapper)
        {
            _carRepository = carRepository;
            _orderRepository = orderRepository;
            _userService = userService;
            _mapper = mapper;
        }

        #region Cars (CRUD)

        // GET api/admins/cars
        [HttpGet("cars")]
        public async Task<IActionResult> GetCars()
        {
            var cars = await _carRepository.GetAllAsync();
            var carViewModels = _mapper.Map<List<CarViewModel>>(cars);
            return Ok(carViewModels);
        }

        // POST api/admins/cars
        [HttpPost("cars")]
        public async Task<IActionResult> CreateCar([FromBody] CarViewModel carViewModel)
        {
            if (carViewModel == null) return BadRequest("Car data is missing.");

            var car = _mapper.Map<Car>(carViewModel);
            await _carRepository.AddAsync(car);
            return CreatedAtAction(nameof(GetCars), new { id = car.CarId }, car);
        }

        // PUT api/admins/cars/{id}
        [HttpPut("cars/{id}")]
        public async Task<IActionResult> UpdateCar(int id, [FromBody] CarViewModel carViewModel)
        {
            if (carViewModel == null || id != carViewModel.CarId)
                return BadRequest("Car data is invalid.");

            var car = _mapper.Map<Car>(carViewModel);
            await _carRepository.UpdateAsync(car);
            return NoContent();
        }

        // DELETE api/admins/cars/{id}
        [HttpDelete("cars/{id}")]
        public async Task<IActionResult> DeleteCar(int id)
        {
            var car = await _carRepository.GetByIdAsync(id);
            if (car == null) return NotFound("Car not found.");

            await _carRepository.DeleteAsync(id);
            return Ok(new { message = "Car deleted successfully." });
        }

        #endregion

        #region Orders

        // GET api/admins/orders
        [HttpGet("orders")]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _orderRepository.GetAllOrdersAsync();
            var orderViewModels = _mapper.Map<List<OrderViewModel>>(orders);
            return Ok(orderViewModels);
        }

        #endregion

        #region Users (Customers)

        // GET api/admins/users
        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            var customerViewModels = _mapper.Map<List<CustomerViewModel>>(users);
            return Ok(customerViewModels);
        }

        // POST api/admins/users/approve/{userId}
        [HttpPost("users/approve/{userId}")]
        public async Task<IActionResult> ApproveUser(string userId)
        {
            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null) return NotFound("User not found.");

            user.ApprovedByAdmin = true;
            await _userService.UpdateUserAsync(user);
            return Ok(new { message = "User approved successfully." });
        }

        // DELETE api/admins/users/{userId}
        [HttpDelete("users/{userId}")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null) return NotFound("User not found.");

            await _userService.DeleteUserAsync(userId);
            return Ok(new { message = "User deleted successfully." });
        }

        #endregion
    }
}
