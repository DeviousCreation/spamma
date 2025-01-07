using FluentValidation;
using NodaTime;
using Spamma.App.Client.Infrastructure.Constants;
using Spamma.App.Client.Infrastructure.Contracts.Domain;
using Spamma.App.Client.Infrastructure.Contracts.Web;
using Spamma.App.Client.Infrastructure.Domain.DomainAggregate.Commands;
using Spamma.App.Client.Infrastructure.Web;
using Spamma.App.Infrastructure.Contracts.Domain;
using Spamma.App.Infrastructure.Domain.DomainAggregate.IntegrationEvents;

namespace Spamma.App.Infrastructure.Domain.DomainAggregate.CommandHandlers;

internal class CreateDomainCommandHandler(
    IEnumerable<IValidator<CreateDomainCommand>> validators, ILogger<CreateDomainCommandHandler> logger,
    IRepository<Domain.DomainAggregate.Aggregate.Domain> repository,
    IIntegrationEventPublisher integrationEventPublisher, IClock clock, ICurrentUserServiceFactory currentUserServiceFactory)
    : CommandHandler<CreateDomainCommand>(validators, logger)
{
    protected override async Task<CommandResult> HandleInternal(CreateDomainCommand request, CancellationToken cancellationToken)
    {
        var currentUserService = currentUserServiceFactory.Create();
        var currentUser = await currentUserService.GetCurrentUserAsync();
        if (currentUser.HasNoValue)
        {
            logger.LogInformation("Current user not found");
            return CommandResult.Failed(new ErrorData(
                ErrorCodes.CurrentUserNotFound, "Current user not found"));
        }

        var domain = new DomainAggregate.Aggregate.Domain(
            Guid.NewGuid(), request.DomainName, currentUser.Value.Id, clock.GetCurrentInstant().ToDateTimeUtc());

        repository.Add(domain);

        var dbResult = await repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

        if (dbResult.IsSuccess)
        {
            await integrationEventPublisher.PublishAsync(
                 new DomainCreatedIntegrationEvent(domain.Id, request.DomainName), cancellationToken: cancellationToken);
            return CommandResult.Succeeded();
        }

        logger.LogInformation("Failed saving changes");
        return CommandResult.Failed(new ErrorData(
            ErrorCodes.SavingChanges, "Failed to save database"));
    }
}