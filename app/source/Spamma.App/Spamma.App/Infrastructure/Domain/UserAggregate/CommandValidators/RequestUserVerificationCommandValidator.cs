using FluentValidation;
using Spamma.App.Client.Infrastructure.Constants;
using Spamma.App.Client.Infrastructure.Domain.UserAggregate.Commands;

namespace Spamma.App.Infrastructure.Domain.UserAggregate.CommandValidators;

internal class RequestUserVerificationCommandValidator : AbstractValidator<RequestUserVerificationCommand>
{
    public RequestUserVerificationCommandValidator()
    {
        this.RuleFor(x => x.EmailAddress)
            .NotEmpty()
            .WithErrorCode(ValidationCodes.PropertyIsRequired)
            .EmailAddress()
            .WithErrorCode(ValidationCodes.InvalidEmailAddress);
    }
}