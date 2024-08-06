using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Spamma.App.Infrastructure.Database.TypeConfigurations.MutableTypes;

internal class DomainConfiguration : IEntityTypeConfiguration<Domain.DomainAggregate.Aggregate.Domain>
{
    public void Configure(EntityTypeBuilder<Domain.DomainAggregate.Aggregate.Domain> builder)
    {
        builder.HasKey(domain => domain.Id);
        builder.ToTable("domain");
        builder.Ignore(domain => domain.DomainEvents);
        builder.Property(domain => domain.Id)
            .ValueGeneratedNever()
            .IsRequired();
        builder.Property(domain => domain.Name)
            .IsRequired()
            .HasMaxLength(255);
        builder.HasIndex(domain => domain.Name)
            .IsUnique();
        builder.Property(domain => domain.CreatedUserId)
            .IsRequired();
        builder.Property(domain => domain.WhenCreated)
            .IsRequired();

        builder.OwnsMany(domains => domains.DomainAccessPolicies, domainAccessPolicies =>
        {
            domainAccessPolicies.HasKey("DomainId", "Id");
            domainAccessPolicies.ToTable("domain_access_policy");
            domainAccessPolicies.Ignore(domainAccessPolicy => domainAccessPolicy.DomainEvents);
            domainAccessPolicies.Property(domainAccessPolicy => domainAccessPolicy.Id)
                .ValueGeneratedNever()
                .IsRequired()
                .HasColumnName("user_id");
            domainAccessPolicies.Ignore(domainAccessPolicy => domainAccessPolicy.IsRevoked);
            domainAccessPolicies.Ignore(domainAccessPolicy => domainAccessPolicy.WhenRevoked);
            domainAccessPolicies.Property(typeof(DateTime?), "_whenRevoked")
                .HasColumnName("when_revoked");
            domainAccessPolicies.Property(domainAccessPolicy => domainAccessPolicy.PolicyType)
                .IsRequired();
            domainAccessPolicies.Property(domainAccessPolicy => domainAccessPolicy.WhenAssigned)
                .IsRequired();
        }).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}