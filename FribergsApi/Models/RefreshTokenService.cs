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
        public async Task<RefreshToken> GenerateRefreshTokenAsync(string userId)
        {
            var refreshToken = new RefreshToken
            {
                UserId = userId,
                Token = Convert.ToBase64String(Guid.NewGuid().ToByteArray()),
                Expires = DateTime.UtcNow.AddDays(7) 
            };

            return refreshToken;
        }

        public async Task SaveRefreshTokenAsync(string userId, RefreshToken refreshToken)
        {
            var existingToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.UserId == userId);

            if (existingToken != null)
            {
                _context.RefreshTokens.Remove(existingToken);
            }
            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();
        }
        public async Task<RefreshToken> GetValidTokenAsync(string refreshToken)
        {
            return await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == refreshToken && rt.Expires > DateTime.UtcNow);
        }
        public async Task RevokeTokenAsync(RefreshToken refreshToken)
        {
            _context.RefreshTokens.Remove(refreshToken);
            await _context.SaveChangesAsync();
        }
    }
}
