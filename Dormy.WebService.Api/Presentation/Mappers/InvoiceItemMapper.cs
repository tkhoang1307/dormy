using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Models.Enums;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;

namespace Dormy.WebService.Api.Presentation.Mappers
{
    public class InvoiceItemMapper
    {
        public InvoiceItemEntity MapToInvoiceItemEntity(InvoiceItemMapperRequestModel model)
        {
            var invoiceItemEntity = new InvoiceItemEntity
            {
                RoomServiceId = model.RoomServiceId,
                RoomServiceName = model.RoomServiceName,
                Cost = model.Cost,
                Quantity = model.Quantity,
                Unit = model.Unit,
                Metadata = model.Metadata,
                CreatedDateUtc = DateTime.UtcNow,
                LastUpdatedDateUtc = DateTime.UtcNow,
            };

            return invoiceItemEntity;
        }

        public InvoiceItemResponseModel MapToInvoiceItemResponseModel(InvoiceItemEntity model)
        {
            var invoiceItemResponseModel = new InvoiceItemResponseModel
            {
                Id = model.Id,
                RoomServiceId = model.RoomServiceId,
                RoomServiceName = model.RoomServiceName,
                Cost = model.Cost,
                Quantity = model.Quantity,
                Unit = model.Unit,
                Metadata = model.Metadata,
            };

            return invoiceItemResponseModel;
        }
    }
}
