using FluentValidation;
using NodaTime;
using Spamma.App.Client.Infrastructure.Constants;
using Spamma.App.Client.Infrastructure.Contracts.Domain;
using Spamma.App.Client.Infrastructure.Domain.UserAggregate.Commands;
using Spamma.App.Infrastructure.Contracts;
using Spamma.App.Infrastructure.Contracts.Domain;
using Spamma.App.Infrastructure.Domain.UserAggregate.Aggregate;
using Spamma.App.Infrastructure.Domain.UserAggregate.IntegrationEvents;

namespace Spamma.App.Infrastructure.Domain.UserAggregate.CommandHandlers;

internal class VerifyUserCommandHandler(
    IEnumerable<IValidator<VerifyUserCommand>> validators, ILogger<ConfirmUserInvitationCommandHandler> logger,
    IRepository<User> repository, IIntegrationEventPublisher integrationEventPublisher, IAuthTokenProvider authTokenProvider,
    IClock clock) : CommandHandler<VerifyUserCommand>(validators, logger)
{
    protected override async Task<CommandResult> HandleInternal(VerifyUserCommand request, CancellationToken cancellationToken)
    {
        var tokenResult = await authTokenProvider.ProcessToken(request.Token);
        if (tokenResult.IsFailure)
        {
            logger.LogInformation("Failed processing token");
            return CommandResult.Failed(tokenResult.Error);
        }

        var maybe = await repository.FindOne(new ByIdSpecification<User>(tokenResult.Value.UserId), cancellationToken);

        if (maybe.HasNoValue)
        {
            logger.LogInformation("User not found");
            return CommandResult.Failed(new ErrorData(
                ErrorCodes.NotFound, "User not found"));
        }

        var user = maybe.Value;

        var now = clock.GetCurrentInstant().ToDateTimeUtc();

        var result = user.ConfirmInvitation(now);

        if (result.IsFailure)
        {
            logger.LogInformation("Failed to confirm invitation");
            return CommandResult.Failed(result.Error);
        }

        var dbResult = await repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

        if (dbResult.IsSuccess)
        {
            await integrationEventPublisher.PublishAsync(new UserVerifiedIntegrationEvent(user.Id, now), cancellationToken);
            return CommandResult.Succeeded();
        }

        logger.LogInformation("Failed saving changes");

        return CommandResult.Failed(new ErrorData(
            ErrorCodes.SavingChanges, "Failed to save database"));
    }
}