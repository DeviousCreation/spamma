using FluentValidation;
using Spamma.App.Client.Infrastructure.Constants;
using Spamma.App.Client.Infrastructure.Domain.SubdomainAggregate.Commands;

namespace Spamma.App.Infrastructure.Domain.SubdomainAggregate.CommandValidators;

internal class DisableSubdomainCommandValidator : AbstractValidator<DisableSubdomainCommand>
{
    public DisableSubdomainCommandValidator()
    {
        this.RuleFor(x => x.SubdomainId)
            .NotEmpty()
            .WithErrorCode(ValidationCodes.PropertyIsRequired);
    }
}