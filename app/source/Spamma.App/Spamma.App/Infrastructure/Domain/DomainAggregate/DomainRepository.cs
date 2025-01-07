using Microsoft.EntityFrameworkCore;
using Spamma.App.Infrastructure.Contracts.Domain;
using Spamma.App.Infrastructure.Database;

namespace Spamma.App.Infrastructure.Domain.DomainAggregate;

internal class DomainRepository(IDbContextFactory<SpammaDataContext> dbContext)
    : Repository<DomainAggregate.Aggregate.Domain>(dbContext);