using MarcusRent.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class BaseController : Controller
{
    protected readonly IUserApiRepository _userApiRepository;
    protected readonly IHttpContextAccessor _contextAccessor;

    public BaseController(IUserApiRepository userApiRepository, IHttpContextAccessor contextAccessor)
    {
        _userApiRepository = userApiRepository;
        _contextAccessor = contextAccessor;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {


        var token = _contextAccessor.HttpContext?.Session?.GetString("jwtToken");
        
        bool isAdmin = false;

        if (!string.IsNullOrEmpty(token))
        {
            try
            {
                var me = _userApiRepository.GetMeAsync(token).GetAwaiter().GetResult();
                isAdmin = me?.Roles.Contains("Admin") ?? false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetMeAsync failed: {ex.Message}");
            }
        }

       
        //ViewData["IsAdmin"] = isAdmin;

        base.OnActionExecuting(context);
    }
    
}
