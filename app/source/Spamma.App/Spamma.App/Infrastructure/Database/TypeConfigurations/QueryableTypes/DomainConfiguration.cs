using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Spamma.App.Infrastructure.Querying.Entities;

namespace Spamma.App.Infrastructure.Database.TypeConfigurations.QueryableTypes;

public class DomainConfiguration : IEntityTypeConfiguration<DomainQueryEntity>
{
    public void Configure(EntityTypeBuilder<DomainQueryEntity> builder)
    {
        builder.HasKey(e => e.Id);
        builder.ToView("vw_domain");
    }
}