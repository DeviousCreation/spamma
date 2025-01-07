using MediatR;
using MediatR.Behaviors.Authorization.Exceptions;

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
    public async Task<CommandResult> Send<TCommand>(TCommand request, CancellationToken cancellationToken = default)
        where TCommand : ICommand
    {
        try
        {
            return await sender.Send(request, cancellationToken);
        }
        catch (UnauthorizedException e)
        {
            return CommandResult.Unauthorized();
        }
    }

    public async Task<CommandResult<TCommandResult>> Send<TCommand, TCommandResult>(TCommand request, CancellationToken cancellationToken = default)
        where TCommand : ICommand<TCommandResult>
        where TCommandResult : ICommandResult
    {
        try
        {
            return await sender.Send(request, cancellationToken);
        }
        catch (UnauthorizedException e)
        {
            return CommandResult<TCommandResult>.Unauthorized();
        }
    }
}