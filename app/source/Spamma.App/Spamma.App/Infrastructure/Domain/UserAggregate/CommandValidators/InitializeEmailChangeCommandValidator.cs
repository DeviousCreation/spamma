﻿using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Spamma.App.Client.Infrastructure.Constants;
using Spamma.App.Client.Infrastructure.Domain.UserAggregate.Commands;
using Spamma.App.Infrastructure.Database;
using Spamma.App.Infrastructure.Querying.Entities;

namespace Spamma.App.Infrastructure.Domain.UserAggregate.CommandValidators;

internal class InitializeEmailChangeCommandValidator : AbstractValidator<InitializeEmailChangeCommand>
{
    public InitializeEmailChangeCommandValidator(IDbContextFactory<SpammaDataContext> dbContextFactory)
    {
        this.RuleFor(x => x.UserId).NotEmpty()
            .WithErrorCode(ValidationCodes.PropertyIsRequired);
        this.RuleFor(x=>x.EmailAddress)
            .NotEmpty()
            .WithErrorCode(ValidationCodes.PropertyIsRequired)
            .EmailAddress()
            .WithErrorCode(ValidationCodes.InvalidEmailAddress)
            .MustAsync(async (email, cancellationToken) =>
            {
                await using var dbContext = dbContextFactory.CreateDbContext();
                return await dbContext.Set<UserQueryEntity>().AllAsync(x => x.EmailAddress != email, cancellationToken);
            })
            .WithErrorCode(ValidationCodes.EmailAddressAlreadyExists);
    }
}