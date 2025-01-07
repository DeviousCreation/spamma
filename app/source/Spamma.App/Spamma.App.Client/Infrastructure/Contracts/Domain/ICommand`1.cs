using MediatR;

namespace Spamma.App.Client.Infrastructure.Contracts.Domain;

public interface ICommand<TResult> : IRequest<CommandResult<TResult>>
    where TResult : ICommandResult;