using Spamma.App.Client.Infrastructure.Domain.SubdomainAggregate.Commands;
using Spamma.App.Infrastructure.Domain.SubdomainAggregate.CommandValidators;

namespace Spamma.App.Tests.Infrastructure.Domain.SubdomainAggregate.CommandValidators;

public class CreateSubdomainCommandValidatorTest
{
    [Fact]
    public async Task ShouldHaveErrorWhenSubdomainIdIsEmpty()
    {
        // Arrange
        var command = new CreateSubdomainCommand(
            Guid.Empty,
            "subdomain",
            Guid.NewGuid());

        var validator = new CreateSubdomainCommandValidator();

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        await Verify(result);
    }

    [Fact]
    public async Task ShouldHaveErrorWhenDomainIdIsEmpty()
    {
        // Arrange
        var command = new CreateSubdomainCommand(
            Guid.NewGuid(),
            "subdomain",
            Guid.Empty);

        var validator = new CreateSubdomainCommandValidator();

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        await Verify(result);
    }

    [Fact]
    public async Task ShouldHaveErrorWhenNameIsEmpty()
    {
        // Arrange
        var command = new CreateSubdomainCommand(
            Guid.NewGuid(),
            string.Empty,
            Guid.NewGuid());

        var validator = new CreateSubdomainCommandValidator();

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        await Verify(result);
    }
}