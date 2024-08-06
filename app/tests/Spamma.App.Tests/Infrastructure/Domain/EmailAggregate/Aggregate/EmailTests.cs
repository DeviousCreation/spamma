using Spamma.App.Infrastructure.Domain.EmailAggregate.Aggregate;
using Spamma.App.Tests.Common;

namespace Spamma.App.Tests.Infrastructure.Domain.EmailAggregate.Aggregate;

public class EmailTests
{
    [Fact]
    public async Task WhenConstructed_ExpectDefault()
    {
        var domain = new Email(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "email@spamma.dev",
            "subject",
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
        var domain = EntityHelpers.CreateEntity<Email>(new
        {
            Id = Guid.NewGuid(),
            SubdomainId = Guid.NewGuid(),
            EmailAddress = "email@spamma.dev",
            Subject = "subject",
            WhenSent = DateTime.UtcNow,
        });

        await Verify(new
        {
            domain,
            ListCheck = EntityHelpers.CheckLists(domain),
        });
    }
}