using Dormy.WebService.Api.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dormy.WebService.Api.Core.Configurations
{
    public class GuardianConfiguration : IEntityTypeConfiguration<GuardianEntity>
    {
        public void Configure(EntityTypeBuilder<GuardianEntity> builder)
        {
            builder
                .Property(guardian => guardian.Id)
                .ValueGeneratedOnAdd();
        }
    }
}
