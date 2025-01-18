using Dormy.WebService.Api.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dormy.WebService.Api.Infrastructure.Postgres.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder
                .Property(user => user.Id)
                .ValueGeneratedOnAdd();

            builder.HasOne(user => user.Guardian)
                .WithOne(guardian => guardian.User)
                .HasForeignKey<UserEntity>(user => user.GuardianId);

            builder.HasOne(user => user.HealthInsurance)
                .WithOne(healthInsurance => healthInsurance.User)
                .HasForeignKey<UserEntity>(user => user.HealthInsuranceId);
        }
    }
}
