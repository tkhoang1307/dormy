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

            builder.HasData(new AdminEntity()
            {
                Id = Guid.NewGuid(),
                UserName = "admin",
                Password = "8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918", //admin
                Email = "hungdv190516@gmail.com",
                DateOfBirth = DateTime.Now,
                Gender = Models.Enums.GenderEnum.MALE,
                FirstName = "Admin",
                JobTitle = "Admin",
            });
        }
    }
}
