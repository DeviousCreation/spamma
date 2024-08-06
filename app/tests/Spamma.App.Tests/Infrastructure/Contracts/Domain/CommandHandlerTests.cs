using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;
using Spamma.App.Client.Infrastructure.Contracts.Domain;
using Spamma.App.Infrastructure.Contracts.Domain;

namespace Spamma.App.Tests.Infrastructure.Contracts.Domain;

public class CommandHandlerTests
{
    private readonly StubCommand _command = new();
    private readonly Mock<ILogger<StubCommandHandler>> _logger = new();
    private readonly Mock<IValidator<StubCommand>> _validator = new();

    [Fact]
    public async Task Handle_WhenCommandIsInvalid_ShouldReturnFailedResult()
    {
        // Arrange
        this._validator.Setup(x => x.Validate(this._command))
            .Returns(new ValidationResult(new List<ValidationFailure>
            {
                new("Property", "Error message"),
            }));
        var handler = new StubCommandHandler(new[] { this._validator.Object }, this._logger.Object);

        // Act
        var result = await handler.Handle(this._command, CancellationToken.None);

        // Assert
        await Verify(new
        {
            result.Status,
            result.ValidationResult,
        });
    }

    internal class StubCommandHandler(IEnumerable<IValidator<StubCommand>> validators, ILogger<StubCommandHandler> logger) : CommandHandler<StubCommand>(validators, logger)
    {
        protected override Task<CommandResult> HandleInternal(StubCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

    internal class StubCommand : ICommand;
}