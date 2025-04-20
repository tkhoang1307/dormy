using Dormy.WebService.Api.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dormy.WebService.Api.Infrastructure.Postgres.Configurations
{
    public class NotificationConfiguration : IEntityTypeConfiguration<NotificationEntity>
    {
        public void Configure(EntityTypeBuilder<NotificationEntity> builder)
        {
            builder
                .Property(notification => notification.Id)
                .ValueGeneratedOnAdd();

            builder
                .Property(roomService => roomService.NotificationType)
                .HasConversion<string>();

            builder
                .HasOne(notification => notification.User)
                .WithMany(user => user.Notifications)
                .HasForeignKey(notification => notification.UserId);

            builder
                .HasOne(notification => notification.Admin)
                .WithMany(admin => admin.Notifications)
                .HasForeignKey(notification => notification.AdminId);
        }
    }
}
