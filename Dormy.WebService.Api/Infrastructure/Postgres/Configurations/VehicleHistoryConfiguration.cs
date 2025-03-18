using Dormy.WebService.Api.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Dormy.WebService.Api.Infrastructure.Postgres.Configurations
{
    public class VehicleHistoryConfiguration : IEntityTypeConfiguration<VehicleHistoryEntity>
    {
        public void Configure(EntityTypeBuilder<VehicleHistoryEntity> builder)
        {
            builder
                .Property(vehicleHistory => vehicleHistory.Id)
                .ValueGeneratedOnAdd();

            builder
                .Property(vehicleHistory => vehicleHistory.Action)
                .HasConversion<string>();

            builder
                .HasOne(vehicleHistory => vehicleHistory.Vehicle)
                .WithMany(vehicle => vehicle.VehicleHistories)
                .HasForeignKey(vehicleHistory => vehicleHistory.VehicleId);

            builder
                .HasOne(vehicleHistory => vehicleHistory.ParkingSpot)
                .WithMany(parkingSpot => parkingSpot.VehicleHistories)
                .HasForeignKey(vehicleHistory => vehicleHistory.ParkingSpotId);
        }
    }
}
