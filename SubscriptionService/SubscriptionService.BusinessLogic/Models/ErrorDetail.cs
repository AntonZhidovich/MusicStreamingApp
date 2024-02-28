namespace SubscriptionService.BusinessLogic.Models
{
    public class ErrorDetail
    {
        public string Title { get; set; }
        public IEnumerable<string> ErrorMessages { get; set; }

        public ErrorDetail(string title, IEnumerable<string> errorMessages)
        {
            Title = title;
            ErrorMessages = errorMessages;
        }
    }
}
