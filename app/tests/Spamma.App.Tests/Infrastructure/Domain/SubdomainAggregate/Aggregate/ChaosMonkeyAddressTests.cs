using Spamma.App.Client.Infrastructure.Constants;
using Spamma.App.Infrastructure.Domain.SubdomainAggregate.Aggregate;
using Spamma.App.Tests.Common;

namespace Spamma.App.Tests.Infrastructure.Domain.SubdomainAggregate.Aggregate;

public class ChaosMonkeyAddressTests
{
    [Fact]
    public async Task WhenConstructed_ExpectDefault()
    {
        var entity = new ChaosMonkeyAddress(
            Guid.NewGuid(),
            "chaos@spamma.dev",
            ChaosMonkeyType.NotFound);

        await Verify(new
        {
            entity,
            ListCheck = EntityHelpers.CheckLists(entity),
        });
    }

    [Fact]
    public async Task WhenPrivateConstructorCalled_ExpectNewInstance()
    {
        var entity = EntityHelpers.CreateEntity<ChaosMonkeyAddress>(new
        {
            Id = Guid.NewGuid(),
            Address = "chaos@spamma.dev",
            Type = ChaosMonkeyType.NotFound,
        });

        await Verify(new
        {
            entity,
            ListCheck = EntityHelpers.CheckLists(entity),
        });
    }

    [Fact]
    public async Task ChangeType_ExpectTypeChanged()
    {
        var entity = EntityHelpers.CreateEntity<ChaosMonkeyAddress>(new
        {
            Id = Guid.NewGuid(),
            Address = "chaos@spamma.dev",
            Type = ChaosMonkeyType.NotFound,
        });

        var result = entity.ChangeType(ChaosMonkeyType.InboxFull);

        await Verify(new
        {
            result,
            ListCheck = EntityHelpers.CheckLists(entity),
        });
    }
}