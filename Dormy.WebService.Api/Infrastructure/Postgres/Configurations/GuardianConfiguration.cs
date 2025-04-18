using Dormy.WebService.Api.Core.Constants;
using Dormy.WebService.Api.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dormy.WebService.Api.Infrastructure.Postgres.Configurations
{
    public class GuardianConfiguration : IEntityTypeConfiguration<GuardianEntity>
    {
        public void Configure(EntityTypeBuilder<GuardianEntity> builder)
        {
            builder
                .Property(guardian => guardian.Id)
                .ValueGeneratedOnAdd();

            builder
                .HasOne(guardian => guardian.User)
                .WithMany(user => user.Guardians)
                .HasForeignKey(guardian => guardian.UserId);

            builder
                .HasData(new GuardianEntity()
                {
                    Id = SeedData.GuardianId,
                    Name = "Bac Ba",
                    Address = "Kien Giang",
                    Email = "bacba@gmail.com",
                    CreatedBy = SeedData.AdminId,
                    CreatedDateUtc = DateTime.UtcNow,
                    PhoneNumber = "09737338939",
                    UserId = SeedData.UserId,
                    RelationshipToUser = "Cha",
                });
        }
    }
}
