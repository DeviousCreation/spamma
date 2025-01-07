using FluentValidation;
using NodaTime;
using Spamma.App.Client.Infrastructure.Constants;
using Spamma.App.Client.Infrastructure.Contracts.Domain;
using Spamma.App.Client.Infrastructure.Domain.SubdomainAggregate.Commands;
using Spamma.App.Infrastructure.Contracts.Domain;
using Spamma.App.Infrastructure.Domain.SubdomainAggregate.Aggregate;
using Spamma.App.Infrastructure.Domain.SubdomainAggregate.IntegrationEvents;

namespace Spamma.App.Infrastructure.Domain.SubdomainAggregate.CommandHandlers;

internal class DisableSubdomainCommandHandler(
    IEnumerable<IValidator<DisableSubdomainCommand>> validators,
    ILogger<DisableSubdomainCommandHandler> logger,
    IRepository<Subdomain> repository,
    IIntegrationEventPublisher integrationEventPublisher,
    IClock clock)
    : CommandHandler<DisableSubdomainCommand>(validators, logger)
{
    protected override async Task<CommandResult> HandleInternal(
        DisableSubdomainCommand request, CancellationToken cancellationToken)
    {
        var now = clock.GetCurrentInstant().ToDateTimeUtc();
        var maybe = await repository.FindOne(new ByIdSpecification<Subdomain>(request.SubdomainId), cancellationToken);

        if (maybe.HasNoValue)
        {
            logger.LogInformation("Domain not found");
            return CommandResult.Failed(new ErrorData(
                ErrorCodes.NotFound, "Domain not found"));
        }

        var subdomain = maybe.Value;

        var result = subdomain.Disable(now);

        if (result.IsFailure)
        {
            logger.LogInformation("Failed to set user permission");
            return CommandResult.Failed(result.Error);
        }

        var dbResult = await repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

        if (dbResult.IsSuccess)
        {
            await integrationEventPublisher.PublishAsync(
                new SubdomainDisabledIntegrationEvent(request.SubdomainId, now), cancellationToken: cancellationToken);
            return CommandResult.Succeeded();
        }

        logger.LogInformation("Failed saving changes");
        return CommandResult.Failed(new ErrorData(
            ErrorCodes.SavingChanges, "Failed to save database"));
    }
}