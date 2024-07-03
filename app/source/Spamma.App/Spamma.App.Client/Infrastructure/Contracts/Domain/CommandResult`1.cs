using System.Text.Json.Serialization;
using Spamma.App.Client.Infrastructure.Constants;

namespace Spamma.App.Client.Infrastructure.Contracts.Domain;

public class CommandResult<T> : CommandResult
{
    private readonly T? _data;

    private CommandResult(ErrorData errorData)
        : base(errorData)
    {
    }

    private CommandResult(T data)
        : base()
    {
        this._data = data;
    }

    private CommandResult(CommandValidationResult commandValidationResult)
        : base(commandValidationResult)
    {
    }

    public T Data
    {
        get
        {
            if (this.Status != CommandResultStatus.Succeeded)
            {
                throw new System.InvalidOperationException("Data is only available when the status is Succeeded");
            }

            return this._data!;
        }
    }

    public new static CommandResult<T> Invalid(CommandValidationResult commandValidationResult)
    {
        return new CommandResult<T>(commandValidationResult);
    }

    public new static CommandResult<T> Failed(ErrorData errorData)
    {
        return new CommandResult<T>(errorData);
    }

    public static CommandResult<T> Succeeded(T data)
    {
        return new CommandResult<T>(data);
    }
}