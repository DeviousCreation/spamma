using System.Net;
using FluentValidation.Results;
using MediatR;

namespace Spamma.App.Client.Infrastructure.Contracts.Domain;

public interface ICommandHandler<TCommand, TResult>
    : IRequestHandler<TCommand, CommandResult<TResult>>
    where TCommand : ICommand<TResult>;