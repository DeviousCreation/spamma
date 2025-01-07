using FluentValidation;
using Spamma.App.Client.Infrastructure.Constants;
using Spamma.App.Client.Infrastructure.Contracts.Domain;
using Spamma.App.Client.Infrastructure.Domain.SettingAggregate;
using Spamma.App.Infrastructure.Constants;
using Spamma.App.Infrastructure.Contracts.Domain;
using Spamma.App.Infrastructure.Domain.DomainAggregate.CommandHandlers;
using Spamma.App.Infrastructure.Domain.DomainAggregate.IntegrationEvents;
using Spamma.App.Infrastructure.Domain.SettingAggregate.Aggregate;

namespace Spamma.App.Infrastructure.Domain.SettingAggregate.CommandHandlers;

internal class SetSecretKeyCommandHandler(
    IEnumerable<IValidator<SetSecretKeyCommand>> validators,
    ILogger<SetSecretKeyCommandHandler> logger,
    IRepository<Domain.SettingAggregate.Aggregate.Setting> repository,
    IIntegrationEventPublisher integrationEventPublisher)
    : CommandHandler<SetSecretKeyCommand>(validators, logger)
{
    protected override async Task<CommandResult> HandleInternal(SetSecretKeyCommand request, CancellationToken cancellationToken)
    {
        var maybe = await repository.FindOne(
            new ByIdSpecification<Aggregate.Setting>(SettingConstants.SecretKey), cancellationToken);

        Setting setting;
        if (maybe.HasNoValue)
        {
            setting = new Setting(SettingConstants.SecretKey, request.Value);
            repository.Add(setting);
        }
        else
        {
            setting = maybe.Value;
            var result = setting.UpdateValue(request.Value);
            if (result.IsFailure)
            {
                return CommandResult.Failed(result.Error);
            }
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