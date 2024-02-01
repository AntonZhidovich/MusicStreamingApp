using Identity.DataAccess.Entities;

namespace Identity.DataAccess.Repositories
{
    public interface ITokenRepository
    {
        Task AddTokenAsync(RefreshToken token);
        Task<RefreshToken> GetByTokenString(string tokenString);
        Task DeleteTokenByUserIdAsync(string userId);
        Task SaveChangesAsync();
    }
}
