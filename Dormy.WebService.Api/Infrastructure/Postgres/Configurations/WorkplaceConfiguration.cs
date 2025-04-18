using Dormy.WebService.Api.Core.Constants;
using Dormy.WebService.Api.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dormy.WebService.Api.Infrastructure.Postgres.Configurations
{
    public class WorkplaceConfiguration : IEntityTypeConfiguration<WorkplaceEntity>
    {
        public void Configure(EntityTypeBuilder<WorkplaceEntity> builder)
        {
            builder
                .Property(workplace => workplace.Id)
                .ValueGeneratedOnAdd();

            builder
                .HasMany(workplace => workplace.Users)
                .WithOne(user => user.Workplace)
                .HasForeignKey(user => user.WorkplaceId);

            builder.HasData(new WorkplaceEntity()
            {
                Id = SeedData.WorkplaceId,
                Abbrevation = "20000",
                Address = "VNG Q1 HCM City",
                CreatedBy = SeedData.AdminId,
                CreatedDateUtc = DateTime.UtcNow,
                Name = "VNG Block 1 HCM",
            });
        }
    }
}
