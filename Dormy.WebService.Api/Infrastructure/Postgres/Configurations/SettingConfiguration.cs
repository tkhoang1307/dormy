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
                .HasOne(setting => setting.Admin)
                .WithMany(admin => admin.Settings)
                .HasForeignKey(setting => setting.AdminId);


        }
    }
}
