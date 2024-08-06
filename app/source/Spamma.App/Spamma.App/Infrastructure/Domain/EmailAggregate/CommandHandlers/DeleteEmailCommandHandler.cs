using FluentValidation;
using Spamma.App.Client.Infrastructure.Constants;
using Spamma.App.Client.Infrastructure.Contracts.Domain;
using Spamma.App.Client.Infrastructure.Domain.EmailAggregate.Commands;
using Spamma.App.Infrastructure.Contracts.Domain;
using Spamma.App.Infrastructure.Domain.EmailAggregate.Aggregate;

namespace Spamma.App.Infrastructure.Domain.EmailAggregate.CommandHandlers;

internal class DeleteEmailCommandHandler(
    IEnumerable<IValidator<DeleteEmailCommand>> validators, ILogger<DeleteEmailCommandHandler> logger,
    IRepository<Email> repository) : CommandHandler<DeleteEmailCommand>(validators, logger)
{
    protected override async Task<CommandResult> HandleInternal(DeleteEmailCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.FindOne(new ByIdSpecification<Email>(request.EmailId), cancellationToken);

        if (entity.HasNoValue)
        {
            logger.LogInformation("Email not found");

            return CommandResult.Failed(new ErrorData(
                ErrorCodes.NotFound, "Email not found"));
        }

        repository.Delete(entity.Value);

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