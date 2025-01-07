using Spamma.App.Infrastructure.Contracts.Domain;

namespace Spamma.App.Infrastructure.Domain.EmailAggregate.Aggregate;

internal class Email : Entity, IAggregateRoot
{
    internal Email(Guid id, Guid subdomainId, string emailAddress, string subject, DateTimeOffset whenSent)
    {
        this.Id = id;
        this.SubdomainId = subdomainId;
        this.EmailAddress = emailAddress;
        this.Subject = subject;
        this.WhenSent = whenSent;
    }

    private Email()
    {
    }

    internal Guid SubdomainId { get; private set; }

    internal string EmailAddress { get; private set; } = null!;

    internal string Subject { get; private set; } = null!;

    internal DateTimeOffset WhenSent { get; private set; }
}