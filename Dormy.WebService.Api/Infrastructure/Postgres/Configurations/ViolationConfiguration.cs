using Dormy.WebService.Api.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dormy.WebService.Api.Infrastructure.Postgres.Configurations
{
    public class ViolationConfiguration : IEntityTypeConfiguration<ViolationEntity>
    {
        public void Configure(EntityTypeBuilder<ViolationEntity> builder)
        {
            builder
                .Property(violation => violation.Id)
                .ValueGeneratedOnAdd();

            builder
                .HasOne(violation => violation.User)
                .WithMany(user => user.Violations)
                .HasForeignKey(violation => violation.UserId);
        }
    }
}
