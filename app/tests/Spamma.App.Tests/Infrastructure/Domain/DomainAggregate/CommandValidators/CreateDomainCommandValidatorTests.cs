using Moq;
using Spamma.App.Client.Infrastructure.Contracts.Querying;
using Spamma.App.Client.Infrastructure.Domain.DomainAggregate.Commands;
using Spamma.App.Infrastructure.Domain.DomainAggregate.CommandValidators;

namespace Spamma.App.Tests.Infrastructure.Domain.DomainAggregate.CommandValidators;

public class CreateDomainCommandValidatorTests
{
    [Fact]
    public async Task ShouldHaveErrorWhenDomainIdIsEmpty()
    {
        // Arrange
        var command = new CreateDomainCommand(Guid.Empty, "example.com", Guid.NewGuid(), DateTime.UtcNow);

        var validator = new CreateDomainCommandValidator(Mock.Of<IQuerier>());

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        await Verify(result);
    }

    [Fact]
    public async Task ShouldHaveErrorWhenDomainNameIsEmpty()
    {
        // Arrange
        var command = new CreateDomainCommand(Guid.NewGuid(), string.Empty, Guid.NewGuid(), DateTime.UtcNow);

        var validator = new CreateDomainCommandValidator(Mock.Of<IQuerier>());

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        await Verify(result);
    }

    [Fact]
    public async Task ShouldNotHaveErrorWhenDomainIdAndDomainNameAreNotEmpty()
    {
        // Arrange
        var command = new CreateDomainCommand(Guid.NewGuid(), "example.com", Guid.NewGuid(), DateTime.UtcNow);

        var validator = new CreateDomainCommandValidator(Mock.Of<IQuerier>());

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        await Verify(result);
    }
}