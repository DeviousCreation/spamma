using FluentValidation;
using Microsoft.Extensions.Logging;
using Moq;
using Spamma.App.Client.Infrastructure.Domain.EmailAggregate.Commands;
using Spamma.App.Infrastructure.Domain.EmailAggregate.Aggregate;
using Spamma.App.Infrastructure.Domain.EmailAggregate.CommandHandlers;
using Spamma.App.Tests.Common;
using Spamma.App.Tests.TestHelpers;

namespace Spamma.App.Tests.Infrastructure.Domain.EmailAggregate.CommandHandlers;

public class EmailReceivedCommandHandlerTests
{
    [Fact]
    public async Task Handle_DataSaves_ExpectNewEmailAndSucceededStatus()
    {
        var command = new EmailReceivedCommand(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "email-address",
            "subject",
            DateTime.UtcNow);

        Email? entity = null;

        var repository = DomainFactory.Empty<Email>(
            DomainFactory.CreateSuccessfulUnitOfWork(), e => entity = e);

        var handler = new EmailReceivedCommandHandler(
            new List<IValidator<EmailReceivedCommand>>(),
            Mock.Of<ILogger<EmailReceivedCommandHandler>>(),
            repository.Object);
        var result = await handler.Handle(command, CancellationToken.None);

        await Verify(new
        {
            result.Status,
            entity,
        });
    }

    [Fact]
    public async Task Handle_DataFailsToSave_ExpectFailedStatus()
    {
        var command = new EmailReceivedCommand(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "email-address",
            "subject",
            DateTime.UtcNow);

        var repository = DomainFactory.Empty<Email>(DomainFactory.CreateFailedUnitOfWork());

        var handler = new EmailReceivedCommandHandler(
            new List<IValidator<EmailReceivedCommand>>(),
            Mock.Of<ILogger<EmailReceivedCommandHandler>>(),
            repository.Object);
        var result = await handler.Handle(command, CancellationToken.None);

        await Verify(new
        {
            result.Status,
        });
    }
}