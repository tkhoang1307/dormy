using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Models.ResponseModels;

namespace Dormy.WebService.Api.Presentation.Mappers
{
    public class ContractMapper
    {
        private readonly RoomMapper _roomMapper;
        public ContractMapper()
        {
            _roomMapper = new();
        }

        public ContractResponseModel MapToContractModel(ContractEntity contractEntity)
        {
            return new ContractResponseModel
            {
                Id = contractEntity.Id,
                SubmissionDate = contractEntity.SubmissionDate,
                StartDate = contractEntity.StartDate,
                EndDate = contractEntity.EndDate,
                Status = contractEntity.Status,
                NumberExtension = contractEntity.NumberExtension,
                ApproverId = contractEntity.ApproverId,
                ApproverName = contractEntity.Approver == null ? string.Empty : $"{contractEntity.Approver?.FirstName} {contractEntity.Approver?.LastName}",
                RoomId = contractEntity.RoomId,
                Room = contractEntity.Room == null ? null : _roomMapper.MapToRoomResponseModel(contractEntity.Room),
            };
        }
    }
}
