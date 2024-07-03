using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Spamma.App.Infrastructure.Database.TypeConfigurations.MutableTypes
{
    public class DomainConfiguration : IEntityTypeConfiguration<Domain.DomainAggregate.Aggregate.Domain>
    {
        public void Configure(EntityTypeBuilder<Domain.DomainAggregate.Aggregate.Domain> builder)
        {
            builder.HasKey(e => e.Id);
            builder.ToTable("Domain");
            builder.HasKey(e => e.Id);
            builder.Ignore(e => e.DomainEvents);
            builder.Property(e => e.Id).ValueGeneratedNever();
        }
    }
}