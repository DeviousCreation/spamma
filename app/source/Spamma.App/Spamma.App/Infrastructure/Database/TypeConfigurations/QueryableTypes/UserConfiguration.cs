using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Spamma.App.Infrastructure.Querying.Entities;

namespace Spamma.App.Infrastructure.Database.TypeConfigurations.QueryableTypes;

internal class UserConfiguration : IEntityTypeConfiguration<UserQueryEntity>
{
    public void Configure(EntityTypeBuilder<UserQueryEntity> builder)
    {
        builder.HasKey(user => user.Id);
        builder.ToView("vw_user");

        builder.Property(user => user.Name);
        builder.Property(user => user.EmailAddress);
        builder.Property(user => user.WhenDisabled);
        builder.Property(user => user.WhenCreated);
        builder.Property(user => user.WhenVerified);
        builder.Property(user => user.LastLoggedIn);
        builder.Property(user => user.DomainCount);
        builder.Property(user => user.SubdomainCount);

        builder.HasMany(x => x.RecordedUserEventQueryEntities)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId);
    }
}