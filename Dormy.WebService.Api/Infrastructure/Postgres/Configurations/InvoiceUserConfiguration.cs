using Dormy.WebService.Api.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Dormy.WebService.Api.Infrastructure.Postgres.Configurations
{
    public class InvoiceUserConfiguration : IEntityTypeConfiguration<InvoiceUserEntity>
    {
        public void Configure(EntityTypeBuilder<InvoiceUserEntity> builder)
        {
            builder
                .Property(invoiceUser => invoiceUser.Id)
                .ValueGeneratedOnAdd();

            builder
                .HasOne(invoiceUser => invoiceUser.Invoice)
                .WithMany(invoice => invoice.InvoiceUsers)
                .HasForeignKey(invoiceUser => invoiceUser.InvoiceId);

            builder
                .HasOne(invoiceUser => invoiceUser.User)
                .WithMany(user => user.InvoiceUsers)
                .HasForeignKey(invoiceUser => invoiceUser.UserId);
        }
    }
}
