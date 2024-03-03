using Identity.DataAccess.Entities;

namespace Identity.DataAccess.Repositories.Interfaces
{
    public interface ITokenRepository
    {
        Task AddTokenAsync(RefreshToken token, CancellationToken cancellationToken = default);
        Task<RefreshToken?> GetByTokenString(string tokenString, CancellationToken cancellationToken = default);
        Task DeleteTokenAsync(RefreshToken token, CancellationToken cancellationToken = default);
        Task<RefreshToken?> GetTokenByUserIdAsync(string userId, CancellationToken cancellationToken = default);
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
