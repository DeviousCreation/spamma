using FluentValidation;
using Spamma.App.Client.Infrastructure.Constants;
using Spamma.App.Client.Infrastructure.Domain.DomainAggregate.Commands;

namespace Spamma.App.Infrastructure.Domain.DomainAggregate.CommandValidators;

internal class DisableDomainCommandValidator : AbstractValidator<DisableDomainCommand>
{
    public DisableDomainCommandValidator()
    {
        this.RuleFor(x => x.DomainId)
            .NotEmpty()
            .WithErrorCode(ValidationCodes.PropertyIsRequired);
    }
    
}