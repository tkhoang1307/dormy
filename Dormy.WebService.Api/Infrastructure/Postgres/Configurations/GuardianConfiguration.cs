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
        }
    }
}
