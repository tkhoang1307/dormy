using Dormy.WebService.Api.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Dormy.WebService.Api.Infrastructure.Postgres.Configurations
{
    public class InvoiceConfiguration : IEntityTypeConfiguration<InvoiceEntity>
    {
        public void Configure(EntityTypeBuilder<InvoiceEntity> builder)
        {
            builder
                .Property(invoice => invoice.Id)
                .ValueGeneratedOnAdd();

            builder
                .HasOne(invoice => invoice.Room)
                .WithMany(room => room.Invoices)
                .HasForeignKey(invoice => invoice.RoomId);
        }
    }
}
