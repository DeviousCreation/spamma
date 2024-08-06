using Spamma.App.Infrastructure.Contracts.Domain;
using Spamma.App.Infrastructure.Database;
using Spamma.App.Infrastructure.Domain.UserAggregate.Aggregate;

namespace Spamma.App.Infrastructure.Domain.UserAggregate;

internal class UserRepository(SpammaDataContext dbContext)
    : Repository<User>(dbContext);