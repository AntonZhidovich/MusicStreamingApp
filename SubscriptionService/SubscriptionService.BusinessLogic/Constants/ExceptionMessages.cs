namespace SubscriptionService.BusinessLogic.Constants
{
    internal static class ExceptionMessages
    {
        public const string TariffPlanNotFound = "No plans was found.";
        public const string SubscriptionNotFound = "No subscriptions was found.";
        public const string IncorrctSubscriptionType = "Provided subscription type is incorrect.";
        public const string SubscriptionExists = "The user already has a subscription with such tariff plan.";
    }
}
