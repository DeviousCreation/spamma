using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Spamma.App.Infrastructure.Querying.Entities;

namespace Spamma.App.Infrastructure.Database.TypeConfigurations.QueryableTypes;

internal class SubdomainConfiguration : IEntityTypeConfiguration<SubdomainQueryEntity>
{
    public void Configure(EntityTypeBuilder<SubdomainQueryEntity> builder)
    {
        builder.HasKey(subdomain => subdomain.Id);
        builder.ToView("vw_subdomain");

        builder.Property(subdomain => subdomain.Name);
        builder.Property(subdomain => subdomain.WhenCreated);
        builder.Property(subdomain => subdomain.IsDisabled);
        builder.Property(subdomain => subdomain.WhenDisabled);

        builder.HasMany(subdomain => subdomain.Emails)
            .WithOne(email => email.Subdomain)
            .HasForeignKey(email => email.SubdomainId);

        builder.HasMany(subdomain => subdomain.SubdomainAccessPolicies)
            .WithOne(subdomainAccessPolicy => subdomainAccessPolicy.Subdomain)
            .HasForeignKey(subdomainAccessPolicy => subdomainAccessPolicy.SubdomainId);

        builder.HasMany(subdomain => subdomain.ChaosMonkeyAddresses)
            .WithOne(chaosMonkeyAddress => chaosMonkeyAddress.Subdomain)
            .HasForeignKey(chaosMonkeyAddress => chaosMonkeyAddress.SubdomainId);
    }
}