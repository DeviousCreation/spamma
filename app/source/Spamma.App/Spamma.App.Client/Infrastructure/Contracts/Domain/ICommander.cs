using MediatR;

namespace Spamma.App.Client.Infrastructure.Contracts.Domain;

public interface ICommander
{
    Task<CommandResult> Send<TCommand>(TCommand request, CancellationToken cancellationToken = default)
        where TCommand : ICommand;

    Task<CommandResult<TCommandResult>> Send<TCommand, TCommandResult>(TCommand request, CancellationToken cancellationToken = default)
        where TCommand : ICommand<TCommandResult>
        where TCommandResult : ICommandResult;
}

public class Commander(ISender sender) : ICommander
{
    public Task<CommandResult> Send<TCommand>(TCommand request, CancellationToken cancellationToken = default)
        where TCommand : ICommand
    {
        return sender.Send(request, cancellationToken);
    }

    public Task<CommandResult<TCommandResult>> Send<TCommand, TCommandResult>(TCommand request, CancellationToken cancellationToken = default)
        where TCommand : ICommand<TCommandResult>
        where TCommandResult : ICommandResult
    {
        return sender.Send(request, cancellationToken);
    }
}