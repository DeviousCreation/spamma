using FluentValidation;
using Spamma.App.Client.Infrastructure.Constants;
using Spamma.App.Client.Infrastructure.Domain.DomainAggregate.Commands;

namespace Spamma.App.Infrastructure.Domain.DomainAggregate.CommandValidators;

internal class RevokeUserToAccessDomainCommandValidator : AbstractValidator<RevokeUserToAccessDomainCommand>
{
    public RevokeUserToAccessDomainCommandValidator()
    {
        this.RuleFor(x => x.DomainId)
            .NotEmpty()
            .WithErrorCode(ValidationCodes.PropertyIsRequired);

        this.RuleFor(x => x.UserId)
            .NotEmpty()
            .WithErrorCode(ValidationCodes.PropertyIsRequired);
    }
}