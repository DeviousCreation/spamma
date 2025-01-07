using System.Text.Json.Serialization;
using Spamma.App.Client.Infrastructure.Constants;

namespace Spamma.App.Client.Infrastructure.Contracts.Domain;

[JsonConverter(typeof(CommandResultConverter))]
public class CommandResult
{
    private readonly ErrorData? _errorData;
    private readonly CommandValidationResult? _commandValidationResult;

    protected CommandResult()
    {
        this.Status = CommandResultStatus.Succeeded;
    }

    protected CommandResult(ErrorData errorData)
    {
        this.Status = CommandResultStatus.Failed;
        this._errorData = errorData;
    }

    protected CommandResult(CommandValidationResult commandValidationResult)
    {
        this.Status = CommandResultStatus.Invalid;
        this._commandValidationResult = commandValidationResult;
    }

    [JsonConstructor]
    private CommandResult(ErrorData? errorData, CommandValidationResult? commandValidationResult)
    {
        this._errorData = errorData;
        this._commandValidationResult = commandValidationResult;
    }

    public ErrorData ErrorData
    {
        get
        {
            if (this.Status != CommandResultStatus.Failed)
            {
                throw new System.InvalidOperationException("ErrorData is only available when the status is Failed");
            }

            return this._errorData!;
        }
    }

    public CommandValidationResult ValidationResult
    {
        get
        {
            if (this.Status != CommandResultStatus.Invalid)
            {
                throw new InvalidOperationException("ValidationResult is only available when the status is Invalid");
            }

            return this._commandValidationResult!;
        }
    }

    public CommandResultStatus Status { get; protected set; }

    public static CommandResult Invalid(CommandValidationResult commandValidationFailure)
    {
        return new CommandResult(commandValidationFailure);
    }

    public static CommandResult Failed(ErrorData errorData)
    {
        return new CommandResult(errorData);
    }

    public static CommandResult Succeeded()
    {
        return new CommandResult();
    }

    public static CommandResult Unauthorized()
    {
        return new CommandResult(new ErrorData(ErrorCodes.NotAuthorized));
    }
}