using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Spamma.App.Infrastructure.Domain.UserAggregate.Aggregate;

namespace Spamma.App.Infrastructure.Database.TypeConfigurations.MutableTypes;

internal class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(user => user.Id);
        builder.ToTable("user");
        builder.Ignore(user => user.DomainEvents);
        builder.Property(user => user.Id)
            .ValueGeneratedNever();

        builder.Property(user => user.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(user => user.EmailAddress)
            .IsRequired()
            .HasMaxLength(320);

        builder.Property(user => user.WhenCreated)
            .IsRequired();

        builder.Property(user => user.SecurityStamp)
            .IsRequired();

        builder.Ignore(user => user.IsVerified);
        builder.Ignore(user => user.WhenVerified);

        builder.Ignore(user => user.WhenDisabled);
        builder.Ignore(user => user.IsDisabled);
        builder.Property("_whenDisabled")
            .HasColumnName("when_disabled");

        builder.OwnsMany(users => users.RecordedUserEvents, recordedUserEvents =>
        {
            recordedUserEvents.HasKey(recordedUserEvent => recordedUserEvent.Id);
            recordedUserEvents.ToTable("recorded_user_event");
            recordedUserEvents.Ignore(recordedUserEvent => recordedUserEvent.DomainEvents);
            recordedUserEvents.Property(recordedUserEvent => recordedUserEvent.Id).ValueGeneratedNever();
            recordedUserEvents.Property(recordedUserEvent => recordedUserEvent.ActionType)
                .IsRequired();
            recordedUserEvents.Property(recordedUserEvent => recordedUserEvent.WhenHappened)
                .IsRequired();
        }).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}