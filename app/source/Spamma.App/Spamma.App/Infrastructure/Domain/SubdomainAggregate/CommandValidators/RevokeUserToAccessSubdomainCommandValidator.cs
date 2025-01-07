using FluentValidation;
using Spamma.App.Client.Infrastructure.Constants;
using Spamma.App.Client.Infrastructure.Domain.SubdomainAggregate.Commands;

namespace Spamma.App.Infrastructure.Domain.SubdomainAggregate.CommandValidators;

internal class RevokeUserToAccessSubdomainCommandValidator : AbstractValidator<RevokeUserToAccessSubdomainCommand>
{
    public RevokeUserToAccessSubdomainCommandValidator()
    {
        this.RuleFor(x => x.SubdomainId)
            .NotEmpty()
            .WithErrorCode(ValidationCodes.PropertyIsRequired);

        this.RuleFor(x => x.UserId)
            .NotEmpty()
            .WithErrorCode(ValidationCodes.PropertyIsRequired);
    }
}