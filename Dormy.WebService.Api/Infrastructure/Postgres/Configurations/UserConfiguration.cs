using Dormy.WebService.Api.Core.Constants;
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

            builder
                .Property(user => user.Status)
                .HasConversion<string>();

            builder
                .Property(user => user.Gender)
                .HasConversion<string>();

            builder.HasOne(user => user.HealthInsurance)
                .WithOne(healthInsurance => healthInsurance.User)
                .HasForeignKey<UserEntity>(user => user.HealthInsuranceId);

            builder.HasData(new UserEntity()
            {
                PhoneNumber = "0123456789",
                DateOfBirth = new DateTime(2000, 1, 1),
                Email = "user@gmail.com",
                FirstName = "Van",
                LastName = "Ba",
                Gender = Models.Enums.GenderEnum.MALE,
                Password = "8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918",
                UserName = "user",
                WorkplaceId  = SeedData.WorkplaceId,
                NationalIdNumber = "30239840329",
                Status = Models.Enums.UserStatusEnum.ACTIVE,
                HealthInsuranceId = SeedData.HealthInsuranceId,
                Id = SeedData.UserId,
            });
        }
    }
}
