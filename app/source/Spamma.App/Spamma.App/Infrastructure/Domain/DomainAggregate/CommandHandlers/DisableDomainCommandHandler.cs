using FluentValidation;
using NodaTime;
using Spamma.App.Client.Infrastructure.Constants;
using Spamma.App.Client.Infrastructure.Contracts.Domain;
using Spamma.App.Client.Infrastructure.Domain.DomainAggregate.Commands;
using Spamma.App.Infrastructure.Contracts.Domain;
using Spamma.App.Infrastructure.Domain.DomainAggregate.IntegrationEvents;

namespace Spamma.App.Infrastructure.Domain.DomainAggregate.CommandHandlers;

internal class DisableDomainCommandHandler(
    IEnumerable<IValidator<DisableDomainCommand>> validators,
    ILogger<DisableDomainCommandHandler> logger,
    IRepository<Domain.DomainAggregate.Aggregate.Domain> repository,
    IIntegrationEventPublisher integrationEventPublisher,
    IClock clock)
    : CommandHandler<DisableDomainCommand>(validators, logger)
{
    protected override async Task<CommandResult> HandleInternal(
        DisableDomainCommand request, CancellationToken cancellationToken)
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

        var result = domain.Disable(clock.GetCurrentInstant().ToDateTimeUtc());

        if (result.IsFailure)
        {
            logger.LogInformation("Failed to set user permission");
            return CommandResult.Failed(result.Error);
        }

        var dbResult = await repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

        if (dbResult.IsSuccess)
        {
            await integrationEventPublisher.PublishAsync(
                new DomainDisabledIntegrationEvent(request.DomainId), cancellationToken: cancellationToken);
            return CommandResult.Succeeded();
        }

        logger.LogInformation("Failed saving changes");
        return CommandResult.Failed(new ErrorData(
            ErrorCodes.SavingChanges, "Failed to save database"));
    }
}