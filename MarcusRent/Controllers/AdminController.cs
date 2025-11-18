using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using AutoMapper;
using Fribergs.Core.ViewModels;
using MarcusRent.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace MarcusRent.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ICarApiRepository _carRepository;
        private readonly IOrderApiRepository _orderRepository;
        private readonly IUserApiRepository _userService;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public AdminController(
            ICarApiRepository carRepository,
            IOrderApiRepository orderRepository,
            IUserApiRepository userService,
            IMapper mapper, IAuthService authService)
        {
            _carRepository = carRepository;
            _orderRepository = orderRepository;
            _userService = userService;
            _mapper = mapper;
            _authService = authService;
        }


        public async Task<IActionResult> Index()
        {
            var userId = GetUserIdFromToken();
            if (string.IsNullOrEmpty(userId))
            {
                TempData["TempData"] = "Du måste vara inloggad för att se dina bokningar.";
                return Redirect("/Identity/Account/Login");
            }

            TempData["CarId"] = null;
            var cars = await _carRepository.GetCarsAsync();
            var orders = await _orderRepository.GetOrdersAsync();
            var users = await _userService.GetAllUsersAsync();

            if (users == null)
            {

                throw new Exception("GetAllUsersAsync() returnerade null");
            }
            else
            {
                Console.WriteLine($"Typ av users: {users.FirstOrDefault()?.GetType().Name}");
            }
           
            // Mappa Order -> AdminOrderViewModel
            var orderViewModels = _mapper.Map<List<OrderViewModel>>(orders);
            var customerViewModels = _mapper.Map<List<CustomerViewModel>>(users);
            var carViewModels = _mapper.Map<List<CarViewModel>>(cars);

            foreach (var carVM in carViewModels)
            {
                var earnings = await _orderRepository.GetTotalEarningsForCarAsync(carVM.CarId);
                var activeRental = orders
                    .FirstOrDefault(o => o.CarId == carVM.CarId && o.EndDate > DateTime.Now);

                carVM.TotalEarnings = earnings;
                carVM.CurrentRentalEndDate = activeRental?.EndDate;
                carVM.CurrentCustomerName = activeRental?.UserId;
            }

            

            


            // Skapa AdminDashboardViewModel
            var vm = new AdminDashboardViewModel
            {
                Cars = carViewModels,
                Orders = orderViewModels,
                Customers = customerViewModels
            };

            return View(vm);

        }

        [HttpPost]
        public async Task<IActionResult> ApproveCustomer(string id)
        {
            await _userService.ApproveUserAsync(id);
            return RedirectToAction("Index");
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
