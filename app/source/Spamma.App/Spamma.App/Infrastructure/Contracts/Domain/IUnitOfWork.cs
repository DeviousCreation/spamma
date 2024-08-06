using ResultMonad;

namespace Spamma.App.Infrastructure.Contracts.Domain
{
    internal interface IUnitOfWork : IDisposable
    {
        Task<ResultWithError<IPersistenceError>> SaveEntitiesAsync(CancellationToken cancellationToken = default);
    }
}