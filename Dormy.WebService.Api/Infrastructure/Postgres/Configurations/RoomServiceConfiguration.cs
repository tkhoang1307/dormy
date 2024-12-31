using Dormy.WebService.Api.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dormy.WebService.Api.Infrastructure.Postgres.Configurations
{
    public class RoomServiceConfiguration : IEntityTypeConfiguration<RoomServiceEntity>
    {
        public void Configure(EntityTypeBuilder<RoomServiceEntity> builder)
        {
            builder
                .Property(roomService => roomService.Id)
                .ValueGeneratedOnAdd();
        }
    }
}
