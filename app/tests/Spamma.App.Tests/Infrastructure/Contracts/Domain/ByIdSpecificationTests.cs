using Spamma.App.Infrastructure.Contracts.Domain;

namespace Spamma.App.Tests.Infrastructure.Contracts.Domain;

public class ByIdSpecificationTests
{
    [Fact]
    public async Task IsSatisfiedByAsync_WhenEntityIdMatchesSpecificationId_ReturnsTrue()
    {
        // Arrange
        var entityId = Guid.NewGuid();
        var specification = new ByIdSpecification<TestAggregate>(entityId);
        var entity = new TestAggregate(entityId);

        // Act
        var result = specification.IsSatisfiedBy(entity);

        // Assert
        await Verify(result);
    }

    [Fact]
    public async Task IsSatisfiedByAsync_WhenEntityIdDoesNotMatchSpecificationId_ReturnsFalse()
    {
        // Arrange
        var entityId = Guid.NewGuid();
        var specification = new ByIdSpecification<TestAggregate>(entityId);
        var entity = new TestAggregate(Guid.NewGuid());

        // Act
        var result = specification.IsSatisfiedBy(entity);

        // Assert
        await Verify(result);
    }

    private class TestAggregate : Entity, IAggregateRoot
    {
        public TestAggregate(Guid id)
        {
            this.Id = id;
        }
    }
}