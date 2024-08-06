using Spamma.App.Infrastructure.Contracts.Domain;
using Spamma.App.Infrastructure.Database;
using Spamma.App.Infrastructure.Domain.SubdomainAggregate.Aggregate;

namespace Spamma.App.Infrastructure.Domain.SubdomainAggregate;

internal class SubdomainRepository(SpammaDataContext dbContext)
    : Repository<Subdomain>(dbContext);