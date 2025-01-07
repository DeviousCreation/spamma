using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Spamma.App.Infrastructure.Domain.SubdomainAggregate.Aggregate;

namespace Spamma.App.Infrastructure.Database.TypeConfigurations.MutableTypes;

internal class SubdomainConfiguration : IEntityTypeConfiguration<Subdomain>
{
    public void Configure(EntityTypeBuilder<Subdomain> builder)
    {
        builder.HasKey(subdomain => subdomain.Id);
        builder.ToTable("subdomain");
        builder.Ignore(subdomain => subdomain.DomainEvents);
        builder.Property(subdomain => subdomain.Id)
            .ValueGeneratedNever()
            .IsRequired();

        builder.Property(subdomain => subdomain.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.HasIndex(subdomain => subdomain.Name)
            .IsUnique();

        builder.Property(subdomain => subdomain.DomainId)
            .IsRequired();

        builder.Property(subdomain => subdomain.CreatedUserId)
            .IsRequired();

        builder.Property(subdomain => subdomain.WhenCreated)
            .IsRequired();

        builder.Ignore(subdomain => subdomain.WhenDisabled);
        builder.Ignore(subdomain => subdomain.IsDisabled);
        builder.Property("_whenDisabled")
            .HasColumnName("when_disabled");

        builder.OwnsMany(subdomains => subdomains.ChaosMonkeyAddresses, chaosMonkeyAddresses =>
        {
            chaosMonkeyAddresses.HasKey(chaosMonkeyAddress => chaosMonkeyAddress.Id);
            chaosMonkeyAddresses.ToTable("chaos_monkey_address");
            chaosMonkeyAddresses.Ignore(chaosMonkeyAddress => chaosMonkeyAddress.DomainEvents);
            chaosMonkeyAddresses.Property(chaosMonkeyAddress => chaosMonkeyAddress.Id).ValueGeneratedNever();

            chaosMonkeyAddresses.Property(chaosMonkeyAddress => chaosMonkeyAddress.EmailAddress)
                .IsRequired()
                .HasMaxLength(320);

            chaosMonkeyAddresses.Property(chaosMonkeyAddress => chaosMonkeyAddress.Type)
                .IsRequired();

            chaosMonkeyAddresses.HasIndex(chaosMonkeyAddress => chaosMonkeyAddress.EmailAddress)
                .IsUnique();
        }).UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.OwnsMany(subdomains => subdomains.SubdomainAccessPolicies, subdomainAccessPolicies =>
        {
            subdomainAccessPolicies.HasKey("SubdomainId", "Id", "WhenAssigned");
            subdomainAccessPolicies.ToTable("subdomain_access_policy");
            subdomainAccessPolicies.Ignore(subdomainAccessPolicy => subdomainAccessPolicy.DomainEvents);
            subdomainAccessPolicies.Property(subdomainAccessPolicy => subdomainAccessPolicy.Id)
                .ValueGeneratedNever()
                .HasColumnName("user_id");
            subdomainAccessPolicies.Ignore(subdomainAccessPolicy => subdomainAccessPolicy.IsRevoked);
            subdomainAccessPolicies.Ignore(subdomainAccessPolicy => subdomainAccessPolicy.WhenRevoked);
            subdomainAccessPolicies.Property(typeof(DateTime?), "_whenRevoked")
                .HasColumnName("when_revoked");

            subdomainAccessPolicies.Property(subdomainAccessPolicy => subdomainAccessPolicy.PolicyType)
                .IsRequired();
            subdomainAccessPolicies.Property(subdomainAccessPolicy => subdomainAccessPolicy.WhenAssigned)
                .IsRequired();
        }).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}