using System.Net.Http;
//using Fribergs.Core;
//using Fribergs.Core.DTO;
using MarcusRent.Services.Base;
using MarcusRent.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace MarcusRent.Controllers
{
    public class AccountController : BaseController
    {

        
        private readonly IAuthService _authService;
        private readonly IClient _client;

        public AccountController(IUserApiRepository userApiRepository, IHttpContextAccessor contextAccessor, 
            IAuthService authService, IClient client)
            : base(userApiRepository, contextAccessor)
        {
            _authService = authService;
            _client = client;
        }

        [HttpGet]
        [Route("Login")]
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

        [HttpGet]
        public IActionResult Register()
        {
            return View(new UserDto());
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserDto userDto)
        {
            Fribergs.Core.DebugHelper.DebugModelStatePostCreate(ModelState);
            if (!ModelState.IsValid)
                return View(userDto);

            //if (userDto.Password != userDto.ConfirmPassword)
            //{
            //    ModelState.AddModelError("", "Lösenorden matchar inte");
            //    return View(userDto);
            //}

            try
            {
                //var result = await _authService.RegisterAsync(userDto.Email, userDto.Password);
                await _client.RegisterAsync(userDto);
                return RedirectToAction("Login");
            }
            catch (ApiException aex)
            {
                ViewBag.Error = aex.Response ?? "Kunde inte registrera användare";
                return View(userDto);
            }

            
        }


    }
}
