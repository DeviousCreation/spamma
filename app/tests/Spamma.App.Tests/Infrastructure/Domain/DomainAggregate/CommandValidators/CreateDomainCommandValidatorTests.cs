using Spamma.App.Client.Infrastructure.Domain.DomainAggregate.Commands;
using Spamma.App.Infrastructure.Domain.DomainAggregate.CommandValidators;

namespace Spamma.App.Tests.Infrastructure.Domain.DomainAggregate.CommandValidators;

public class CreateDomainCommandValidatorTests
{
    [Fact]
    public async Task ShouldHaveErrorWhenDomainIdIsEmpty()
    {
        // Arrange
        var command = new CreateDomainCommand(Guid.Empty, "example.com");

        var validator = new CreateDomainCommandValidator();

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        await Verify(result);
    }

    [Fact]
    public async Task ShouldHaveErrorWhenDomainNameIsEmpty()
    {
        // Arrange
        var command = new CreateDomainCommand(Guid.NewGuid(), string.Empty);

        var validator = new CreateDomainCommandValidator();

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        await Verify(result);
    }

    [Fact]
    public async Task ShouldNotHaveErrorWhenDomainIdAndDomainNameAreNotEmpty()
    {
        // Arrange
        var command = new CreateDomainCommand(Guid.NewGuid(), "example.com");

        var validator = new CreateDomainCommandValidator();

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        await Verify(result);
    }
}