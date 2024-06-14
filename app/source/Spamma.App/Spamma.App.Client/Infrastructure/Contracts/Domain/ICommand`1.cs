using MediatR;

namespace Spamma.App.Client.Infrastructure.Contracts.Domain;

public interface ICommand<T> : IRequest<CommandResult<T>>;