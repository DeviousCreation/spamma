using FluentValidation;
using Spamma.App.Client.Infrastructure.Constants;
using Spamma.App.Client.Infrastructure.Domain.SubdomainAggregate.Commands;

namespace Spamma.App.Infrastructure.Domain.SubdomainAggregate.CommandValidators;

internal class DeleteChaosMonkeyAddressCommandValidator : AbstractValidator<DeleteChaosMonkeyAddressCommand>
{
    public DeleteChaosMonkeyAddressCommandValidator()
    {
        this.RuleFor(x => x.AddressId)
            .NotEmpty()
            .WithErrorCode(ValidationCodes.PropertyIsRequired);

        this.RuleFor(x => x.SubdomainId)
            .NotEmpty()
            .WithErrorCode(ValidationCodes.PropertyIsRequired);
    }
}