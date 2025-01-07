namespace Spamma.App.Client.Infrastructure.Constants;

public enum ErrorCodes
{
    NotAuthorized = -2,
    NotAuthenticated = -1,
    Unknown = 0,
    CommunicationError,
    SavingChanges,
    ChaosMonkeyAddressAlreadyExists,
    ChaosMonkeyAddressNotFound,
    NotFound,
    DomainAccessPolicyAlreadyRevoked,
    DomainAccessPolicyAlreadyExists,
    DomainAccessPolicyNotFound,
    SubdomainAccessPolicyNotFound,
    SubdomainAccessPolicyAlreadyExists,
    TokenNotValid,
    DomainAlreadyDisabled,
    CurrentUserNotFound,
    UserAlreadyVerified,
    SettingValueNotChanged
}