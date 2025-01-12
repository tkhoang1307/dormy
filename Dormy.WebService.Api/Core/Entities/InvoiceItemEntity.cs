﻿namespace Dormy.WebService.Api.Core.Entities
{
    public class InvoiceItemEntity : BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string ServiceName { get; set; } = string.Empty;

        public decimal Cost { get; set; }

        public decimal Quantity { get; set; }

        public string Unit { get; set; } = string.Empty;

        public object Metadata { get; set; } = null!;

        public Guid InvoiceId { get; set; }

        public InvoiceEntity Invoice { get; set; } = null!;
    }
}