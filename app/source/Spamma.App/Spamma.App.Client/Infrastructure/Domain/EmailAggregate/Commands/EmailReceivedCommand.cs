using Spamma.App.Client.Infrastructure.Contracts.Domain;

namespace Spamma.App.Client.Infrastructure.Domain.EmailAggregate.Commands;

public record EmailReceivedCommand(
    Guid EmailId, Guid SubdomainId, string EmailAddress, string Subject, DateTimeOffset WhenSent) : ICommand;