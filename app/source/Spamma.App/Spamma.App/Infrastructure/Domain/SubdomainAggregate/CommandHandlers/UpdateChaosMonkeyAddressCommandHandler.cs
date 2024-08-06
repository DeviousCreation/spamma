using FluentValidation;
using Spamma.App.Client.Infrastructure.Constants;
using Spamma.App.Client.Infrastructure.Contracts.Domain;
using Spamma.App.Client.Infrastructure.Domain.SubdomainAggregate.Commands;
using Spamma.App.Infrastructure.Contracts.Domain;
using Spamma.App.Infrastructure.Domain.SubdomainAggregate.Aggregate;

namespace Spamma.App.Infrastructure.Domain.SubdomainAggregate.CommandHandlers;

internal class UpdateChaosMonkeyAddressCommandHandler(
    IEnumerable<IValidator<UpdateChaosMonkeyAddressCommand>> validators, ILogger<UpdateChaosMonkeyAddressCommandHandler> logger,
    IRepository<Subdomain> repository) : CommandHandler<UpdateChaosMonkeyAddressCommand>(validators, logger)
{
    protected override async Task<CommandResult> HandleInternal(UpdateChaosMonkeyAddressCommand request, CancellationToken cancellationToken)
    {
        var maybe = await repository.FindOne(new ByIdSpecification<Subdomain>(request.SubdomainId), cancellationToken);
        if (maybe.HasNoValue)
        {
            logger.LogInformation("Subdomain not found");
            return CommandResult.Failed(new ErrorData(
                ErrorCodes.NotFound, "Subdomain not found"));
        }

        var entity = maybe.Value;

        var result = entity.UpdateChaosMonkeyAddress(request.AddressId, request.Type);

        if (result.IsFailure)
        {
            logger.LogInformation("Failed to update chaos monkey address");
            return CommandResult.Failed(result.Error);
        }

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