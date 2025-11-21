using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DAL.Classes;
using Fribergs.Core.DTO;
using FribergsApi.Models;
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
            RefreshTokenService refreshTokenService)
        {
            _userService = userService;
            _configuration = configuration;
            _refreshTokenService = refreshTokenService;
            _tokenService = tokenService;
        }

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

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userService.ValidateUserAsync(model.Email, model.Password);
            if (user == null)
                return Unauthorized("Invalid email or password.");

            var token = await _tokenService.GenerateAccessToken(user);


            var refreshToken = await _refreshTokenService.GenerateRefreshTokenAsync(user.Id);
            await _refreshTokenService.SaveRefreshTokenAsync(user.Id, refreshToken);

            return Ok(new AuthResponse
            {
                UserId = user.Id,
                Token = token,
                RefreshToken = refreshToken.Token,
                Email = user.Email
            });
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetMe()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            if (email == null) return Unauthorized();

            var user = await _userService.GetUserByEmailAsync(email);
            if (user == null) return NotFound();

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();

            var roles = await _userService.GetRolesAsync(user);

            var dto = new UserDto
            {
                UserId = userId,
                Email = user.Email,
                Roles = roles.ToList(),
                FullName = $"{user.FirstName} {user.LastName}",
                ApprovedByAdmin = user.ApprovedByAdmin

            };
            return Ok(dto);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto model)
        {
            if (string.IsNullOrEmpty(model.RefreshToken))
            {
                return BadRequest("Refresh token is required.");
            }

            var refreshToken = await _refreshTokenService.GetValidTokenAsync(model.RefreshToken);
            if (refreshToken == null)
            {
                return Unauthorized("Invalid or expired refresh token.");
            }

            var user = await _userService.GetUserByIdAsync(refreshToken.UserId);
            if (user == null)
            {
                return Unauthorized("User not found.");
            }

            var newAccessToken = await _tokenService.GenerateAccessToken(user);

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
