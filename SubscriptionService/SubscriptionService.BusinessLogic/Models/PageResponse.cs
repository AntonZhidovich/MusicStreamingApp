namespace SubscriptionService.BusinessLogic.Models
{
    public class PageResponse<TEntity>
    {
        public int PagesCount { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public IEnumerable<TEntity> Items { get; set; } = null!;
    }
}
