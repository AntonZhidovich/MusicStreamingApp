namespace MusicService.Application.Interfaces
{
    public interface IUserServiceGrpcClient
    {
        Task<bool> UserIsInRoleAsync(string id, string roleName, CancellationToken cancellationToken = default);
    }
}
