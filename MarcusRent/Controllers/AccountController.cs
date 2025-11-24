using System.Net.Http;
//using Fribergs.Core;
//using Fribergs.Core.DTO;
using MarcusRent.Services.Base;
using MarcusRent.Interfaces;
using Microsoft.AspNetCore.Mvc;
using MarcusRent.Repositories;
using System.Threading.Tasks;


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
        [Route("login")]
        public IActionResult Login()
        {
            return View(new LoginUserDto());
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginUserDto model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var success = await _authService.LoginAsync(model.Email, model.Password);

            if (!success)
            {
                ViewBag.Error = "Felaktigt användarnamn eller lösenord.";
                return View(model);
            }

            return RedirectToAction("Index", "Home");
        }



        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _authService.LogoutAsync();
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

            try
            {
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
