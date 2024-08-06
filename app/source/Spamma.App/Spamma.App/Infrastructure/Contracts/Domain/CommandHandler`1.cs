using FluentValidation;
using Spamma.App.Client.Infrastructure.Contracts.Domain;

namespace Spamma.App.Infrastructure.Contracts.Domain;

internal abstract class CommandHandler<TCommand>(IEnumerable<IValidator<TCommand>> validators, ILogger logger)
    : ICommandHandler<TCommand>
    where TCommand : ICommand
{
    public async Task<CommandResult> Handle(TCommand request, CancellationToken cancellationToken)
    {
        var failures = validators
            .Select(v => v.Validate(request))
            .SelectMany(result => result.Errors)
            .Where(error => error != null)
            .ToList();

        if (failures.Count == 0)
        {
            return await this.HandleInternal(request, cancellationToken);
        }

        logger.LogInformation("Command validation failed");
        return CommandResult.Invalid(new CommandValidationResult
        {
            Failures = failures.Select(x => new CommandValidationFailure
            {
                PropertyName = x.PropertyName,
                ErrorMessage = x.ErrorMessage,
                AttemptedValue = x.AttemptedValue,
            }).ToList(),
        });
    }

    protected abstract Task<CommandResult> HandleInternal(TCommand request, CancellationToken cancellationToken);
}