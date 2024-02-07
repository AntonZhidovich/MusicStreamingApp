namespace MusicService.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IAuthorRepository Authors { get; }
        IGenreRepository Genres { get; }
        ISongRepository Songs {  get; }
        IUserRepository Users { get; }

        Task CommitAsync();
    }
}
