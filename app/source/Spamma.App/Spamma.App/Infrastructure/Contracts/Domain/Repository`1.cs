using MaybeMonad;
using Microsoft.EntityFrameworkCore;
using Spamma.App.Infrastructure.Database;

namespace Spamma.App.Infrastructure.Contracts.Domain
{
    internal abstract class Repository<TAggregateRoot>
        : IRepository<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        private readonly SpammaDataContext _dbContext;

        private protected Repository(IDbContextFactory<SpammaDataContext> dbContextFactory)
        {
            this._dbContext = dbContextFactory.CreateDbContext();
        }

        public IUnitOfWork UnitOfWork => this._dbContext;

        public void Add(TAggregateRoot aggregate)
        {
            if (aggregate is not TAggregateRoot entity)
            {
                throw new ArgumentException("Entity not of correct type", nameof(aggregate));
            }

            this._dbContext.Set<TAggregateRoot>().Add(entity);
        }

        public void ForceUpdate(TAggregateRoot aggregate)
        {
            if (aggregate is not TAggregateRoot entity)
            {
                throw new ArgumentException("Entity not of correct type", nameof(aggregate));
            }

            this._dbContext.Set<TAggregateRoot>().Update(entity);
        }

        public void Delete(IAggregateRoot aggregate)
        {
            if (aggregate is not TAggregateRoot entity)
            {
                throw new ArgumentException("Entity not of correct type", nameof(aggregate));
            }

            this._dbContext.Set<TAggregateRoot>().Remove(entity);
        }

        public async Task<Maybe<TAggregateRoot>> FindOne(Specification<TAggregateRoot> specification, CancellationToken cancellationToken = default, bool refresh = true)
        {
            var data = await this._dbContext.Set<TAggregateRoot>().SingleOrDefaultAsync(specification.ToExpression(), cancellationToken);
            if (data == null)
            {
                return Maybe<TAggregateRoot>.Nothing;
            }

            if (refresh)
            {
                await this.Refresh(data);
            }

            return Maybe.From(data);
        }

        public async Task<IList<TAggregateRoot>> FindMany(Specification<TAggregateRoot> specification, CancellationToken cancellationToken = default, bool refresh = true)
        {
            var data = await this._dbContext.Set<TAggregateRoot>().Where(specification.ToExpression())
                .ToListAsync(cancellationToken);

            if (!refresh)
            {
                return data;
            }

            foreach (TAggregateRoot entity in data)
            {
                await this.Refresh(entity);
            }

            return data;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            this._dbContext.Dispose();
        }

        private async Task Refresh(TAggregateRoot? aggregate)
        {
            if (aggregate != null)
            {
                await this._dbContext.Entry(aggregate).ReloadAsync();
            }
        }
    }
}