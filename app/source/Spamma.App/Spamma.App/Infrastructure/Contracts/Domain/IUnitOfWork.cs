using ResultMonad;

namespace Spamma.App.Infrastructure.Contracts.Domain
{
    public interface IUnitOfWork : IDisposable
    {
        Task<ResultWithError<IPersistenceError>> SaveEntitiesAsync(CancellationToken cancellationToken = default);
    }
}