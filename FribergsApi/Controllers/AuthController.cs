using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DAL.Classes;
using Fribergs.Core.DTO;
using FribergsApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace FribergsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly IConfiguration _configuration;
        private readonly TokenService _tokenService;
        private readonly RefreshTokenService _refreshTokenService;

        public AuthController(UserService userService, IConfiguration configuration,
            TokenService tokenService,
            RefreshTokenService refreshTokenService            )
        {
            _userService = userService;
            _configuration = configuration;
            _refreshTokenService = refreshTokenService;
            _tokenService = tokenService;
        }

        // ---------------------------
        // REGISTER
        // ---------------------------
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] LoginUserDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userService.RegisterAsync(
                firstName: "",
                lastName: "",
                email: model.Email,
                password: model.Password,
                role: "User"
            );

            if (user == null)
                return BadRequest("User already exists or could not be created.");

            var token = await _tokenService.GenerateAccessToken(user);

            return Ok(new AuthResponse
            {
                UserId = user.Id,
                Token = token,
                Email = user.Email
            });
        }

        // ---------------------------
        // LOGIN
        // ---------------------------
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userService.ValidateUserAsync(model.Email, model.Password);
            if (user == null)
                return Unauthorized("Invalid email or password.");

            var token = await _tokenService.GenerateAccessToken(user);

            // Skapa och spara refresh token
            var refreshToken = await _refreshTokenService.GenerateRefreshTokenAsync(user.Id);
            await _refreshTokenService.SaveRefreshTokenAsync(user.Id, refreshToken); // Spara i den separata refresh token-tabellen

            return Ok(new AuthResponse
            {
                UserId = user.Id,
                Token = token,
                RefreshToken = refreshToken.Token, // Returnera den nya refresh token
                Email = user.Email
            });
        }


        //// ---------------------------
        //// GENERATE JWT TOKEN (privat)
        //// ---------------------------
        //private async Task<string> GenerateJwtToken(ApplicationUser user)
        //{
        //    var roles = await _userService.GetRolesAsync(user);

        //    var claims = new List<Claim>
        //    {
        //        new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
        //        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        //        new Claim(JwtRegisteredClaimNames.Email, user.Email),
        //        new Claim(ClaimTypes.NameIdentifier, user.Id)
        //    };


        //    foreach (var role in roles)
        //        claims.Add(new Claim(ClaimTypes.Role, role));

        //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        //    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        //    var token = new JwtSecurityToken(
        //        issuer: _configuration["Jwt:Issuer"],
        //        audience: _configuration["Jwt:Audience"],
        //        claims: claims,
        //        expires: DateTime.Now.AddDays(1),
        //        signingCredentials: creds
        //    );

        //    return new JwtSecurityTokenHandler().WriteToken(token);
        //}

        // ---------------------------
        // GET CURRENT USER ("me")
        // ---------------------------
        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetMe()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            if (email == null) return Unauthorized();

            var user = await _userService.GetUserByEmailAsync(email);
            if (user == null) return NotFound();

            var roles = await _userService.GetRolesAsync(user);

            return Ok(new
            {
                user.Email,
                Roles = roles
            });
        }
        // ---------------------------
        // REFRESH TOKEN
        // ---------------------------
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto model)
        {
            if (string.IsNullOrEmpty(model.RefreshToken))
            {
                return BadRequest("Refresh token is required.");
            }

            // Hämta och validera refresh token
            var refreshToken = await _refreshTokenService.GetValidTokenAsync(model.RefreshToken);
            if (refreshToken == null)
            {
                return Unauthorized("Invalid or expired refresh token.");
            }

            // Hämta användaren baserat på refresh token
            var user = await _userService.GetUserByIdAsync(refreshToken.UserId);
            if (user == null)
            {
                return Unauthorized("User not found.");
            }

            // Generera ett nytt access token
            var newAccessToken = await _tokenService.GenerateAccessToken(user);

            // Återkalla den gamla refresh token och skapa en ny
            await _refreshTokenService.RevokeTokenAsync(refreshToken);
            var newRefreshToken = await _refreshTokenService.GenerateRefreshTokenAsync(user.Id);

            return Ok(new
            {
                accessToken = newAccessToken,
                refreshToken = newRefreshToken.Token
            });
        }



    }
}
