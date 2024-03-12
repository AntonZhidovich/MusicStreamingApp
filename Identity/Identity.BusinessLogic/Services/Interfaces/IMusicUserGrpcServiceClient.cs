using Identity.DataAccess.Entities;

namespace Identity.BusinessLogic.Services.Interfaces
{
    public interface IMusicUserGrpcServiceClient
    {
        Task AddUserAsync(User user, CancellationToken cancellationToken = default);
    }
}
