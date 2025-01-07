using FluentValidation;
using NodaTime;
using Spamma.App.Client.Infrastructure.Constants;
using Spamma.App.Client.Infrastructure.Contracts.Domain;
using Spamma.App.Client.Infrastructure.Domain.SubdomainAggregate.Commands;
using Spamma.App.Infrastructure.Contracts.Domain;
using Spamma.App.Infrastructure.Domain.SubdomainAggregate.Aggregate;
using Spamma.App.Infrastructure.Domain.SubdomainAggregate.IntegrationEvents;

namespace Spamma.App.Infrastructure.Domain.SubdomainAggregate.CommandHandlers;

internal class AlterUserAccessToSubdomainCommandHandler(
    IEnumerable<IValidator<AlterUserAccessToSubdomainCommand>> validators,
    ILogger<AlterUserAccessToSubdomainCommandHandler> logger,
    IRepository<Subdomain> repository, IIntegrationEventPublisher integrationEventPublisher, IClock clock)
    : CommandHandler<AlterUserAccessToSubdomainCommand>(validators, logger)
{
    protected override async Task<CommandResult> HandleInternal(
        AlterUserAccessToSubdomainCommand request, CancellationToken cancellationToken)
    {
        var now = clock.GetCurrentInstant().ToDateTimeUtc();
        var domain = await repository.FindOne(
            new ByIdSpecification<Subdomain>(request.SubdomainId), cancellationToken);

        if (domain.HasNoValue)
        {
            logger.LogInformation("Subdomain not found");
            return CommandResult.Failed(new ErrorData(
                ErrorCodes.NotFound, "Subdomain not found"));
        }

        var entity = domain.Value;

        var result = entity.SetUserAccess(request.UserId, request.SubdomainAccessPolicyType, now);

        if (result.IsFailure)
        {
            logger.LogInformation("Failed to set user permission");
            return CommandResult.Failed(result.Error);
        }

        var dbResult = await repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

        if (dbResult.IsSuccess)
        {
            await integrationEventPublisher.PublishAsync(
                new UserAccessAlteredAgainstSubdomainIntegrationEvent(
                    request.SubdomainId, request.UserId, request.SubdomainAccessPolicyType, now),
                cancellationToken: cancellationToken);

            return CommandResult.Succeeded();
        }

        logger.LogInformation("Failed saving changes");
        return CommandResult.Failed(new ErrorData(
            ErrorCodes.SavingChanges, "Failed to save database"));
    }
}