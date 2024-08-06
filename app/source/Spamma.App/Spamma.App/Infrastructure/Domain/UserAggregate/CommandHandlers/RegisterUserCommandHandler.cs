using FluentValidation;
using NodaTime;
using Spamma.App.Client.Infrastructure.Constants;
using Spamma.App.Client.Infrastructure.Contracts.Domain;
using Spamma.App.Client.Infrastructure.Domain.UserAggregate.Commands;
using Spamma.App.Infrastructure.Contracts.Domain;
using Spamma.App.Infrastructure.Domain.UserAggregate.Aggregate;
using Spamma.App.Infrastructure.Domain.UserAggregate.IntegrationEvents;

namespace Spamma.App.Infrastructure.Domain.UserAggregate.CommandHandlers;

internal class RegisterUserCommandHandler(
    IEnumerable<IValidator<RegisterUserCommand>> validators, ILogger<InviteUserCommandHandler> logger,
    IRepository<User> repository, IIntegrationEventPublisher integrationEventPublisher, IClock clock) : CommandHandler<RegisterUserCommand>(validators, logger)
{
    protected override async Task<CommandResult> HandleInternal(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var user = new User(request.UserId, request.EmailAddress, request.WhenCreated);

        repository.Add(user);

        var dbResult = await repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

        if (dbResult.IsSuccess)
        {
            await integrationEventPublisher.PublishAsync(
                new UserRegisteredIntegrationEvent(user.Id, user.EmailAddress, user.SecurityStamp, clock.GetCurrentInstant().ToDateTimeUtc()),
                cancellationToken);
            return CommandResult.Succeeded();
        }

        logger.LogInformation("Failed saving changes");
        return CommandResult.Failed(new ErrorData(
            ErrorCodes.SavingChanges, "Failed to save database"));
    }
}