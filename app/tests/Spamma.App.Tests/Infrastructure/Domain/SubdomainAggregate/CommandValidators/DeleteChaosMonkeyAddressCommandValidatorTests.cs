using Spamma.App.Client.Infrastructure.Domain.SubdomainAggregate.Commands;
using Spamma.App.Infrastructure.Domain.SubdomainAggregate.CommandValidators;

namespace Spamma.App.Tests.Infrastructure.Domain.SubdomainAggregate.CommandValidators;

public class DeleteChaosMonkeyAddressCommandValidatorTests
{
    [Fact]
    public async Task ShouldHaveErrorWhenSubdomainIdIsEmpty()
    {
        // Arrange
        var command = new DeleteChaosMonkeyAddressCommand(
            Guid.Empty,
            Guid.NewGuid());

        var validator = new DeleteChaosMonkeyAddressCommandValidator();

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        await Verify(result);
    }

    [Fact]
    public async Task ShouldHaveErrorWhenAddressIdIsEmpty()
    {
        // Arrange
        var command = new DeleteChaosMonkeyAddressCommand(
            Guid.NewGuid(),
            Guid.Empty);

        var validator = new DeleteChaosMonkeyAddressCommandValidator();

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        await Verify(result);
    }
}