using Microsoft.AspNetCore.Mvc;
using MarcusRent.Repositories;
using AutoMapper;
using Fribergs.Core.ViewModels;
using Fribergs.Core.DTO;
using Fribergs.Core;

namespace MarcusRent.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserApiRepository _userApi;
        private readonly IMapper _mapper;

        public UserController(IUserApiRepository userApi, IMapper mapper)
        {
            _userApi = userApi;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            DebugHelper.DebugModelStatePostCreate(ModelState);

            var user = await _userApi.GetUserByIdAsync(id);
            if (user == null)
                return NotFound();

            var vm = _mapper.Map<CustomerViewModel>(user);
            vm.UserId = id;

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("UserId,FullName,Email,ApprovedByAdmin")] CustomerViewModel vm)
        {
            DebugHelper.DebugModelStatePostCreate(ModelState);

            if (!ModelState.IsValid)
                return View(vm);

            var dto = _mapper.Map<UserDto>(vm);

            var result = await _userApi.UpdateUserAsync(dto);
            if (!result)
            {
                TempData["ErrorMessage"] = "Kunde inte uppdatera användaren.";
                return View(vm);
            }

            TempData["TempData"] = "Användaren har uppdaterats.";
            return RedirectToAction("Index", "Admin");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(string userId)
        {
            var result = await _userApi.DeleteUserAsync(userId);

            if (!result)
            {
                TempData["ErrorMessage"] = "Kunde inte radera användaren. Användaren har kopplade ordrar.";
                return RedirectToAction("Index", "Admin");
            }

            TempData["TempData"] = "Användare raderad.";
            return RedirectToAction("Index", "Admin");
        }
    }
}
