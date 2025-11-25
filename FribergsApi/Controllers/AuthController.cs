using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DAL.Classes;
using Fribergs.Core.DTO;
using FribergsApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Fribergs.Core.Constants;

namespace FribergsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        //private readonly UserService _userService;
        private readonly IConfiguration _configuration;
        private readonly TokenService _tokenService;
        private readonly RefreshTokenService _refreshTokenService;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthController(UserManager<ApplicationUser> userManager,
            IConfiguration configuration,
            TokenService tokenService,
            RefreshTokenService refreshTokenService)
        {
           
            _configuration = configuration;
            _refreshTokenService = refreshTokenService;
            _tokenService = tokenService;
            _userManager = userManager;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] UserDto userDto)
        {
            try
            {
                ApplicationUser user = new ApplicationUser()
                {
                    UserName = userDto.Email,
                    Email = userDto.Email,
                    FirstName = userDto.FirstName,
                    LastName = userDto.LastName,
                };
                var result = await _userManager.CreateAsync(user, userDto.Password);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                    return BadRequest(ModelState);
                }

                await _userManager.AddToRoleAsync(user, ApiRoles.User);

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
            catch (Exception ex)
            {
                return Problem($"Something went wrong in the {nameof(Register)}", statusCode: 500);
            }

        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<AuthResponse>> Login(LoginUserDto userDto)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(userDto.Email);
                if(user == null)
                {
                    return Unauthorized();
                }

               
                var passwordValid = await _userManager.CheckPasswordAsync(user, userDto.Password);

                if (!passwordValid)
                {
                    return Unauthorized();
                }

                var roles = await _userManager.GetRolesAsync(user);

                

                var token = await _tokenService.GenerateAccessToken(user);
                var refreshToken = await _refreshTokenService.GenerateRefreshTokenAsync(user.Id);
                await _refreshTokenService.SaveRefreshTokenAsync(user.Id, refreshToken);

                var response = new AuthResponse
                {
                    UserId = user.Id,
                    Token = token,
                    RefreshToken = refreshToken.Token,
                    Email = user.Email,
                    Roles = roles.ToList()

                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return Problem($"Something went wrong in the {nameof(Login)}", statusCode: 500);
            }
        }

        //[Authorize]
        //[HttpGet("me")]
        //public async Task<IActionResult> GetMe()
        //{
        //    var email = User.FindFirst(ClaimTypes.Email)?.Value;
        //    if (email == null) return Unauthorized();

        //    var user = await _userManager.FindByEmailAsync(email);
        //    if (user == null) return NotFound();

        //    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //    if (userId == null) return Unauthorized();

        //    var roles = await _userManager.GetRolesAsync(user);

        //    var dto = new UserDto
        //    {
        //        UserId = userId,
        //        Email = user.Email,
        //        Roles = roles.ToList(),
        //        //FullName = $"{user.FirstName} {user.LastName}",
        //        FirstName = user.FirstName,
        //        LastName = user.LastName,
        //        ApprovedByAdmin = user.ApprovedByAdmin

        //    };
        //    return Ok(dto);
        //}

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


            var user = await _userManager.FindByIdAsync(refreshToken.UserId);

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
