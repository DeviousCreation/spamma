using FluentValidation;
using NodaTime;
using Spamma.App.Client.Infrastructure.Constants;
using Spamma.App.Client.Infrastructure.Contracts.Domain;
using Spamma.App.Client.Infrastructure.Domain.UserAggregate.Commands;
using Spamma.App.Infrastructure.Contracts.Domain;
using Spamma.App.Infrastructure.Domain.UserAggregate.Aggregate;
using Spamma.App.Infrastructure.Domain.UserAggregate.IntegrationEvents;
using Spamma.App.Infrastructure.Domain.UserAggregate.QuerySpecifications;

namespace Spamma.App.Infrastructure.Domain.UserAggregate.CommandHandlers;

internal class StartAuthViaEmailCommandHandler(IEnumerable<IValidator<StartAuthViaEmailCommand>> validators, ILogger<InviteUserCommandHandler> logger,
    IRepository<User> repository, IIntegrationEventPublisher integrationEventPublisher, IClock clock) : CommandHandler<StartAuthViaEmailCommand>(validators, logger)
{
    protected override async Task<CommandResult> HandleInternal(StartAuthViaEmailCommand request, CancellationToken cancellationToken)
    {
        var maybe = await repository.FindOne(new ByEmailAddressSpecification(request.EmailAddress), cancellationToken);

        if (maybe.HasNoValue)
        {
            logger.LogInformation("User not found");
            return CommandResult.Failed(new ErrorData(
                ErrorCodes.NotFound, "User not found"));
        }

        var user = maybe.Value;

        var now = clock.GetCurrentInstant().ToDateTimeUtc();

        var result = user.StartAuthViaEmail(now);

        if (result.IsFailure)
        {
            logger.LogInformation("Failed to start auth via email");
            return CommandResult.Failed(result.Error);
        }

        var dbResult = await repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

        if (dbResult.IsSuccess)
        {
            await integrationEventPublisher.PublishAsync(new EmailAuthStartedIntegrationEvent(user.Id, user.EmailAddress, user.SecurityStamp, now), cancellationToken);
            return CommandResult.Succeeded();
        }

        logger.LogInformation("Failed saving changes");

        return CommandResult.Failed(new ErrorData(
            ErrorCodes.SavingChanges, "Failed to save database"));
    }
}