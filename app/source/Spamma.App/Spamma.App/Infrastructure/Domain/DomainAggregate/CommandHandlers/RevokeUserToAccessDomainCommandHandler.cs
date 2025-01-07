using FluentValidation;
using NodaTime;
using Spamma.App.Client.Infrastructure.Constants;
using Spamma.App.Client.Infrastructure.Contracts.Domain;
using Spamma.App.Client.Infrastructure.Domain.DomainAggregate.Commands;
using Spamma.App.Infrastructure.Contracts.Domain;
using Spamma.App.Infrastructure.Domain.DomainAggregate.IntegrationEvents;

namespace Spamma.App.Infrastructure.Domain.DomainAggregate.CommandHandlers;

internal class RevokeUserToAccessDomainCommandHandler(
    IEnumerable<IValidator<RevokeUserToAccessDomainCommand>> validators,
    ILogger<RevokeUserToAccessDomainCommandHandler> logger,
    IRepository<Domain.DomainAggregate.Aggregate.Domain> repository,
    IIntegrationEventPublisher integrationEventPublisher,
    IClock clock) : CommandHandler<RevokeUserToAccessDomainCommand>(validators, logger)
{
    protected override async Task<CommandResult> HandleInternal(
        RevokeUserToAccessDomainCommand request, CancellationToken cancellationToken)
    {
        var maybe = await repository.FindOne(
            new ByIdSpecification<Aggregate.Domain>(request.DomainId), cancellationToken);

        if (maybe.HasNoValue)
        {
            logger.LogInformation("Domain not found");
            return CommandResult.Failed(new ErrorData(
                ErrorCodes.NotFound, "Domain not found"));
        }

        var domain = maybe.Value;

        var result = domain.RevokeUserAccess(request.UserId, clock.GetCurrentInstant().ToDateTimeUtc());

        if (result.IsFailure)
        {
            logger.LogInformation("Failed to revoke user permission");
            return CommandResult.Failed(result.Error);
        }

        var dbResult = await repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

        if (dbResult.IsSuccess)
        {
            await integrationEventPublisher.PublishAsync(
                new UserRevokedAccessToDomainIntegrationEvent(
                    request.DomainId, request.UserId), cancellationToken: cancellationToken);

            return CommandResult.Succeeded();
        }

        logger.LogInformation("Failed saving changes");
        return CommandResult.Failed(new ErrorData(
            ErrorCodes.SavingChanges, "Failed to save database"));
    }
}