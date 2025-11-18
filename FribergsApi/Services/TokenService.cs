using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DAL.Classes;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace FribergsApi.Services

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
            // Hämta roller för användaren
            var roles = await _userManager.GetRolesAsync(user);

            // Skapa claims för JWT-token
            var claims = new List<Claim>
            {
                // Motsvarande de gamla claims (Sub, Jti, etc.)
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName ?? ""),  // UserName
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),  // Unique Identifier (Jti)
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),  // Email
                new Claim(ClaimTypes.NameIdentifier, user.Id),  // UserId
                new Claim(ClaimTypes.Name, user.UserName ?? ""),  // UserName
                new Claim("fullName", user.FullName ?? "")  // Full Name (nytt claim)
            };

            // Lägg till användarens roller i claims
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));  // Lägg till roller i token
            }

            // Skapa den hemliga nyckeln för signering
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"])  // Ladda nyckeln från konfiguration
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Skapa JWT-token
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],  // Laddar issuer från konfiguration
                audience: _config["Jwt:Audience"],  // Laddar audience från konfiguration
                claims: claims,  // Lägg till claims
                expires: DateTime.UtcNow.AddDays(1),  // Sätt utgångstid till 1 dag
                signingCredentials: creds  // Lägg till signeringscredentials
            );

            // Returnera den genererade token som en sträng
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
