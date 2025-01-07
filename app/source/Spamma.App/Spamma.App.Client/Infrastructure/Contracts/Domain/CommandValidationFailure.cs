namespace Spamma.App.Client.Infrastructure.Contracts.Domain;

public record CommandValidationFailure(string ErrorMessage, string? PropertyName = null, object? AttemptedValue = null);