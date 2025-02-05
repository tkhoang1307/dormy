using Dormy.WebService.Api.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dormy.WebService.Api.Infrastructure.Postgres.Configurations
{
    public class BuildingConfiguration : IEntityTypeConfiguration<BuildingEntity>
    {
        public void Configure(EntityTypeBuilder<BuildingEntity> builder)
        {
            builder
                .Property(building => building.Id)
                .ValueGeneratedOnAdd();

            builder
                .Property(building => building.GenderRestriction)
                .HasConversion<string>();

        }
    }
}
