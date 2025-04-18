using Dormy.WebService.Api.Core.Constants;
using Dormy.WebService.Api.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dormy.WebService.Api.Infrastructure.Postgres.Configurations
{
    public class RoomTypeConfiguration : IEntityTypeConfiguration<RoomTypeEntity>
    {
        public void Configure(EntityTypeBuilder<RoomTypeEntity> builder)
        {
            builder
                .Property(roomType => roomType.Id)
                .ValueGeneratedOnAdd();

            builder.HasData(new RoomTypeEntity()
            {
                Capacity = 5,
                Description = "Room type normal",
                Id = SeedData.RoomTypeId,
                Price = 100000,
                RoomTypeName = "Normal",
            });
        }
    }
}
