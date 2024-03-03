namespace SubscriptionService.DataAccess.Entities
{
    public class TariffPlan
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int MaxPlaylistsCount { get; set; }
        public double MonthFee { get; set; }
        public double AnnualFee { get; set; }
        public List<Subscription> Subscriptions { get; set; }
    }
}
