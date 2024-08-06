using EntityFramework.Exceptions.Common;
using Microsoft.EntityFrameworkCore;
using ResultMonad;
using Spamma.App.Infrastructure.Contracts.Database;
using Spamma.App.Infrastructure.Contracts.Domain;
using Spamma.App.Infrastructure.Database.TypeConfigurations.QueryableTypes;
using MutableDomainConfiguration = Spamma.App.Infrastructure.Database.TypeConfigurations.MutableTypes.DomainConfiguration;
using MutableEmailConfiguration = Spamma.App.Infrastructure.Database.TypeConfigurations.MutableTypes.EmailConfiguration;
using MutableSubdomainConfiguration = Spamma.App.Infrastructure.Database.TypeConfigurations.MutableTypes.SubdomainConfiguration;
using MutableUserConfiguration = Spamma.App.Infrastructure.Database.TypeConfigurations.MutableTypes.UserConfiguration;

namespace Spamma.App.Infrastructure.Database
{
    internal class SpammaDataContext(DbContextOptions options) : DbContext(options), IUnitOfWork
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
            modelBuilder.ApplyConfiguration(new MutableEmailConfiguration());
            modelBuilder.ApplyConfiguration(new MutableSubdomainConfiguration());
            modelBuilder.ApplyConfiguration(new MutableUserConfiguration());

            modelBuilder.ApplyConfiguration(new ChaosMonkeyAddressConfiguration());
            modelBuilder.ApplyConfiguration(new DomainAccessPolicyConfiguration());
            modelBuilder.ApplyConfiguration(new DomainConfiguration());
            modelBuilder.ApplyConfiguration(new EmailConfiguration());
            modelBuilder.ApplyConfiguration(new RecordedUserEventConfiguration());
            modelBuilder.ApplyConfiguration(new SubdomainAccessPolicyConfiguration());
            modelBuilder.ApplyConfiguration(new SubdomainConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}