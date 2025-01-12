using Dormy.WebService.Api.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Dormy.WebService.Api.Infrastructure.Postgres.Configurations
{
    public class ContractConfiguration : IEntityTypeConfiguration<ContractEntity>
    {
        public void Configure(EntityTypeBuilder<ContractEntity> builder)
        {
            builder
                .Property(contract => contract.Id)
                .ValueGeneratedOnAdd();

            builder
                .HasOne(contract => contract.User)
                .WithMany(user => user.Contracts)
                .HasForeignKey(contract => contract.UserId);

            builder
                .HasOne(contract => contract.Approver)
                .WithMany(admin => admin.Contracts)
                .HasForeignKey(contract => contract.ApproverId);

            builder
                .HasOne(contract => contract.Bed)
                .WithMany(bed => bed.Contracts)
                .HasForeignKey(contract => contract.BedId);

            builder
                .HasOne(contract => contract.Room)
                .WithMany(room => room.Contracts)
                .HasForeignKey(contract => contract.RoomId);
        }
    }
}
