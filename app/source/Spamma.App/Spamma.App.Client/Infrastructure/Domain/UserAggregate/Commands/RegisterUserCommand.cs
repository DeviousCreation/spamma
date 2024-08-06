using Spamma.App.Client.Infrastructure.Contracts.Domain;

namespace Spamma.App.Client.Infrastructure.Domain.UserAggregate.Commands;

public record RegisterUserCommand(Guid UserId, string FirstName, string LastName, string EmailAddress) : ICommand
{
    public DateTime WhenCreated { get; set; }
}