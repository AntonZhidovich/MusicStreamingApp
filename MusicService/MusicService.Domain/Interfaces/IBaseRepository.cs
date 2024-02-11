namespace MusicService.Domain.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        Task CreateAsync(T entity, CancellationToken cancellationToken = default);
        void Update(T entity);
        void Delete(T entity);
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
        Task<int> CountAsync(CancellationToken cancellationToken = default);
        Task<int> CountAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);
        Task<IEnumerable<T>> ApplySpecificationAsync(ISpecification<T> specification, int currentPage, int pageSize, CancellationToken cancellationToken = default);
    }
}
