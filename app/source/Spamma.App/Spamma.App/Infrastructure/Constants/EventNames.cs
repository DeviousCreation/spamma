namespace Spamma.App.Infrastructure.Constants;

internal static class EventNames
{
    internal const string DomainCreated = "Domain.DomainCreated";
    internal const string UserAllowedAccessToDomain = "Domain.UserAllowedAccessToDomain";
    internal const string UserRevokedAccessToDomain = "Domain.UserRevokedAccessToDomain";
    internal const string UserAccessAlteredAgainstDomain = "Domain.UserAccessAlteredAgainstDomain";
    internal const string NewEmailReceived = "Email.NewEmailReceived";
    internal const string EmailDeleted = "Email.EmailDeleted";
    internal const string SubdomainCreated = "Subdomain.SubdomainCreated";
    internal const string ChaosMonkeyAddressDeleted = "Subdomain.ChaosMonkeyAddressDeleted";
    internal const string ChaosMonkeyAddressCreated = "Subdomain.ChaosMonkeyAddressCreated";
    internal const string ChaosMonkeyAddressUpdated = "Subdomain.ChaosMonkeyAddressUpdated";
    internal const string UserInvited = "User.UserInvited";
    internal const string UserInvitationConfirmed = "User.UserInvitationConfirmed";
    internal const string EmailAddressChangeInitialized = "User.EmailAddressChangeInitialized";
    internal const string EmailAddressChanged = "user.EmailAddressChanged";
    internal const string UserRegistered = "User.UserRegistered";
    internal const string UserVerified = "User.UserVerified";
    internal const string UserDetailsUpdated = "User.UserDetailsUpdated";
    internal const string SignInProcessStarted = "User.SignInProcessStarted";
}