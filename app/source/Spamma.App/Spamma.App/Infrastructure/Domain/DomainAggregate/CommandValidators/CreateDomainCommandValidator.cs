using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Spamma.App.Client.Infrastructure.Constants;
using Spamma.App.Client.Infrastructure.Domain.DomainAggregate.Commands;
using Spamma.App.Infrastructure.Database;
using Spamma.App.Infrastructure.Querying.Entities;

namespace Spamma.App.Infrastructure.Domain.DomainAggregate.CommandValidators;

internal class CreateDomainCommandValidator : AbstractValidator<CreateDomainCommand>
{
    public CreateDomainCommandValidator(IDbContextFactory<SpammaDataContext> dbContextFactory)
    {
        var dbContext = dbContextFactory.CreateDbContext();
        this.RuleFor(x => x.DomainId)
            .NotEmpty()
            .WithErrorCode(ValidationCodes.PropertyIsRequired);

        this.RuleFor(x => x.DomainName)
            .NotEmpty()
            .WithErrorCode(ValidationCodes.PropertyIsRequired)
            .MustAsync(async (domainName, cancellationToken) =>
            {
                var exists = await dbContext.Set<DomainQueryEntity>()
                    .AnyAsync(d => d.Name == domainName);
                return !exists;
            });

        this.RuleFor(x => x.CreatedByUserId)
            .NotEmpty()
            .WithErrorCode(ValidationCodes.PropertyIsRequired);
    }
}