using EntityFramework.Exceptions.Common;
using Microsoft.EntityFrameworkCore;
using ResultMonad;
using Spamma.App.Infrastructure.Contracts.Database;
using Spamma.App.Infrastructure.Contracts.Domain;
using Spamma.App.Infrastructure.Database.TypeConfigurations.QueryableTypes;
using MutableDomainConfiguration = Spamma.App.Infrastructure.Database.TypeConfigurations.MutableTypes.DomainConfiguration;

namespace Spamma.App.Infrastructure.Database
{
    public class SpammaDataContext(DbContextOptions options) : DbContext(options), IUnitOfWork
    {
        public async Task<ResultWithError<IPersistenceError>> SaveEntitiesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                await this.SaveChangesAsync(cancellationToken);
            }
            catch (UniqueConstraintException)
            {
                return ResultWithError.Fail<IPersistenceError>(new UniquePersistenceError());
            }
            catch (ReferenceConstraintException)
            {
                return ResultWithError.Fail<IPersistenceError>(new InUsePersistenceError());
            }

            return ResultWithError.Ok<IPersistenceError>();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new MutableDomainConfiguration());
            modelBuilder.ApplyConfiguration(new DomainConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}