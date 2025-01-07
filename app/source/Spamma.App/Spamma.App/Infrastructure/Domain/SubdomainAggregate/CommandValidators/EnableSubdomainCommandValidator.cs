using FluentValidation;
using Spamma.App.Client.Infrastructure.Constants;
using Spamma.App.Client.Infrastructure.Domain.SubdomainAggregate.Commands;

namespace Spamma.App.Infrastructure.Domain.SubdomainAggregate.CommandValidators;

internal class EnableSubdomainCommandValidator : AbstractValidator<EnableSubdomainCommand>
{
    public EnableSubdomainCommandValidator()
    {
        this.RuleFor(x => x.SubdomainId)
            .NotEmpty()
            .WithErrorCode(ValidationCodes.PropertyIsRequired);
    }
}