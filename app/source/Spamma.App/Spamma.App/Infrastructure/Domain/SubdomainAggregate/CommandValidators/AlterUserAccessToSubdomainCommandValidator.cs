using FluentValidation;
using Spamma.App.Client.Infrastructure.Constants;
using Spamma.App.Client.Infrastructure.Domain.SubdomainAggregate.Commands;

namespace Spamma.App.Infrastructure.Domain.SubdomainAggregate.CommandValidators;

internal class AlterUserAccessToSubdomainCommandValidator : AbstractValidator<AlterUserAccessToSubdomainCommand>
{
    public AlterUserAccessToSubdomainCommandValidator()
    {
        this.RuleFor(x => x.SubdomainId)
            .NotEmpty()
            .WithErrorCode(ValidationCodes.PropertyIsRequired);

        this.RuleFor(x => x.UserId)
            .NotEmpty()
            .WithErrorCode(ValidationCodes.PropertyIsRequired);
    }
}