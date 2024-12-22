using Dormy.WebService.Api.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Dormy.WebService.Api.Infrastructure.Postgres.Configurations
{
    public class AdminConfiguration : IEntityTypeConfiguration<AdminEntity>
    {
        public void Configure(EntityTypeBuilder<AdminEntity> builder)
        {
            builder
               .Property(e => e.Id)
               .ValueGeneratedOnAdd();
        }
    }
}
