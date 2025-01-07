using Spamma.App.Client.Infrastructure.Contracts.Domain;

namespace Spamma.App.Client.Infrastructure.Domain.SettingAggregate;

public record SetSecretKeyCommand(string Value) : ICommand;