using Spamma.App.Client.Infrastructure.Contracts.Domain;
using Spamma.App.Client.Infrastructure.Domain.UserAggregate.CommandResults;

namespace Spamma.App.Client.Infrastructure.Domain.UserAggregate.Commands;

public record CompleteAuthViaEmailCommand(string Token) : ICommand<CompleteAuthViaEmailCommandResult>;