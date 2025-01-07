﻿using FluentValidation;
using Spamma.App.Client.Infrastructure.Constants;
using Spamma.App.Client.Infrastructure.Domain.UserAggregate.Commands;

namespace Spamma.App.Infrastructure.Domain.UserAggregate.CommandValidators;

internal class EnableUserCommandValidator : AbstractValidator<EnableUserCommand>
{
    public EnableUserCommandValidator()
    {
        this.RuleFor(x => x.UserId)
            .NotEmpty()
            .WithErrorCode(ValidationCodes.PropertyIsRequired);
    }
}