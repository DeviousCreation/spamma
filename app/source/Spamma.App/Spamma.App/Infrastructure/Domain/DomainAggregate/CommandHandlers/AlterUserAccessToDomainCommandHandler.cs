using FluentValidation;
using Spamma.App.Client.Infrastructure.Constants;
using Spamma.App.Client.Infrastructure.Contracts.Domain;
using Spamma.App.Client.Infrastructure.Domain.DomainAggregate.Commands;
using Spamma.App.Infrastructure.Contracts.Domain;
using Spamma.App.Infrastructure.Domain.DomainAggregate.IntegrationEvents;

namespace Spamma.App.Infrastructure.Domain.DomainAggregate.CommandHandlers;

internal class AlterUserAccessToDomainCommandHandler(IEnumerable<IValidator<AlterUserAccessToDomainCommand>> validators,
    ILogger<AlterUserAccessToDomainCommandHandler> logger,
    IRepository<Domain.DomainAggregate.Aggregate.Domain> repository, IIntegrationEventPublisher integrationEventPublisher)
    : CommandHandler<AlterUserAccessToDomainCommand>(validators, logger)
{
    protected override async Task<CommandResult> HandleInternal(AlterUserAccessToDomainCommand request, CancellationToken cancellationToken)
    {
        var domain = await repository.FindOne(new ByIdSpecification<Aggregate.Domain>(request.DomainId), cancellationToken);

        if (domain.HasNoValue)
        {
            logger.LogInformation("Domain not found");
            return CommandResult.Failed(new ErrorData(
                ErrorCodes.NotFound, "Domain not found"));
        }

        var entity = domain.Value;

        var result = entity.SetUserAccess(request.UserId, request.DomainAccessPolicyType, request.WhenUpdated);

        if (result.IsFailure)
        {
            logger.LogInformation("Failed to set user permission");
            return CommandResult.Failed(result.Error);
        }

        var dbResult = await repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

        if (dbResult.IsSuccess)
        {
            await integrationEventPublisher.PublishAsync(
                new UserAccessAlteredAgainstDomainIntegrationEvent(request.DomainId, request.DomainId, request.DomainAccessPolicyType), cancellationToken: cancellationToken);

            return CommandResult.Succeeded();
        }

        logger.LogInformation("Failed saving changes");
        return CommandResult.Failed(new ErrorData(
            ErrorCodes.SavingChanges, "Failed to save database"));
    }
}