using Spamma.App.Infrastructure.Contracts.Domain;
using Spamma.App.Infrastructure.Database;

namespace Spamma.App.Infrastructure.Domain.DomainAggregate;

public class DomainRepository(SpammaDataContext dbContext)
    : Repository<DomainAggregate.Aggregate.Domain>(dbContext);