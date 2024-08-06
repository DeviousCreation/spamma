using DotNetCore.CAP;
using FluentValidation;
using Spamma.App.Client.Infrastructure.Constants;
using Spamma.App.Client.Infrastructure.Contracts.Domain;
using Spamma.App.Client.Infrastructure.Domain.DomainAggregate.Commands;
using Spamma.App.Infrastructure.Constants;
using Spamma.App.Infrastructure.Contracts.Domain;
using Spamma.App.Infrastructure.Domain.DomainAggregate.IntegrationEvents;

namespace Spamma.App.Infrastructure.Domain.DomainAggregate.CommandHandlers;

internal class CreateDomainCommandHandler(
    IEnumerable<IValidator<CreateDomainCommand>> validators, ILogger<CreateDomainCommandHandler> logger,
    IRepository<Domain.DomainAggregate.Aggregate.Domain> repository, IIntegrationEventPublisher integrationEventPublisher)
    : CommandHandler<CreateDomainCommand>(validators, logger)
{
    protected override async Task<CommandResult> HandleInternal(CreateDomainCommand request, CancellationToken cancellationToken)
    {
        var domain = new DomainAggregate.Aggregate.Domain(
            request.DomainId, request.DomainName, request.CreatedByUserId, request.WhenCreated);

        repository.Add(domain);

        var dbResult = await repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

        if (dbResult.IsSuccess)
        {
            await integrationEventPublisher.PublishAsync(
                 new DomainCreatedIntegrationEvent(request.DomainId, request.DomainName), cancellationToken: cancellationToken);
            return CommandResult.Succeeded();
        }

        logger.LogInformation("Failed saving changes");
        return CommandResult.Failed(new ErrorData(
            ErrorCodes.SavingChanges, "Failed to save database"));
    }
}