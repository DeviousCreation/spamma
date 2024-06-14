﻿namespace Spamma.App.Client.Infrastructure.Contracts.Domain;

public record CommandValidationResult
{
    public bool IsValid => this.Failures.Count == 0;

    public IReadOnlyList<CommandValidationFailure> Failures { get; init; } = [];
}