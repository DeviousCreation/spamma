﻿using Spamma.App.Client.Infrastructure.Contracts.Domain;

namespace Spamma.App.Client.Infrastructure.Domain.UserAggregate.Commands;

public record ConfirmUserInvitationCommand(string Token) : ICommand;