using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Spamma.App.Infrastructure.Querying.Entities;

namespace Spamma.App.Infrastructure.Database.TypeConfigurations.QueryableTypes;

internal class ChaosMonkeyAddressConfiguration : IEntityTypeConfiguration<ChaosMonkeyAddressQueryEntity>
{
    public void Configure(EntityTypeBuilder<ChaosMonkeyAddressQueryEntity> builder)
    {
        builder.HasKey(chaosMonkeyAddress => chaosMonkeyAddress.Id);
        builder.ToView("vw_chaos_monkey_address");
    }
}