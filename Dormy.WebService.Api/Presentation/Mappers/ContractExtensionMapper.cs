using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Models.Enums;
using Dormy.WebService.Api.Models.RequestModels;

namespace Dormy.WebService.Api.Presentation.Mappers
{
    public class ContractExtensionMapper
    {
        public ContractExtensionEntity MapToContractExtensionEntity(ContractExtensionRequestModel model)
        {
            return new ContractExtensionEntity
            {
                SubmissionDate = DateTime.Today,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                Status = ContractExtensionStatusEnum.PENDING,
                ContractId = model.ContractId,
                CreatedDateUtc = DateTime.UtcNow,
                LastUpdatedDateUtc = DateTime.UtcNow,
            };
        }
    }
}
