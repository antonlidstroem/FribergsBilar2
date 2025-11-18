using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using DAL.Classes;
using Microsoft.EntityFrameworkCore;

namespace FribergsApi.Models
{
    public class RefreshTokenService
    {
        private readonly ApplicationDbContext _context;

        public RefreshTokenService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Generera en ny refresh token
        public async Task<RefreshToken> GenerateRefreshTokenAsync(string userId)
        {
            var refreshToken = new RefreshToken
            {
                UserId = userId,
                Token = Convert.ToBase64String(Guid.NewGuid().ToByteArray()),
                Expires = DateTime.UtcNow.AddDays(7) // Exempel på utgångstid
            };

            return refreshToken;
        }

        // Spara refresh token för användaren i databasen
        public async Task SaveRefreshTokenAsync(string userId, RefreshToken refreshToken)
        {
            // Hitta om det finns en gammal refresh token för användaren
            var existingToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.UserId == userId);

            if (existingToken != null)
            {
                // Ta bort den gamla refresh token
                _context.RefreshTokens.Remove(existingToken);
            }

            // Lägg till den nya refresh token
            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();
        }

        // Hämta en giltig refresh token
        public async Task<RefreshToken> GetValidTokenAsync(string refreshToken)
        {
            return await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == refreshToken && rt.Expires > DateTime.UtcNow);
        }

        // Återkalla (ta bort) en refresh token
        public async Task RevokeTokenAsync(RefreshToken refreshToken)
        {
            _context.RefreshTokens.Remove(refreshToken);
            await _context.SaveChangesAsync();
        }
    }


}
