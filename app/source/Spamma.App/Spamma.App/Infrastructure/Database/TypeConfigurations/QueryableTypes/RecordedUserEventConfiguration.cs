using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Spamma.App.Infrastructure.Querying.Entities;

namespace Spamma.App.Infrastructure.Database.TypeConfigurations.QueryableTypes;

internal class RecordedUserEventConfiguration : IEntityTypeConfiguration<RecordedUserEventQueryEntity>
{
    public void Configure(EntityTypeBuilder<RecordedUserEventQueryEntity> builder)
    {
        builder.HasKey(recordedUserEvent => recordedUserEvent.Id);

        builder.Property(recordedUserEvent => recordedUserEvent.ActionType);
        builder.Property(recordedUserEvent => recordedUserEvent.WhenHappened);

        builder.ToView("vw_recorded_user_event");
    }
}