using MarcusRent.Interfaces;
using MarcusRent.Services.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class BaseController : Controller
{
    protected readonly IUserApiRepository _userApiRepository;
    protected readonly IHttpContextAccessor _contextAccessor;
    protected readonly IClient _client;

    public BaseController(
        IUserApiRepository userApiRepository, 
        IHttpContextAccessor contextAccessor,
        IClient client)
    {
        _userApiRepository = userApiRepository;
        _contextAccessor = contextAccessor;
        _client = client;
    }

    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var token = _contextAccessor.HttpContext?.Session?.GetString("jwtToken");

        bool isAdmin = false;

        if (!string.IsNullOrEmpty(token))
        {
            try
            {
                var userId = GetUserIdFromToken(token);
                var me = await _client.UsersGETAsync(userId);

                isAdmin = me?.Roles.Contains("Admin") ?? false;
            }
            catch (Exception ex)
            {

                Console.WriteLine($"GetMeAsync failed: {ex.Message}");
            }
        }

        ViewData["IsAdmin"] = isAdmin;

        await next();
    }



    private string GetUserIdFromToken(string token)
    {
        var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        // NameIdentifier är vanlig claim för userId
        var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier);
        return userIdClaim?.Value;
    }


}
