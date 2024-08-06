using FluentValidation;
using Spamma.App.Client.Infrastructure.Constants;
using Spamma.App.Client.Infrastructure.Contracts.Domain;
using Spamma.App.Client.Infrastructure.Domain.SubdomainAggregate.Commands;
using Spamma.App.Infrastructure.Contracts.Domain;
using Spamma.App.Infrastructure.Domain.SubdomainAggregate.Aggregate;

namespace Spamma.App.Infrastructure.Domain.SubdomainAggregate.CommandHandlers;

internal class CreateSubdomainCommandHandler(
    IEnumerable<IValidator<CreateSubdomainCommand>> validators, ILogger<CreateSubdomainCommandHandler> logger,
    IRepository<Subdomain> repository) : CommandHandler<CreateSubdomainCommand>(validators, logger)
{
    protected override async Task<CommandResult> HandleInternal(CreateSubdomainCommand request, CancellationToken cancellationToken)
    {
        var entity = new Subdomain(request.SubdomainId, request.SubdomainName, request.DomainId);

        repository.Add(entity);

        var dbResult = await repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

        if (dbResult.IsSuccess)
        {
            return CommandResult.Succeeded();
        }

        logger.LogInformation("Failed saving changes");
        return CommandResult.Failed(new ErrorData(
            ErrorCodes.SavingChanges, "Failed to save database"));
    }
}