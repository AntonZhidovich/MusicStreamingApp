namespace MusicService.Domain.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        Task CreateAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task SaveChangesAsync();
        Task<int> CountAsync();
        Task<int> CountAsync(ISpecification<T> specification);
        Task<IEnumerable<T>> ApplySpecificationAsync(ISpecification<T> specification, int currentPage, int pageSize);
    }
}
