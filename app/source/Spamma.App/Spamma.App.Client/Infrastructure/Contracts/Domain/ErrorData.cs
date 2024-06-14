using Spamma.App.Client.Infrastructure.Constants;

namespace Spamma.App.Client.Infrastructure.Contracts.Domain;

public sealed class ErrorData(ErrorCode code, string message)
{
    public ErrorData(ErrorCode code)
        : this(code, code.ToString())
    {
    }

    public ErrorCode Code { get; } = code;

    public string Message { get; } = message;
}