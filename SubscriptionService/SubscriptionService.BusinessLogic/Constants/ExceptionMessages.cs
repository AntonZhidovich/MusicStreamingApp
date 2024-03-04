namespace SubscriptionService.BusinessLogic.Constants
{
    internal static class ExceptionMessages
    {
        public const string tariffPlanNotFound = "No plans was found.";
        public const string userNotFound = "No users was found";
        public const string subscriptionNotFound = "No subscriptions was found.";
        public const string incorrctSubscriptionType = "Provided subscription type is incorrect.";
        public const string subscriptionExists = "The user already has a subscription.";
        public const string validationError = "Validation failure.";
    }
}
