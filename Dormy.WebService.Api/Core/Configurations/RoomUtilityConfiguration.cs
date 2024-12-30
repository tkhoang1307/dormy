using Dormy.WebService.Api.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dormy.WebService.Api.Core.Configurations
{
    public class RoomUtilityConfiguration : IEntityTypeConfiguration<RoomUtilityEntity>
    {
        public void Configure(EntityTypeBuilder<RoomUtilityEntity> builder)
        {
            builder
                .Property(roomUtility => roomUtility.Id)
                .ValueGeneratedOnAdd();
        }
    }
}
