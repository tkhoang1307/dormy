using Dormy.WebService.Api.Core.Constants;
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

            builder
                .HasData(new AdminEntity()
                {
                    DateOfBirth = new DateTime(2000, 1, 1),
                    Email = "admin@gmail.com",
                    FirstName = "Le",
                    LastName = "Long",
                    UserName = "admin",
                    Password = "8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918",
                    JobTitle = "Admin",
                    Gender = Models.Enums.GenderEnum.MALE,
                    PhoneNumber = "0895940404",
                    Id = SeedData.AdminId,
                });
        }
    }
}
