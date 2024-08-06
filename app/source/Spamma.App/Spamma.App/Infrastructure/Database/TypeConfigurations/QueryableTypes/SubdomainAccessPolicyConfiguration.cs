using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Spamma.App.Infrastructure.Querying.Entities;

namespace Spamma.App.Infrastructure.Database.TypeConfigurations.QueryableTypes;

internal class SubdomainAccessPolicyConfiguration : IEntityTypeConfiguration<SubdomainAccessPolicyQueryEntity>
{
    public void Configure(EntityTypeBuilder<SubdomainAccessPolicyQueryEntity> builder)
    {
        builder.HasKey(domainAccessPolicy => new
        {
            domainAccessPolicy.UserId,
            domainAccessPolicy.SubdomainId,
        });
        builder.ToView("vw_subdomain_access_policy");
    }
}