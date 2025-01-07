using ResultMonad;
using Spamma.App.Client.Infrastructure.Constants;
using Spamma.App.Client.Infrastructure.Contracts.Domain;
using Spamma.App.Infrastructure.Contracts.Domain;

namespace Spamma.App.Infrastructure.Domain.SettingAggregate.Aggregate;

internal sealed class Setting : Entity, IAggregateRoot
{
    internal Setting(Guid id, string value)
        : base(id)
    {
        this.Value = value;
    }

    private Setting()
    {
    }

    internal string Value { get; private set; }

    internal ResultWithError<ErrorData> UpdateValue(string value)
    {
        if (this.Value == value)
        {
            return ResultWithError.Fail(new ErrorData(ErrorCodes.SettingValueNotChanged, "Setting value has not changed."));
        }

        this.Value = value;

        return ResultWithError.Ok<ErrorData>();
    }
}