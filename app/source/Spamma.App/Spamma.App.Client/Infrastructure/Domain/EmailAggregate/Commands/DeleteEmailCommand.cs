using Spamma.App.Client.Infrastructure.Contracts.Domain;

namespace Spamma.App.Client.Infrastructure.Domain.EmailAggregate.Commands;

public record DeleteEmailCommand(Guid EmailId) : ICommand;