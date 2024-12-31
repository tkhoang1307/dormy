using Dormy.WebService.Api.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dormy.WebService.Api.Infrastructure.Postgres.Configurations
{
    public class RoomConfiguration : IEntityTypeConfiguration<RoomEntity>
    {
        public void Configure(EntityTypeBuilder<RoomEntity> builder)
        {
            builder
                .Property(building => building.Id)
                .ValueGeneratedOnAdd();

            builder
                .HasOne(room => room.Building)
                .WithMany(building => building.Rooms)
                .HasForeignKey(room => room.Id);

            builder
                .HasOne(room => room.RoomType)
                .WithMany(roomType => roomType.Rooms)
                .HasForeignKey(room => room.RoomTypeId);
        }
    }
}
