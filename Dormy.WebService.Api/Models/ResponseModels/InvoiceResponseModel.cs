﻿using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Models.Enums;

namespace Dormy.WebService.Api.Models.ResponseModels
{
    public class InvoiceBatchResponseModel : BaseResponseModel
    {
        public Guid Id { get; set; }

        public string InvoiceName { get; set; } = string.Empty;

        public DateTime DueDate { get; set; }

        public decimal AmountBeforePromotion { get; set; }

        public decimal AmountAfterPromotion { get; set; }

        public int? Month { get; set; }

        public int? Year { get; set; }

        public string Type { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public Guid RoomId { get; set; }

        public string RoomName { get; set; } = string.Empty;
    }

    public class InvoiceResponseModel : InvoiceBatchResponseModel
    {
        public List<InvoiceItemResponseModel>? InvoiceItems { get; set; }

        public List<InvoiceUserResponseModel>? InvoiceUsers { get; set; }
    }

    public class InvoiceUserResponseModel
    {
        public Guid UserId { get; set; }

        public string UserName { get; set; } = string.Empty;
    }
}
