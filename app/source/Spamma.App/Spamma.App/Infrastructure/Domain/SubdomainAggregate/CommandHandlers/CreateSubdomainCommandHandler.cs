using FluentValidation;
using NodaTime;
using Spamma.App.Client.Infrastructure.Constants;
using Spamma.App.Client.Infrastructure.Contracts.Domain;
using Spamma.App.Client.Infrastructure.Contracts.Web;
using Spamma.App.Client.Infrastructure.Domain.SubdomainAggregate.Commands;
using Spamma.App.Client.Infrastructure.Web;
using Spamma.App.Infrastructure.Contracts.Domain;
using Spamma.App.Infrastructure.Domain.SubdomainAggregate.Aggregate;

namespace Spamma.App.Infrastructure.Domain.SubdomainAggregate.CommandHandlers;

internal class CreateSubdomainCommandHandler(
    IEnumerable<IValidator<CreateSubdomainCommand>> validators, ILogger<CreateSubdomainCommandHandler> logger,
    IRepository<Subdomain> repository, IClock clock, ICurrentUserServiceFactory currentUserServiceFactory)
    : CommandHandler<CreateSubdomainCommand>(validators, logger)
{
    protected override async Task<CommandResult> HandleInternal(
        CreateSubdomainCommand request, CancellationToken cancellationToken)
    {
        var currentUserService = currentUserServiceFactory.Create();
        var currentUser = await currentUserService.GetCurrentUserAsync();
        if (currentUser.HasNoValue)
        {
            logger.LogInformation("Current user not found");
            return CommandResult.Failed(new ErrorData(
                ErrorCodes.CurrentUserNotFound, "Current user not found"));
        }

        var entity = new Subdomain(
            Guid.NewGuid(), request.SubdomainName, request.DomainId, currentUser.Value.Id,
            clock.GetCurrentInstant().ToDateTimeUtc());

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