using Dormy.WebService.Api.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Dormy.WebService.Api.Infrastructure.Postgres.Configurations
{
    public class RoomTypeServiceConfiguration : IEntityTypeConfiguration<RoomTypeServiceEntity>
    {
        public void Configure(EntityTypeBuilder<RoomTypeServiceEntity> builder)
        {
            builder
                .Property(roomTypeService => roomTypeService.Id)
                .ValueGeneratedOnAdd();

            builder
                .HasOne(roomTypeService => roomTypeService.RoomType)
                .WithMany(roomType => roomType.RoomTypeServices)
                .HasForeignKey(roomTypeService => roomTypeService.RoomTypeId);

            builder
                .HasOne(roomTypeService => roomTypeService.RoomService)
                .WithMany(roomService => roomService.RoomTypeServices)
                .HasForeignKey(roomTypeService => roomTypeService.RoomServiceId);
        }
    }
}
