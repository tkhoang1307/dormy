using Dormy.WebService.Api.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dormy.WebService.Api.Core.Configurations
{
    public class WorkplaceConfiguration : IEntityTypeConfiguration<WorkplaceEntity>
    {
        public void Configure(EntityTypeBuilder<WorkplaceEntity> builder)
        {
            builder
                .Property(workplace => workplace.Id)
                .ValueGeneratedOnAdd();
        }
    }
}
