using MaybeMonad;
using Microsoft.EntityFrameworkCore;
using Spamma.App.Infrastructure.Database;

namespace Spamma.App.Infrastructure.Contracts.Domain
{
    public abstract class Repository<TAggregateRoot>(SpammaDataContext dbContext) : IRepository<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        public IUnitOfWork UnitOfWork => dbContext;

        public void Add(TAggregateRoot aggregate)
        {
            if (aggregate is not TAggregateRoot entity)
            {
                throw new ArgumentException("Entity not of correct type", nameof(aggregate));
            }

            dbContext.Set<TAggregateRoot>().Add(entity);
        }

        public void Update(TAggregateRoot aggregate)
        {
            if (aggregate is not TAggregateRoot entity)
            {
                throw new ArgumentException("Entity not of correct type", nameof(aggregate));
            }

            dbContext.Set<TAggregateRoot>().Update(entity);
        }

        public void Delete(IAggregateRoot aggregate)
        {
            if (aggregate is not TAggregateRoot entity)
            {
                throw new ArgumentException("Entity not of correct type", nameof(aggregate));
            }

            dbContext.Set<TAggregateRoot>().Remove(entity);
        }

        public async Task<Maybe<TAggregateRoot>> FindOne(Specification<TAggregateRoot> specification, CancellationToken cancellationToken = default, bool refresh = true)
        {
            var data = await dbContext.Set<TAggregateRoot>().SingleOrDefaultAsync(specification.ToExpression(), cancellationToken);
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
            var data = await dbContext.Set<TAggregateRoot>().Where(specification.ToExpression())
                .ToListAsync(cancellationToken);

            if (refresh)
            {
                foreach (TAggregateRoot entity in data)
                {
                    await this.Refresh(entity);
                }
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
            dbContext.Dispose();
        }

        private async Task Refresh(TAggregateRoot? aggregate)
        {
            if (aggregate != null)
            {
                await dbContext.Entry(aggregate).ReloadAsync();
            }
        }
    }
}