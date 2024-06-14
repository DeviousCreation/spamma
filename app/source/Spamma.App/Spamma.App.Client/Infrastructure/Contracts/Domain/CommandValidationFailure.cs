namespace Spamma.App.Client.Infrastructure.Contracts.Domain;

public record CommandValidationFailure
{
    public required string PropertyName { get; init; }

    public required string ErrorMessage { get; init; }

    public object? AttemptedValue { get; init; }
}