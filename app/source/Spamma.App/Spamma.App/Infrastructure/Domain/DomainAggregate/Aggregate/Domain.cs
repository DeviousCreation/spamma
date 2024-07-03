using Spamma.App.Infrastructure.Contracts.Domain;

namespace Spamma.App.Infrastructure.Domain.DomainAggregate.Aggregate;

public class Domain : Entity, IAggregateRoot
{
    public Domain(Guid id, string name)
    {
        this.Id = id;
        this.Name = name;
    }

    private Domain()
    {
    }

    public string Name { get; private set; }
}