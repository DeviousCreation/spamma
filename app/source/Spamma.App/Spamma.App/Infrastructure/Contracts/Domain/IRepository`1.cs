using MaybeMonad;

namespace Spamma.App.Infrastructure.Contracts.Domain
{
    internal interface IRepository<TAggregateRoot> : IDisposable
        where TAggregateRoot : class, IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }

        void Add(TAggregateRoot aggregate);

        void ForceUpdate(TAggregateRoot aggregate);

        void Delete(IAggregateRoot aggregate);

        Task<Maybe<TAggregateRoot>> FindOne(Specification<TAggregateRoot> specification, CancellationToken cancellationToken = default, bool refresh = true);

        Task<IList<TAggregateRoot>> FindMany(Specification<TAggregateRoot> specification, CancellationToken cancellationToken = default, bool refresh = true);
    }
}