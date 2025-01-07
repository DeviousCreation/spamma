using FluentValidation;
using Spamma.App.Client.Infrastructure.Constants;
using Spamma.App.Client.Infrastructure.Domain.DomainAggregate.Commands;

namespace Spamma.App.Infrastructure.Domain.DomainAggregate.CommandValidators;

internal class AlterUserAccessToDomainCommandValidator : AbstractValidator<AlterUserAccessToDomainCommand>
{
    public AlterUserAccessToDomainCommandValidator()
    {
        this.RuleFor(x => x.DomainId)
            .NotEmpty()
            .WithErrorCode(ValidationCodes.PropertyIsRequired);

        this.RuleFor(x => x.UserId)
            .NotEmpty()
            .WithErrorCode(ValidationCodes.PropertyIsRequired);

        this.RuleFor(x => x.DomainAccessPolicyType)
            .NotEmpty()
            .WithErrorCode(ValidationCodes.PropertyIsRequired);
    }
}