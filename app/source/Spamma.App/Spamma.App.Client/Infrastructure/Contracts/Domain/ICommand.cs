using MediatR;

namespace Spamma.App.Client.Infrastructure.Contracts.Domain;

public interface ICommand : IRequest<CommandResult>
{
    string PolicyName => string.Empty;
}