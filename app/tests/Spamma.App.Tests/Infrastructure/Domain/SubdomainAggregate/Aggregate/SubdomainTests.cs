using Spamma.App.Client.Infrastructure.Constants;
using Spamma.App.Infrastructure.Domain.SubdomainAggregate.Aggregate;
using Spamma.App.Tests.Common;

namespace Spamma.App.Tests.Infrastructure.Domain.SubdomainAggregate.Aggregate;

public class SubdomainTests
{
    [Fact]
    public async Task WhenConstructed_ExpectDefault()
    {
        var entity = new Subdomain(
            Guid.NewGuid(),
            "dev.spamma.dev",
            Guid.NewGuid());

        await Verify(new
        {
            entity,
            ListCheck = EntityHelpers.CheckLists(entity),
        });
    }

    [Fact]
    public async Task WhenPrivateConstructorCalled_ExpectNewInstance()
    {
        var entity = EntityHelpers.CreateEntity<Subdomain>(new
        {
            Id = Guid.NewGuid(),
            Name = "dev.spamma.dev",
            DomainId = Guid.NewGuid(),
        });

        await Verify(new
        {
            entity,
            ListCheck = EntityHelpers.CheckLists(entity),
        });
    }

    [Fact]
    public async Task AddChaosMonkeyAddress_WhenAddressExists_ExpectFailedResult()
    {
        var entity = EntityHelpers.CreateEntity<Subdomain>(new
        {
            Id = Guid.NewGuid(),
            Name = "dev.spamma.dev",
            DomainId = Guid.NewGuid(),
            ChaosMonkeyAddresses = new List<ChaosMonkeyAddress>
            {
                EntityHelpers.CreateEntity<ChaosMonkeyAddress>(new
                {
                    Address = "example@dev.spamma.dev",
                }),
            },
        });

        var result = entity.AddChaosMonkeyAddress(new ChaosMonkeyAddress(
            Guid.NewGuid(),
            "example@dev.spamma.dev",
            ChaosMonkeyType.NotFound));

        await Verify(new
        {
            entity,
            result.IsFailure,
            result.Error,
        });
    }

    [Fact]
    public async Task AddChaosMonkeyAddress_WhenAddressDoesNotExist_ExpectSuccessResult()
    {
        var entity = EntityHelpers.CreateEntity<Subdomain>(new
        {
            Id = Guid.NewGuid(),
            Name = "dev.spamma.dev",
            DomainId = Guid.NewGuid(),
        });

        var result = entity.AddChaosMonkeyAddress(new ChaosMonkeyAddress(
            Guid.NewGuid(),
            "example@dev.spamma.dev",
            ChaosMonkeyType.NotFound));

        await Verify(new
        {
            entity,
            result.IsSuccess,
        });
    }

    [Fact]
    public async Task RemoveChaosMonkeyAddress_WhenAddressExists_ExpectSuccessResult()
    {
        var id = Guid.NewGuid();
        var entity = EntityHelpers.CreateEntity<Subdomain>(new
        {
            Id = Guid.NewGuid(),
            Name = "dev.spamma.dev",
            DomainId = Guid.NewGuid(),
            ChaosMonkeyAddresses = new List<ChaosMonkeyAddress>
            {
                EntityHelpers.CreateEntity<ChaosMonkeyAddress>(new
                {
                    Id = id,
                    Address = "example@dev.spamma.dev",
                }),
            },
        });

        var result = entity.RemoveChaosMonkeyAddress(id);

        await Verify(new
        {
            entity,
            result.IsSuccess,
        });
    }

    [Fact]
    public async Task RemoveChaosMonkeyAddress_WhenAddressDoesNotExist_ExpectFailedResult()
    {
        var entity = EntityHelpers.CreateEntity<Subdomain>(new
        {
            Id = Guid.NewGuid(),
            Name = "dev.spamma.dev",
            DomainId = Guid.NewGuid(),
        });

        var result = entity.RemoveChaosMonkeyAddress(Guid.NewGuid());

        await Verify(new
        {
            entity,
            result.IsFailure,
            result.Error,
        });
    }

    [Fact]
    public async Task UpdateChaosMonkeyAddress_WhenAddressExists_ExpectSuccessResult()
    {
        var id = Guid.NewGuid();
        var entity = EntityHelpers.CreateEntity<Subdomain>(new
        {
            Id = Guid.NewGuid(),
            Name = "dev.spamma.dev",
            DomainId = Guid.NewGuid(),
            ChaosMonkeyAddresses = new List<ChaosMonkeyAddress>
            {
                EntityHelpers.CreateEntity<ChaosMonkeyAddress>(new
                {
                    Id = id,
                    Address = "example@dev.spamma.dev",
                    Type = ChaosMonkeyType.InboxFull,
                }),
            },
        });

        var result = entity.UpdateChaosMonkeyAddress(id, ChaosMonkeyType.NotFound);

        await Verify(new
        {
            entity,
            result.IsSuccess,
        });
    }

    [Fact]
    public async Task UpdateChaosMonkeyAddress_WhenAddressDoesNotExists_ExpectFailedResult()
    {
        var entity = EntityHelpers.CreateEntity<Subdomain>(new
        {
            Id = Guid.NewGuid(),
            Name = "dev.spamma.dev",
            DomainId = Guid.NewGuid(),
        });

        var result = entity.UpdateChaosMonkeyAddress(Guid.NewGuid(), ChaosMonkeyType.NotFound);

        await Verify(new
        {
            entity,
            result.IsSuccess,
        });
    }
}