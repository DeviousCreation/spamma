using Spamma.App.Infrastructure.Contracts.Domain;
using Spamma.App.Infrastructure.Database;
using Spamma.App.Infrastructure.Domain.EmailAggregate.Aggregate;

namespace Spamma.App.Infrastructure.Domain.EmailAggregate;

internal class EmailRepository(SpammaDataContext dbContext)
    : Repository<Email>(dbContext);