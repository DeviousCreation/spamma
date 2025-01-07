using Microsoft.EntityFrameworkCore;
using Spamma.App.Infrastructure.Contracts.Domain;
using Spamma.App.Infrastructure.Database;

namespace Spamma.App.Infrastructure.Domain.SettingAggregate;

internal class SettingRepository(IDbContextFactory<SpammaDataContext> dbContext)
    : Repository<SettingAggregate.Aggregate.Setting>(dbContext);