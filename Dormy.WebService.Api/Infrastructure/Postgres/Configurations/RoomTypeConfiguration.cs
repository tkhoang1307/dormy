using Dormy.WebService.Api.Core.Constants;
using Dormy.WebService.Api.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dormy.WebService.Api.Infrastructure.Postgres.Configurations
{
    public class RoomTypeConfiguration : IEntityTypeConfiguration<RoomTypeEntity>
    {
        public void Configure(EntityTypeBuilder<RoomTypeEntity> builder)
        {
            builder
                .Property(roomType => roomType.Id)
                .ValueGeneratedOnAdd();
        }
    }
}
