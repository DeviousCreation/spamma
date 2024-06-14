using MediatR;

namespace Spamma.App.Client.Infrastructure.Contracts.Domain;

public interface ICommandHandler<in TCommand>
    : IRequestHandler<TCommand, CommandResult>
    where TCommand : ICommand;