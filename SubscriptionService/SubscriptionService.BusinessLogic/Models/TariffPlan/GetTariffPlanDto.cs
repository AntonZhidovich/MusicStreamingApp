namespace SubscriptionService.BusinessLogic.Models.TariffPlan
{
    public class GetTariffPlanDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int MaxPlaylistsCount { get; set; }
        public double MonthFee { get; set; }
        public double AnnualFee { get; set; }
    }
}
