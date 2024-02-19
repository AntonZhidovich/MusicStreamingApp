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

        public async Task AddTokenAsync(RefreshToken token, CancellationToken cancellationToken = default)
        {
            await _dbContext.AddAsync(token);
            
            await SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteTokenAsync(RefreshToken token, CancellationToken cancellationToken = default)
        {
            _dbContext.Remove(token);

            await SaveChangesAsync(cancellationToken);
        }

        public async Task<RefreshToken?> GetByTokenString(string tokenString, CancellationToken cancellationToken = default)
        {
            var token = await _dbContext.RefreshTokens
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Token == tokenString, cancellationToken);

            return token;
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<RefreshToken?> GetTokenByUserIdAsync(string userId, CancellationToken cancellationToken = default)
        {
            var token = await _dbContext.RefreshTokens
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.UserId == userId, cancellationToken);

            return token;
        }
    }
}
