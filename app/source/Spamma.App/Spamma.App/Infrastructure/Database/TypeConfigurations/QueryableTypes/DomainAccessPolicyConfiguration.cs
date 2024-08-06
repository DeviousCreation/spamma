using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Spamma.App.Infrastructure.Querying.Entities;

namespace Spamma.App.Infrastructure.Database.TypeConfigurations.QueryableTypes;

internal class DomainAccessPolicyConfiguration : IEntityTypeConfiguration<DomainAccessPolicyQueryEntity>
{
    public void Configure(EntityTypeBuilder<DomainAccessPolicyQueryEntity> builder)
    {
        builder.HasKey(domainAccessPolicy => new
        {
            domainAccessPolicy.UserId,
            domainAccessPolicy.DomainId,
        });
        builder.ToView("vw_domain_access_policy");
    }
}