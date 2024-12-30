using Dormy.WebService.Api.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dormy.WebService.Api.Core.Configurations
{
    public class BedConfiguration : IEntityTypeConfiguration<BedEntity>
    {
        public void Configure(EntityTypeBuilder<BedEntity> builder)
        {
            builder
                .Property(bed => bed.Id)
                .ValueGeneratedOnAdd();

            builder
                .HasOne(bed => bed.Room)
                .WithMany(room => room.Beds)
                .HasForeignKey(bed => bed.RoomId);
        }
    }
}
