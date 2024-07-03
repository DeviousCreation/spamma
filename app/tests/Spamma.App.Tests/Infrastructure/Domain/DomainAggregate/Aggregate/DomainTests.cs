using Spamma.App.Tests.Common;
using DomainEntity = Spamma.App.Infrastructure.Domain.DomainAggregate.Aggregate.Domain;

namespace Spamma.App.Tests.Infrastructure.Domain.DomainAggregate.Aggregate;

public class DomainTests
{
    [Fact]
    public async Task WhenConstructed_ExpectDefault()
    {
        var domain = new DomainEntity(Guid.NewGuid(), "spamma.dev");

        await Verify(new
        {
            domain,
            ListCheck = EntityHelpers.CheckLists(domain),
        });
    }

    [Fact]
    public async Task WhenPrivateConstructorCalled_ExpectNewInstance()
    {
        var domain = EntityHelpers.CreateEntity<DomainEntity>(new
        {
            Id = Guid.NewGuid(),
            Name = "spamma.dev",
        });

        await Verify(new
        {
            domain,
            ListCheck = EntityHelpers.CheckLists(domain),
        });
    }
}