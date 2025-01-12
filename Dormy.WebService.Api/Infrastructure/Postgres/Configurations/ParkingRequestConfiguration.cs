using Dormy.WebService.Api.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Dormy.WebService.Api.Infrastructure.Postgres.Configurations
{
    public class ParkingRequestConfiguration : IEntityTypeConfiguration<ParkingRequestEntity>
    {
        public void Configure(EntityTypeBuilder<ParkingRequestEntity> builder)
        {
            builder
                .Property(parkingRequest => parkingRequest.Id)
                .ValueGeneratedOnAdd();

            builder
                .HasOne(parkingRequest => parkingRequest.User)
                .WithMany(user => user.ParkingRequests)
                .HasForeignKey(parkingRequest => parkingRequest.UserId);

            builder
                .HasOne(parkingRequest => parkingRequest.Approver)
                .WithMany(admin => admin.ParkingRequests)
                .HasForeignKey(parkingRequest => parkingRequest.ApproverId);

            builder
                .HasOne(parkingRequest => parkingRequest.Vehicle)
                .WithMany(vehicle => vehicle.ParkingRequests)
                .HasForeignKey(parkingRequest => parkingRequest.VehicleId);

            builder
                .HasOne(parkingRequest => parkingRequest.ParkingSpot)
                .WithMany(parkingSpot => parkingSpot.ParkingRequests)
                .HasForeignKey(parkingRequest => parkingRequest.ParkingSpotId);
        }
    }
}
