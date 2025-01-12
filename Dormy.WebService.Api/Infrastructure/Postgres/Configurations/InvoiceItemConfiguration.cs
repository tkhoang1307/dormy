using Dormy.WebService.Api.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Dormy.WebService.Api.Infrastructure.Postgres.Configurations
{
    public class InvoiceItemConfiguration : IEntityTypeConfiguration<InvoiceItemEntity>
    {
        public void Configure(EntityTypeBuilder<InvoiceItemEntity> builder)
        {
            builder
                .Property(invoiceItem => invoiceItem.Id)
                .ValueGeneratedOnAdd();

            builder
                .HasOne(invoiceItem => invoiceItem.Invoice)
                .WithMany(invoice => invoice.InvoiceItems)
                .HasForeignKey(invoiceItem => invoiceItem.InvoiceId);
        }
    }
}
