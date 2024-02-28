namespace SubscriptionService.BusinessLogic.Models
{
    public class GetPageRequest
    {
        private const int _maxPageSize = 30;
        private int _pageNumber = 1;
        private int _pageSize = 10;

        public int CurrentPage
        {
            get => _pageNumber;
            set => _pageNumber = Math.Max(1, value);
        }

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = Math.Min(_maxPageSize, Math.Max(value, 0));
        }
    }
}
