using Dormy.WebService.Api.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Dormy.WebService.Api.Infrastructure.Postgres.Configurations
{
    public class RequestConfiguration : IEntityTypeConfiguration<RequestEntity>
    {
        public void Configure(EntityTypeBuilder<RequestEntity> builder)
        {
            builder
                .Property(request => request.Id)
                .ValueGeneratedOnAdd();

            builder
                .Property(request => request.Status)
                .HasConversion<string>();

            builder
                .HasOne(request => request.Approver)
                .WithMany(admin => admin.Requests)
                .HasForeignKey(request => request.ApproverId);

            builder
                .HasOne(request => request.User)
                .WithMany(user => user.Requests)
                .HasForeignKey(request => request.UserId);

            builder
                .HasOne(request => request.Room)
                .WithMany(room => room.Requests)
                .HasForeignKey(request => request.RoomId);

            builder
                .HasOne(request => request.Contract)
                .WithOne(contract => contract.Request)
                .HasForeignKey<RequestEntity>(request => request.ContractId);
        }
    }
}
