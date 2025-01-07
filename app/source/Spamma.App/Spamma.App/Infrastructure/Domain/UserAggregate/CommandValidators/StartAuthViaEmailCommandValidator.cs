using FluentValidation;
using Spamma.App.Client.Infrastructure.Constants;
using Spamma.App.Client.Infrastructure.Domain.UserAggregate.Commands;

namespace Spamma.App.Infrastructure.Domain.UserAggregate.CommandValidators;

internal class StartAuthViaEmailCommandValidator : AbstractValidator<StartAuthViaEmailCommand>
{
    public StartAuthViaEmailCommandValidator()
    {
        this.RuleFor(x => x.EmailAddress)
            .NotEmpty()
            .WithErrorCode(ValidationCodes.PropertyIsRequired)
            .EmailAddress()
            .WithErrorCode(ValidationCodes.InvalidEmailAddress);
    }
}