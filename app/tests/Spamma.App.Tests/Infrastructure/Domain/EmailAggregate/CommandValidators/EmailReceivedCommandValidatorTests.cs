using Spamma.App.Client.Infrastructure.Domain.EmailAggregate.Commands;
using Spamma.App.Infrastructure.Domain.EmailAggregate.CommandValidators;

namespace Spamma.App.Tests.Infrastructure.Domain.EmailAggregate.CommandValidators;

public class EmailReceivedCommandValidatorTests
{
    [Fact]
    public async Task ShouldHaveErrorWhenEmailIdIsEmpty()
    {
        // Arrange
        var command = new EmailReceivedCommand(
            Guid.Empty,
            Guid.NewGuid(),
            "email-address",
            "subject",
            DateTime.UtcNow);

        var validator = new EmailReceivedCommandValidator();

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        await Verify(result);
    }

    [Fact]
    public async Task ShouldHaveErrorWhenSubdomainIdIsEmpty()
    {
        // Arrange
        var command = new EmailReceivedCommand(
            Guid.NewGuid(),
            Guid.Empty,
            "email-address",
            "subject",
            DateTime.UtcNow);

        var validator = new EmailReceivedCommandValidator();

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        await Verify(result);
    }

    [Fact]
    public async Task ShouldHaveErrorWhenEmailAddressIsEmpty()
    {
        // Arrange
        var command = new EmailReceivedCommand(
            Guid.NewGuid(),
            Guid.NewGuid(),
            string.Empty,
            "subject",
            DateTime.UtcNow);

        var validator = new EmailReceivedCommandValidator();

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        await Verify(result);
    }

    [Fact]
    public async Task ShouldHaveErrorWhenSubjectIsEmpty()
    {
        // Arrange
        var command = new EmailReceivedCommand(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "email-address",
            string.Empty,
            DateTime.UtcNow);

        var validator = new EmailReceivedCommandValidator();

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        await Verify(result);
    }

    [Fact]
    public async Task ShouldHaveErrorWhenWhenSentIsEmpty()
    {
        // Arrange
        var command = new EmailReceivedCommand(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "email-address",
            "subject",
            DateTime.MinValue);

        var validator = new EmailReceivedCommandValidator();

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        await Verify(result);
    }
}