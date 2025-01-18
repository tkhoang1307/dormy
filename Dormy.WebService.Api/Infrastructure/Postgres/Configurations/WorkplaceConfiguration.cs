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
        }
    }
}
