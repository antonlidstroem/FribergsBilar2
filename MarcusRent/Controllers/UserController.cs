using Microsoft.AspNetCore.Mvc;
//using DAL.Classes;
using MarcusRent.Repositories;
using MarcusRent.Models;



namespace MarcusRent.Controllers
{
    public class UserController : BaseController
    {
        //private readonly UserManager<ApplicationUser> _userManager;
        //private readonly IOrderRepository _orderRepository;
        private readonly UserApiRepository _userApiRepository;
        private readonly IHttpContextAccessor _contextAccessor;



        public UserController(UserApiRepository userApiRepository, IHttpContextAccessor contextAccessor)
            : base(userApiRepository, contextAccessor)
        {
            {
                _userApiRepository = userApiRepository;
                _contextAccessor = contextAccessor;
                //_userManager = userManager;
                //_orderRepository = orderRepository;
            }
        }

        // GET: Users/Edit/id
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userApiRepository.GetUserByIdAsync(id);
            if (user == null) return NotFound();

            TempData["CarId"] = null;

            return View(user); // skapa en vy för detta
        }

        // POST: Users/Edit/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserDtoClient user)
        {

            TempData["CarId"] = null;

            var success = await _userApiRepository.UpdateUserAsync(user);
            if (!success)
            {
                ModelState.AddModelError("", "Det gick inte att uppdatera användaren.");
                return View(user);
            }

            return RedirectToAction("Index", "Admin");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var success = await _userApiRepository.DeleteUserAsync(id);
            if (!success)
            {
                TempData["ErrorMessage"] = "Det gick inte att ta bort användaren.";
            }

            return RedirectToAction("Index", "Admin");
        }

    }
}


    

   


