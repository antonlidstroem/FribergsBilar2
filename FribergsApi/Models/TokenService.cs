using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DAL.Classes;
using Fribergs.Core.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace FribergsApi.Models
{
    public class TokenService
    {
        private readonly IConfiguration _config;
        private readonly UserManager<ApplicationUser> _userManager;

        public TokenService(IConfiguration config, UserManager<ApplicationUser> userManager)
        {
            _config = config;
            _userManager = userManager;
        }

        public async Task<string> GenerateAccessToken(ApplicationUser user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = roles.Select(q => new Claim(ClaimTypes.Role, q)).ToList();

            var userClaims = await _userManager.GetClaimsAsync(user);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName ?? ""),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
                new Claim(CustomClaimTypes.Uid, user.Id)
                // new Claim("firstName", user.FirstName?? ""),
                //new Claim("lastName", user.LastName ?? "")
            }.Union(roleClaims)
            .Union(userClaims);

            var token = new JwtSecurityToken(
                issuer: _config["JwtSettings:Issuer"],
                audience: _config["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(_config["JwtSetings:DurationInMinutes"])),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);

            //var claims = new List<Claim>
            //{
            //    new Claim(JwtRegisteredClaimNames.Sub, user.UserName ?? ""),  
            //    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            //    new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""), 
            //    new Claim(ClaimTypes.NameIdentifier, user.Id), 
            //    new Claim(ClaimTypes.Name, user.UserName ?? ""),  
            //    new Claim("firstName", user.FirstName?? ""), 
            //    new Claim("lastName", user.LastName ?? "")
            //};

            

            //foreach (var role in roles)
            //{
            //    claims.Add(new Claim(ClaimTypes.Role, role));  
            //}

            //var key = new SymmetricSecurityKey(
            //    Encoding.UTF8.GetBytes(_config["Jwt:Key"])  
            //);

            //var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //var token = new JwtSecurityToken(
            //    issuer: _config["Jwt:Issuer"], 
            //    audience: _config["Jwt:Audience"], 
            //    claims: claims,  
            //    expires: DateTime.UtcNow.AddDays(1), 
            //    signingCredentials: creds  
            //);
            //return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
