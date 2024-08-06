using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Spamma.App.Infrastructure.Domain.EmailAggregate.Aggregate;

namespace Spamma.App.Infrastructure.Database.TypeConfigurations.MutableTypes;

internal class EmailConfiguration : IEntityTypeConfiguration<Email>
{
    public void Configure(EntityTypeBuilder<Email> builder)
    {
        builder.HasKey(email => email.Id);
        builder.ToTable("email");
        builder.Ignore(email => email.DomainEvents);
        builder.Property(email => email.Id)
            .ValueGeneratedNever()
            .IsRequired();

        builder.Property(email => email.EmailAddress)
            .IsRequired()
            .HasMaxLength(320);

        builder.Property(email => email.Subject)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(email => email.SubdomainId)
            .IsRequired();

        builder.Property(email => email.WhenSent)
            .IsRequired();
    }
}