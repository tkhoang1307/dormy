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
                Id = Guid.NewGuid(),
                SubmissionDate = DateTime.Today,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                Status = ContractExtensionStatusEnum.PENDING,
                CreatedDateUtc = DateTime.UtcNow,
                LastUpdatedDateUtc = DateTime.UtcNow,
            };
        }
    }
}
