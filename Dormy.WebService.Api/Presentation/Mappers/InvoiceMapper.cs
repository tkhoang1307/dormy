using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Models.Enums;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
                ContractId = model.ContractId,
                InvoiceItems = model.InvoiceItems.Select(it => _invoiceItemMapper.MapToInvoiceItemEntity(it)).ToList(),
                InvoiceUsers = model.InvoiceUsers.Select(iu => _invoiceUserMapper.MapToInvoiceUserEntity(iu)).ToList(),
                CreatedDateUtc = DateTime.UtcNow,
                LastUpdatedDateUtc = DateTime.UtcNow,
            };

            return invoiceEntity;
        }

        public InvoiceBatchResponseModel MapToInvoiceBatchResponseModel(InvoiceEntity entity, bool isUserDisplayed = false)
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
                //ContractId = entity.ContractId,
                RoomId = entity.RoomId,
                UserFullname = isUserDisplayed ? entity.InvoiceUsers.FirstOrDefault().User.LastName + " " + entity.InvoiceUsers.FirstOrDefault().User.FirstName : "",
                CreatedBy = entity.CreatedBy,
                CreatedDateUtc = entity.CreatedDateUtc,
                LastUpdatedDateUtc = entity.LastUpdatedDateUtc,
                LastUpdatedBy = entity.LastUpdatedBy,
                IsDeleted = entity != null && entity.IsDeleted
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
                RoomName = entity.Room.RoomNumber.ToString(),
                //ContractId = entity.ContractId,
                CreatedBy = entity.CreatedBy,
                CreatedDateUtc = entity.CreatedDateUtc,
                LastUpdatedDateUtc = entity.LastUpdatedDateUtc,
                LastUpdatedBy = entity.LastUpdatedBy,
                InvoiceItems = (entity != null && entity.InvoiceItems != null) ? entity.InvoiceItems.Select(r => _invoiceItemMapper.MapToInvoiceItemResponseModel(r)).ToList() : [],
                InvoiceUsers = (entity != null && entity.InvoiceUsers != null) ? entity.InvoiceUsers.Select(iu => new InvoiceUserResponseModel()
                {
                    UserId = iu.UserId,
                    UserName = iu.User.LastName + " " + iu.User.FirstName,
                }).ToList() : [],
                IsDeleted = entity != null && entity.IsDeleted,
            };

            return invoiceResponse;
        }
    }
}
