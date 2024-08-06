using FluentValidation;
using Spamma.App.Client.Infrastructure.Constants;
using Spamma.App.Client.Infrastructure.Contracts.Domain;
using Spamma.App.Client.Infrastructure.Domain.EmailAggregate.Commands;
using Spamma.App.Infrastructure.Contracts.Domain;
using Spamma.App.Infrastructure.Domain.EmailAggregate.Aggregate;

namespace Spamma.App.Infrastructure.Domain.EmailAggregate.CommandHandlers;

internal class EmailReceivedCommandHandler(
    IEnumerable<IValidator<EmailReceivedCommand>> validators, ILogger<EmailReceivedCommandHandler> logger,
    IRepository<Email> repository) : CommandHandler<EmailReceivedCommand>(validators, logger)
{
    protected override async Task<CommandResult> HandleInternal(EmailReceivedCommand request, CancellationToken cancellationToken)
    {
        var entity = new Email(request.EmailId, request.SubdomainId, request.EmailAddress, request.Subject, request.WhenSent);

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