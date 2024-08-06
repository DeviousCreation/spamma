using Spamma.App.Client.Infrastructure.Constants;
using Spamma.App.Infrastructure.Domain.DomainAggregate.Aggregate;
using Spamma.App.Tests.Common;
using DomainEntity = Spamma.App.Infrastructure.Domain.DomainAggregate.Aggregate.Domain;

namespace Spamma.App.Tests.Infrastructure.Domain.DomainAggregate.Aggregate;

public class DomainTests
{
    [Fact]
    public async Task WhenConstructed_ExpectDefault()
    {
        var domain = new DomainEntity(
            Guid.NewGuid(),
            "spamma.dev",
            Guid.NewGuid(),
            DateTime.UtcNow);

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
    
    [Fact]
    public async Task SetUserAccess_WhenUserHasNoAccess_ExpectDomainAccessPolicyAdded()
    {
        var domain = EntityHelpers.CreateEntity<DomainEntity>();

        var result = domain.SetUserAccess(Guid.NewGuid(), DomainAccessPolicyType.Administrator, DateTime.UtcNow);

        await Verify(new
        {
            domain,
            result.IsSuccess,
        });
    }
    
    [Fact]
    public async Task SetUserAccess_WhenUserHasAccessAndHasSamePolicy_ExpectFailure()
    {
        var userId = Guid.NewGuid();
        var domain = EntityHelpers.CreateEntity<DomainEntity>(new
        {
            DomainAccessPolicies = new List<DomainAccessPolicy>
            {
                EntityHelpers.CreateEntity<DomainAccessPolicy>(new
                {
                    Id = userId,
                    PolicyType = DomainAccessPolicyType.Administrator,
                }),
            },
        });

        var result = domain.SetUserAccess(userId, DomainAccessPolicyType.Administrator, DateTime.UtcNow);

        await Verify(new
        {
            domain,
            result.IsFailure,
            result.Error,
        });
    }
}