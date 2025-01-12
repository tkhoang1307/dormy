using Dormy.WebService.Api.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Dormy.WebService.Api.Infrastructure.Postgres.Configurations
{
    public class VehicleConfiguration : IEntityTypeConfiguration<VehicleEntity>
    {
        public void Configure(EntityTypeBuilder<VehicleEntity> builder)
        {
            builder
                .Property(vehicle => vehicle.Id)
                .ValueGeneratedOnAdd();

            builder
                .HasOne(vehicle => vehicle.User)
                .WithMany(user => user.Vehicles)
                .HasForeignKey(vehicle => vehicle.UserId);
        }
    }
}
