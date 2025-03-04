using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Models.Enums;
using Dormy.WebService.Api.Models.RequestModels;

namespace Dormy.WebService.Api.Presentation.Mappers
{
    public class InvoiceUserMapper
    {
        public InvoiceUserEntity MapToInvoiceUserEntity(InvoiceUserMapperModel model)
        {
            var invoiceUserEntity = new InvoiceUserEntity
            {
                InvoiceId = model.InvoiceId,
                UserId = model.UserId,
                CreatedDateUtc = DateTime.UtcNow,
                LastUpdatedDateUtc = DateTime.UtcNow,
            };

            return invoiceUserEntity;
        }
    }
}
