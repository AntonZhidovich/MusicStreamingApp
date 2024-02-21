namespace SubscriptionService.BusinessLogic.Models.TariffPlan
{
    public class UpdateTariffPlanDto
    {
        public string? Name { get; set; } = null;
        public string? Description { get; set; } = null;
        public int? MaxPlaylistsCount { get; set; } = null;
        public double? MonthFee { get; set; } = null;
        public double? AnnualFee { get; set; } = null;
    }
}
