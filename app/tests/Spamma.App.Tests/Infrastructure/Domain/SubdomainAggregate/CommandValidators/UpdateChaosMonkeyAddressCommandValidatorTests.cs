using Spamma.App.Client.Infrastructure.Constants;
using Spamma.App.Client.Infrastructure.Domain.SubdomainAggregate.Commands;
using Spamma.App.Infrastructure.Domain.SubdomainAggregate.CommandValidators;

namespace Spamma.App.Tests.Infrastructure.Domain.SubdomainAggregate.CommandValidators;

public class UpdateChaosMonkeyAddressCommandValidatorTests
{
    [Fact]
    public async Task ShouldHaveErrorWhenSubdomainIdIsEmpty()
    {
        // Arrange
        var command = new UpdateChaosMonkeyAddressCommand(
            Guid.Empty,
            Guid.NewGuid(),
            ChaosMonkeyType.None);

        var validator = new UpdateChaosMonkeyAddressCommandValidator();

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        await Verify(result);
    }

    [Fact]
    public async Task ShouldHaveErrorWhenAddressIdIsEmpty()
    {
        // Arrange
        var command = new UpdateChaosMonkeyAddressCommand(
            Guid.NewGuid(),
            Guid.Empty,
            ChaosMonkeyType.None);

        var validator = new UpdateChaosMonkeyAddressCommandValidator();

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        await Verify(result);
    }

    [Fact]
    public async Task ShouldHaveErrorWhenTypeIsEmpty()
    {
        // Arrange
        var command = new UpdateChaosMonkeyAddressCommand(
            Guid.NewGuid(),
            Guid.NewGuid(),
            ChaosMonkeyType.None);

        var validator = new UpdateChaosMonkeyAddressCommandValidator();

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        await Verify(result);
    }
}