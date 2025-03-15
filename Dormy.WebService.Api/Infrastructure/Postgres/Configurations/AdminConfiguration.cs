using Dormy.WebService.Api.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dormy.WebService.Api.Infrastructure.Postgres.Configurations
{
    public class AdminConfiguration : IEntityTypeConfiguration<AdminEntity>
    {
        public void Configure(EntityTypeBuilder<AdminEntity> builder)
        {
            builder
                .Property(admin => admin.Id)
                .ValueGeneratedOnAdd();

            builder
                .Property(admin => admin.Gender)
                .HasConversion<string>();
        }
    }
}
