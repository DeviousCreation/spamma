﻿using FluentValidation;
using NodaTime;
using Spamma.App.Client.Infrastructure.Constants;
using Spamma.App.Client.Infrastructure.Contracts.Domain;
using Spamma.App.Client.Infrastructure.Domain.UserAggregate.Commands;
using Spamma.App.Infrastructure.Contracts;
using Spamma.App.Infrastructure.Contracts.Domain;
using Spamma.App.Infrastructure.Domain.UserAggregate.Aggregate;
using Spamma.App.Infrastructure.Domain.UserAggregate.IntegrationEvents;

namespace Spamma.App.Infrastructure.Domain.UserAggregate.CommandHandlers;

internal class DisableUserCommandHandler(
    IEnumerable<IValidator<DisableUserCommand>> validators, ILogger<DisableUserCommandHandler> logger,
    IRepository<User> repository, IIntegrationEventPublisher integrationEventPublisher, IClock clock)
    : CommandHandler<DisableUserCommand>(validators, logger)
{
    protected override async Task<CommandResult> HandleInternal(
        DisableUserCommand request, CancellationToken cancellationToken)
    {
        var now = clock.GetCurrentInstant().ToDateTimeUtc();
        var maybe = await repository.FindOne(new ByIdSpecification<User>(request.UserId), cancellationToken);

        if (maybe.HasNoValue)
        {
            logger.LogInformation("User not found");
            return CommandResult.Failed(new ErrorData(
                ErrorCodes.NotFound, "User not found"));
        }

        var user = maybe.Value;

        var result = user.Disable(now);

        if (result.IsFailure)
        {
            logger.LogInformation("Failed to disable user");
            return CommandResult.Failed(result.Error);
        }

        var dbResult = await repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

        if (dbResult.IsSuccess)
        {
            await integrationEventPublisher.PublishAsync(
                new UserDisabledIntegrationEvent(user.Id, user.EmailAddress, now), cancellationToken);
            return CommandResult.Succeeded();
        }

        logger.LogInformation("Failed saving changes");

        return CommandResult.Failed(new ErrorData(
            ErrorCodes.SavingChanges, "Failed to save database"));
    }
}