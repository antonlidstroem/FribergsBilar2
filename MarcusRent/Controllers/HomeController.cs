using System.Diagnostics;
using System.Threading.Tasks;
using Fribergs.Core.ViewModels;
using MarcusRent.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MarcusRent.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;


        public HomeController(ILogger<HomeController> logger, IUserApiRepository userApiRepository, 
            IHttpContextAccessor contextAccessor)
            : base(userApiRepository, contextAccessor) 
        {
            _logger = logger;
            
        }
        public async Task<IActionResult> Index()
        {        
                TempData["CarId"] = null;
           
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
