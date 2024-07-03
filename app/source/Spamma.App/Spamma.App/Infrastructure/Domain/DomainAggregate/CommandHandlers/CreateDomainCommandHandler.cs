using FluentValidation;
using Spamma.App.Client.Infrastructure.Constants;
using Spamma.App.Client.Infrastructure.Contracts.Domain;
using Spamma.App.Client.Infrastructure.Domain.DomainAggregate.Commands;
using Spamma.App.Infrastructure.Contracts.Domain;

namespace Spamma.App.Infrastructure.Domain.DomainAggregate.CommandHandlers;

public class CreateDomainCommandHandler(
    IEnumerable<IValidator<CreateDomainCommand>> validators, ILogger<CreateDomainCommandHandler> logger, IRepository<Domain.DomainAggregate.Aggregate.Domain> repository)
    : CommandHandler<CreateDomainCommand>(validators, logger)
{
    protected override async Task<CommandResult> HandleInternal(CreateDomainCommand request, CancellationToken cancellationToken)
    {
        var domain = new DomainAggregate.Aggregate.Domain(
            request.DomainId, request.DomainName);

        repository.Add(domain);

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