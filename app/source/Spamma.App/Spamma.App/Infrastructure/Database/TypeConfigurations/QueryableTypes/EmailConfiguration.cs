using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Spamma.App.Infrastructure.Querying.Entities;

namespace Spamma.App.Infrastructure.Database.TypeConfigurations.QueryableTypes;

internal class EmailConfiguration : IEntityTypeConfiguration<EmailQueryEntity>
{
    public void Configure(EntityTypeBuilder<EmailQueryEntity> builder)
    {
        builder.HasKey(email => email.Id);

        builder.Property(email => email.EmailAddress);
        builder.Property(email => email.Subject);
        builder.Property(email => email.WhenSent);

        builder.ToView("vw_email");
    }
}