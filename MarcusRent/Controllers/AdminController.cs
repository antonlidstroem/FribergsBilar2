using AutoMapper;
using Fribergs.Core.ViewModels;
using MarcusRent.Models;
using MarcusRent.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace MarcusRent.Controllers
{
    
    public class AdminController : Controller
    {
        private readonly ICarApiRepository _carRepository;
        private readonly IOrderApiRepository _orderRepository;
        private readonly IUserApiRepository _userService;
        private readonly IMapper _mapper;

        public AdminController(
            ICarApiRepository carRepository,
            IOrderApiRepository orderRepository,
            IUserApiRepository userService,
            IMapper mapper)
        {
            _carRepository = carRepository;
            _orderRepository = orderRepository;
            _userService = userService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = await _userService.GetCurrentUserAsync();
            if (currentUser == null || !currentUser.Roles.Contains("Admin"))
            {
                return RedirectToAction("Index", "Home");
            }

            TempData["CarId"] = null;
            var cars = await _carRepository.GetCarsAsync();
            var orders = await _orderRepository.GetOrdersAsync();
            var users = await _userService.GetAllUsersAsync();

            // Map Car -> AdminCarViewModel
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

            // Map Order -> AdminOrderViewModel
            var orderViewModels = _mapper.Map<List<OrderViewModel>>(orders);

            // Map ApplicationUser -> AdminCustomerViewModel
            var customerViewModels = _mapper.Map<List<CustomerViewModel>>(users);

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
    }
}