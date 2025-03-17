using Dormy.WebService.Api.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dormy.WebService.Api.Infrastructure.Postgres.Configurations
{
    public class SettingConfiguration : IEntityTypeConfiguration<SettingEntity>
    {
        public void Configure(EntityTypeBuilder<SettingEntity> builder)
        {
            builder
                .Property(setting => setting.Id)
                .ValueGeneratedOnAdd();

            builder
                .Property(setting => setting.DataType)
                .HasConversion<string>();
        }
    }
}
