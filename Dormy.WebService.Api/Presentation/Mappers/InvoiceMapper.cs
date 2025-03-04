using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Models.Enums;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;
using Microsoft.VisualBasic;

namespace Dormy.WebService.Api.Presentation.Mappers
{
    public class InvoiceMapper
    {
        private readonly InvoiceItemMapper _invoiceItemMapper;
        private readonly InvoiceUserMapper _invoiceUserMapper;

        public InvoiceMapper()
        {
            _invoiceItemMapper = new InvoiceItemMapper();
            _invoiceUserMapper = new InvoiceUserMapper();
        }

        public InvoiceEntity MapToInvoiceEntity(InvoiceMapperRequestModel model)
        {
            var invoiceEntity = new InvoiceEntity
            {
                InvoiceName = model.InvoiceName,
                DueDate = model.DueDate,
                AmountBeforePromotion = model.AmountBeforePromotion,
                AmountAfterPromotion = model.AmountAfterPromotion,
                Month = model.Month,
                Year = model.Year,
                Type = (InvoiceTypeEnum)Enum.Parse(typeof(InvoiceTypeEnum), model.Type),
                Status = (InvoiceStatusEnum)Enum.Parse(typeof(InvoiceStatusEnum), model.Status),
                RoomId = model.RoomId,
                InvoiceItems = model.InvoiceItems.Select(it => _invoiceItemMapper.MapToInvoiceItemEntity(it)).ToList(),
                InvoiceUsers = model.InvoiceUsers.Select(iu => _invoiceUserMapper.MapToInvoiceUserEntity(iu)).ToList(),
                CreatedDateUtc = DateTime.UtcNow,
                LastUpdatedDateUtc = DateTime.UtcNow,
            };

            return invoiceEntity;
        }

        public InvoiceBatchResponseModel MapToInvoiceBatchResponseModel(InvoiceEntity entity)
        {
            var invoiceResponse = new InvoiceBatchResponseModel
            {
                Id = entity.Id,
                InvoiceName = entity.InvoiceName,
                DueDate = entity.DueDate,
                AmountBeforePromotion = entity.AmountBeforePromotion,
                AmountAfterPromotion = entity.AmountAfterPromotion,
                Month = entity.Month, 
                Year = entity.Year,
                Type = entity.Type.ToString(),
                Status = entity.Status.ToString(),
                RoomId = entity.RoomId,
                CreatedBy = entity.CreatedBy,
                CreatedDateUtc = entity.CreatedDateUtc,
                LastUpdatedDateUtc = entity.LastUpdatedDateUtc,
                LastUpdatedBy = entity.LastUpdatedBy,
                IsDeleted = entity != null && entity.IsDeleted,
            };

            return invoiceResponse;
        }

        public InvoiceResponseModel MapToInvoiceResponseModel(InvoiceEntity entity)
        {
            var invoiceResponse = new InvoiceResponseModel
            {
                Id = entity.Id,
                InvoiceName = entity.InvoiceName,
                DueDate = entity.DueDate,
                AmountBeforePromotion = entity.AmountBeforePromotion,
                AmountAfterPromotion = entity.AmountAfterPromotion,
                Month = entity.Month,
                Year = entity.Year,
                Type = entity.Type.ToString(),
                Status = entity.Status.ToString(),
                RoomId = entity.RoomId,
                CreatedBy = entity.CreatedBy,
                CreatedDateUtc = entity.CreatedDateUtc,
                LastUpdatedDateUtc = entity.LastUpdatedDateUtc,
                LastUpdatedBy = entity.LastUpdatedBy,
                //Rooms = (entity != null && entity.Rooms != null) ? entity.Rooms.Select(r => _roomMapper.MapToRoomResponseModel(r)).OrderBy(r => r.FloorNumber).ToList() : [],
                InvoiceItems = (entity != null && entity.InvoiceItems != null) ? entity.InvoiceItems.Select(r => _invoiceItemMapper.MapToInvoiceItemResponseModel(r)).ToList() : [],
                IsDeleted = entity != null && entity.IsDeleted,
            };

            return invoiceResponse;
        }
    }
}
