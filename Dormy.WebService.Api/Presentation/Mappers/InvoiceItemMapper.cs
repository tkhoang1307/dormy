using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Models.Enums;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;
using Newtonsoft.Json.Linq;

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
                Metadata = model.Metadata != null ? model.Metadata.ToString() : string.Empty,
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
                RoomServiceId = model?.RoomServiceId,
                RoomServiceName = model.RoomServiceName,
                Cost = model.Cost,
                Quantity = model.Quantity,
                Unit = model.Unit,
                Metadata = JObject.Parse(model.Metadata),
            };

            return invoiceItemResponseModel;
        }
    }
}
