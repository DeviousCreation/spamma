using FluentValidation;
using NodaTime;
using Spamma.App.Client.Infrastructure.Constants;
using Spamma.App.Client.Infrastructure.Contracts.Domain;
using Spamma.App.Client.Infrastructure.Domain.SubdomainAggregate.Commands;
using Spamma.App.Infrastructure.Contracts.Domain;
using Spamma.App.Infrastructure.Domain.SubdomainAggregate.Aggregate;
using Spamma.App.Infrastructure.Domain.SubdomainAggregate.IntegrationEvents;

namespace Spamma.App.Infrastructure.Domain.SubdomainAggregate.CommandHandlers;

internal class RevokeUserToAccessSubdomainCommandHandler(
    IEnumerable<IValidator<RevokeUserToAccessSubdomainCommand>> validators,
    ILogger<RevokeUserToAccessSubdomainCommandHandler> logger,
    IRepository<Subdomain> repository,
    IIntegrationEventPublisher integrationEventPublisher,
    IClock clock) : CommandHandler<RevokeUserToAccessSubdomainCommand>(validators, logger)
{
    protected override async Task<CommandResult> HandleInternal(RevokeUserToAccessSubdomainCommand request, CancellationToken cancellationToken)
    {
        var now = clock.GetCurrentInstant().ToDateTimeUtc();
        var maybe = await repository.FindOne(new ByIdSpecification<Subdomain>(request.SubdomainId), cancellationToken);

        if (maybe.HasNoValue)
        {
            logger.LogInformation("Subdomain not found");
            return CommandResult.Failed(new ErrorData(
                ErrorCodes.NotFound, "Subdomain not found"));
        }

        var domain = maybe.Value;

        var result = domain.RevokeUserAccess(request.UserId, now);

        if (result.IsFailure)
        {
            logger.LogInformation("Failed to revoke user permission");
            return CommandResult.Failed(result.Error);
        }

        var dbResult = await repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

        if (dbResult.IsSuccess)
        {
            await integrationEventPublisher.PublishAsync(
                new UserRevokedAccessToSubdomainIntegrationEvent(request.SubdomainId, request.UserId, now),
                cancellationToken: cancellationToken);

            return CommandResult.Succeeded();
        }

        logger.LogInformation("Failed saving changes");
        return CommandResult.Failed(new ErrorData(
            ErrorCodes.SavingChanges, "Failed to save database"));
    }
}