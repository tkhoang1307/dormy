using Dormy.WebService.Api.Core.Constants;
using Dormy.WebService.Api.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dormy.WebService.Api.Infrastructure.Postgres.Configurations
{
    public class RoomServiceConfiguration : IEntityTypeConfiguration<RoomServiceEntity>
    {
        public void Configure(EntityTypeBuilder<RoomServiceEntity> builder)
        {
            builder
                .Property(roomService => roomService.Id)
                .ValueGeneratedOnAdd();

            builder
                .Property(roomService => roomService.RoomServiceType)
                .HasConversion<string>();

            builder.HasData(new RoomServiceEntity()
            {
                Id = SeedData.RoomServiceId,
                Cost = 3000,
                CreatedBy = SeedData.AdminId,
                IsServiceIndicatorUsed = true,
                RoomServiceName = "Water",
                RoomServiceType = Models.Enums.RoomServiceTypeEnum.WATER,
                Unit = "m3"
            });
        }
    }
}
