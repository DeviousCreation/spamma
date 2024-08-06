using Spamma.App.Client.Infrastructure.Constants;
using Spamma.App.Client.Infrastructure.Domain.SubdomainAggregate.Commands;
using Spamma.App.Infrastructure.Domain.SubdomainAggregate.CommandValidators;

namespace Spamma.App.Tests.Infrastructure.Domain.SubdomainAggregate.CommandValidators;

public class AddChaosMonkeyAddressCommandValidatorTests
{
    [Fact]
    public async Task ShouldHaveErrorWhenSubdomainIdIsEmpty()
    {
        // Arrange
        var command = new AddChaosMonkeyAddressCommand(
            Guid.Empty,
            Guid.NewGuid(),
            "address",
            ChaosMonkeyType.NotFound);

        var validator = new AddChaosMonkeyAddressCommandValidator();

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        await Verify(result);
    }

    [Fact]
    public async Task ShouldHaveErrorWhenAddressIdIsEmpty()
    {
        // Arrange
        var command = new AddChaosMonkeyAddressCommand(
            Guid.NewGuid(),
            Guid.Empty,
            "address",
            ChaosMonkeyType.NotFound);

        var validator = new AddChaosMonkeyAddressCommandValidator();

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        await Verify(result);
    }

    [Fact]
    public async Task ShouldHaveErrorWhenAddressIsEmpty()
    {
        // Arrange
        var command = new AddChaosMonkeyAddressCommand(
            Guid.NewGuid(),
            Guid.NewGuid(),
            string.Empty,
            ChaosMonkeyType.NotFound);

        var validator = new AddChaosMonkeyAddressCommandValidator();

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        await Verify(result);
    }

    [Fact]
    public async Task ShouldHaveErrorWhenTypeIsEmpty()
    {
        // Arrange
        var command = new AddChaosMonkeyAddressCommand(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "address",
            ChaosMonkeyType.None);

        var validator = new AddChaosMonkeyAddressCommandValidator();

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        await Verify(result);
    }

    [Fact]
    public async Task ShouldNotHaveErrorWhenSubdomainIdAndAddressAndTypeAreNotEmpty()
    {
        // Arrange
        var command = new AddChaosMonkeyAddressCommand(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "address",
            ChaosMonkeyType.NotFound);

        var validator = new AddChaosMonkeyAddressCommandValidator();

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        await Verify(result);
    }
}