using Spamma.App.Infrastructure.Contracts.Querying;

namespace Spamma.App.Infrastructure.Querying.Entities;

public class DomainQueryEntity : IQueryEntity
{
    private DomainQueryEntity()
    {
    }

    public Guid Id { get; private set; }

    public string Name { get; private set; }
}