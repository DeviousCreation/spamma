using FluentValidation;
using NodaTime;
using Spamma.App.Client.Infrastructure.Constants;
using Spamma.App.Client.Infrastructure.Contracts.Domain;
using Spamma.App.Client.Infrastructure.Domain.UserAggregate.CommandResults;
using Spamma.App.Client.Infrastructure.Domain.UserAggregate.Commands;
using Spamma.App.Infrastructure.Contracts;
using Spamma.App.Infrastructure.Contracts.Domain;
using Spamma.App.Infrastructure.Domain.UserAggregate.Aggregate;
using Spamma.App.Infrastructure.Domain.UserAggregate.IntegrationEvents;

namespace Spamma.App.Infrastructure.Domain.UserAggregate.CommandHandlers;

internal class CompleteAuthViaEmailCommandHandler(IEnumerable<IValidator<CompleteAuthViaEmailCommand>> validators, ILogger<CompleteAuthViaEmailCommandHandler> logger,
    IRepository<User> repository, IIntegrationEventPublisher integrationEventPublisher, IAuthTokenProvider authTokenProvider,
    IClock clock) : CommandHandler<CompleteAuthViaEmailCommand, CompleteAuthViaEmailCommandResult>(validators, logger)
{
    protected override async Task<CommandResult<CompleteAuthViaEmailCommandResult>> HandleInternal(CompleteAuthViaEmailCommand request, CancellationToken cancellationToken)
    {
        var tokenResult = await authTokenProvider.ProcessToken(request.Token);

        if (tokenResult.IsFailure)
        {
            logger.LogInformation("Failed processing token");
            return CommandResult<CompleteAuthViaEmailCommandResult>.Failed(tokenResult.Error);
        }

        var maybe = await repository.FindOne(new ByIdSpecification<User>(tokenResult.Value.UserId), cancellationToken);

        if (maybe.HasNoValue)
        {
            logger.LogInformation("User not found");
            return CommandResult<CompleteAuthViaEmailCommandResult>.Failed(new ErrorData(
                ErrorCodes.NotFound, "User not found"));
        }

        var user = maybe.Value;

        var now = clock.GetCurrentInstant().ToDateTimeUtc();

        var result = user.CompleteAuthViaEmail(now);

        if (result.IsFailure)
        {
            logger.LogInformation("Failed to complete auth via email");
            return CommandResult<CompleteAuthViaEmailCommandResult>.Failed(result.Error);
        }

        var dbResult = await repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

        if (dbResult.IsSuccess)
        {
            await integrationEventPublisher.PublishAsync(new EmailAuthCompletedIntegrationEvent(user.Id, now), cancellationToken);
            return CommandResult<CompleteAuthViaEmailCommandResult>.Succeeded(
                new CompleteAuthViaEmailCommandResult(user.Id, user.EmailAddress, user.Name));
        }

        logger.LogInformation("Failed saving changes");

        return CommandResult<CompleteAuthViaEmailCommandResult>.Failed(new ErrorData(
            ErrorCodes.SavingChanges, "Failed to save database"));
    }
}