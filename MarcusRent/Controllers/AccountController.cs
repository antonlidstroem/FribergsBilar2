using Microsoft.AspNetCore.Mvc;
using Fribergs.Core.DTO;
using MarcusRent.Interfaces;


namespace MarcusRent.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IUserApiRepository _userApiRepository;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IAuthService _authService;

        public AccountController(IUserApiRepository userApiRepository, IHttpContextAccessor contextAccessor, IAuthService authService)
            : base(userApiRepository, contextAccessor)
        {
            _userApiRepository = userApiRepository;
            _contextAccessor = contextAccessor;
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginUserDto());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginUserDto model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var loginSuccess = await _authService.LoginAsync(model.Email, model.Password);
            HttpContext.Session.SetString("userEmail", model.Email);


            if (!loginSuccess)
            {
                ViewBag.Error = "Felaktigt användarnamn eller lösenord.";
                return View(model);
            }

            var token = _authService.GetJwtToken();
            HttpContext.Session.SetString("jwtToken", token);
            HttpContext.Session.SetString("userName", model.Email);

            return RedirectToAction("Index", "Home");
        }



        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
