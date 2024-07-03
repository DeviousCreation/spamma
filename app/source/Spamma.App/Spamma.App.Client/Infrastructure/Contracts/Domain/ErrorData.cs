using Spamma.App.Client.Infrastructure.Constants;

namespace Spamma.App.Client.Infrastructure.Contracts.Domain;

public sealed class ErrorData(ErrorCodes codes, string message)
{
    public ErrorData(ErrorCodes codes)
        : this(codes, codes.ToString())
    {
    }

    public ErrorCodes Codes { get; } = codes;

    public string Message { get; } = message;
}