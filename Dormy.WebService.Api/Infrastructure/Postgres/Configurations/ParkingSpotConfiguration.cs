using Dormy.WebService.Api.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Dormy.WebService.Api.Infrastructure.Postgres.Configurations
{
    public class ParkingSpotConfiguration : IEntityTypeConfiguration<ParkingSpotEntity>
    {
        public void Configure(EntityTypeBuilder<ParkingSpotEntity> builder)
        {
            builder
                .Property(parkingSpot => parkingSpot.Id)
                .ValueGeneratedOnAdd();

            builder
                .Property(parkingSpot => parkingSpot.Status)
                .HasConversion<string>();
        }
    }
}
