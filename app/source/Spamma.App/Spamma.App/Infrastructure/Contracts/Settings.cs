namespace Spamma.App.Infrastructure.Contracts;

internal class Settings
{
    public string SigningKeyBase64 { get; init; } = string.Empty;

    public string BaseUri { get; init; } = string.Empty;

    public string LoginUri => string.Concat(this.BaseUri, "/logged-in?token={0}");

    public string RegistrationUri => string.Concat(this.BaseUri, "/registered?token={0}");
}