﻿using FluentValidation;
using Microsoft.Extensions.Logging;
using Moq;
using Spamma.App.Client.Infrastructure.Domain.DomainAggregate.Commands;
using Spamma.App.Infrastructure.Domain.DomainAggregate.CommandHandlers;
using Spamma.App.Tests.Common;
using DomainEntity = Spamma.App.Infrastructure.Domain.DomainAggregate.Aggregate.Domain;

namespace Spamma.App.Tests.Infrastructure.Domain.DomainAggregate.CommandHandlers;

public class CreateDomainCommandHandlerTests
{
    [Fact]
    public async Task Handle_DataSaves_ExpectNewEmailAndSucceededStatus()
    {
        var command = new CreateDomainCommand(Guid.NewGuid(), "spamma.dev");

        DomainEntity? entity = null;

        var repository = DomainFactory.Empty<DomainEntity>(
            DomainFactory.CreateSuccessfulUnitOfWork(), e => entity = e);

        var handler = new CreateDomainCommandHandler(
            new List<IValidator<CreateDomainCommand>>(),
            Mock.Of<ILogger<CreateDomainCommandHandler>>(),
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
        var command = new CreateDomainCommand(Guid.NewGuid(), "spamma.dev");

        var repository = DomainFactory.Empty<DomainEntity>(DomainFactory.CreateFailedUnitOfWork());

        var handler = new CreateDomainCommandHandler(
            new List<IValidator<CreateDomainCommand>>(),
            Mock.Of<ILogger<CreateDomainCommandHandler>>(),
            repository.Object);
        var result = await handler.Handle(command, CancellationToken.None);

        await Verify(new
        {
            result.Status,
        });
    }
}