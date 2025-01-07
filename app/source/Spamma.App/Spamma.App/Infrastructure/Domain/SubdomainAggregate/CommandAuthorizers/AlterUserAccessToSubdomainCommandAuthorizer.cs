﻿using MediatR.Behaviors.Authorization;
using Spamma.App.Client.Infrastructure.Domain.SubdomainAggregate.Commands;
using Spamma.App.Infrastructure.AuthorizationRequirements;

namespace Spamma.App.Infrastructure.Domain.SubdomainAggregate.CommandAuthorizers;

public class AlterUserAccessToSubdomainCommandAuthorizer : AbstractRequestAuthorizer<AlterUserAccessToSubdomainCommand>
{
    public override void BuildPolicy(AlterUserAccessToSubdomainCommand request)
    {
        this.UseRequirement(new MustBeInRoleRequirement
        {
            Role = "subdomain-user-management",
        });
    }
}