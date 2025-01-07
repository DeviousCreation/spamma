using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Spamma.App.Infrastructure.Querying.Entities;

namespace Spamma.App.Infrastructure.Database.TypeConfigurations.QueryableTypes;

internal class DomainConfiguration : IEntityTypeConfiguration<DomainQueryEntity>
{
    public void Configure(EntityTypeBuilder<DomainQueryEntity> builder)
    {
        builder.HasKey(domain => domain.Id);
        builder.ToView("vw_domain");

        builder.Property(domain => domain.Name);
        builder.Property(domain => domain.WhenCreated);
        builder.Property(domain => domain.IsDisable);
        builder.Property(domain => domain.WhenDisabled);

        builder.HasMany(domain => domain.DomainAccessPolicies)
            .WithOne(domainAccessPolicy => domainAccessPolicy.Domain)
            .HasForeignKey(domainAccessPolicy => domainAccessPolicy.DomainId);

        builder.HasMany(domain => domain.Subdomains)
            .WithOne(subdomain => subdomain.Domain)
            .HasForeignKey(subdomain => subdomain.DomainId);
    }
}