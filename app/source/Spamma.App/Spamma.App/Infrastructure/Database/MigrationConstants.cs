namespace Spamma.App.Infrastructure.Database;

internal static class MigrationConstants
{
    internal static class Tables
    {
        internal const string Domain = "domain";
        internal const string Email = "email";
        internal const string Subdomain = "subdomain";
        internal const string User = "user";
        internal const string DomainAccessPolicy = "domain_access_policy";
        internal const string ChaosMonkeyAddress = "chaos_monkey_address";
        internal const string SubdomainAccessPolicy = "subdomain_access_policy";
        internal const string RecordedUserEvent = "recorded_user_event";
    }
}