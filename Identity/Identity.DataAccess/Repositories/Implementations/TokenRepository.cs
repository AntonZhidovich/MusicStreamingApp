using Identity.DataAccess.Data;
using Identity.DataAccess.Entities;
using Identity.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Identity.DataAccess.Repositories.Implementations
{
    public class TokenRepository : ITokenRepository
    {
        private readonly UserDBContext _dbContext;

        public TokenRepository(UserDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddTokenAsync(RefreshToken token)
        {
            await _dbContext.AddAsync(token);
            await SaveChangesAsync();
        }

        public async Task DeleteTokenByUserIdAsync(string userId)
        {
            var token = await GetTokenByUserIdAsync(userId);

            if (token != null)
            {
                _dbContext.Remove(token);
            }

            await SaveChangesAsync();
        }

        public async Task<RefreshToken?> GetByTokenString(string tokenString)
        {
            var token = await _dbContext.RefreshTokens.FirstOrDefaultAsync(t => t.Token == tokenString);

            return token;
        }

        private async Task<RefreshToken?> GetTokenByUserIdAsync(string userId)
        {
            var token = await _dbContext.RefreshTokens.FirstOrDefaultAsync(t => t.UserId == userId);

            return token;
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
