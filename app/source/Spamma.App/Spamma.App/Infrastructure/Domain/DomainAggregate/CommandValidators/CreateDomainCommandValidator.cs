using FluentValidation;
using Spamma.App.Client.Infrastructure.Constants;
using Spamma.App.Client.Infrastructure.Domain.DomainAggregate.Commands;

namespace Spamma.App.Infrastructure.Domain.DomainAggregate.CommandValidators;

public class CreateDomainCommandValidator : AbstractValidator<CreateDomainCommand>
{
    public CreateDomainCommandValidator()
    {
        this.RuleFor(x => x.DomainId)
            .NotEmpty()
            .WithErrorCode(ValidationCodes.PropertyIsRequired);

        this.RuleFor(x => x.DomainName)
            .NotEmpty()
            .WithErrorCode(ValidationCodes.PropertyIsRequired);
    }
}