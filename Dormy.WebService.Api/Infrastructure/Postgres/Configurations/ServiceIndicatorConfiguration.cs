using Dormy.WebService.Api.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Dormy.WebService.Api.Infrastructure.Postgres.Configurations
{
    public class ServiceIndicatorConfigurationn : IEntityTypeConfiguration<ServiceIndicatorEntity>
    {
        public void Configure(EntityTypeBuilder<ServiceIndicatorEntity> builder)
        {
            builder
                .Property(serviceIndicator => serviceIndicator.Id)
                .ValueGeneratedOnAdd();

            builder
                .HasOne(serviceIndicator => serviceIndicator.RoomService)
                .WithMany(roomService => roomService.ServiceIndicators)
                .HasForeignKey(serviceIndicator => serviceIndicator.RoomServiceId);

            builder
                .HasOne(serviceIndicator => serviceIndicator.Room)
                .WithMany(room => room.ServiceIndicators)
                .HasForeignKey(serviceIndicator => serviceIndicator.RoomId);
        }
    }
}
