using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Models.Enums;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;

namespace Dormy.WebService.Api.Presentation.Mappers
{
    public class ContractMapper
    {
        private readonly RoomMapper _roomMapper;
        private readonly UserMapper _userMapper;
        private readonly HealthInsuranceMapper _healthInsuranceMapper;
        private readonly WorkplaceMapper _workplaceMapper;
        private readonly VehicleMapper _vehicleMapper;
        private readonly GuardianMapper _guardianMapper;

        public ContractMapper()
        {
            _roomMapper = new();
            _userMapper = new();
            _healthInsuranceMapper = new();
            _workplaceMapper = new();
            _vehicleMapper = new();
            _guardianMapper = new();
        }

        public ContractResponseModel MapToContractModel(ContractEntity contractEntity)
        {
            return new ContractResponseModel
            {
                Id = contractEntity.Id,
                SubmissionDate = contractEntity.SubmissionDate,
                StartDate = contractEntity.StartDate,
                EndDate = contractEntity.EndDate,
                Status = contractEntity.Status.ToString(),
                NumberExtension = contractEntity.NumberExtension,
                ApproverId = contractEntity.ApproverId,
                ApproverName = contractEntity.Approver == null ? string.Empty : $"{contractEntity.Approver?.FirstName} {contractEntity.Approver?.LastName}",
                Room = _roomMapper.MapToRoomResponseModel(contractEntity.Room),
                User = _userMapper.MapToUserResponseModel(contractEntity.User),
                Workplace = contractEntity.User.Workplace != null ? _workplaceMapper.MapToWorkplaceResponseModel(contractEntity.User.Workplace) : null,
                HealthInsurance = contractEntity.User.HealthInsurance != null ? _healthInsuranceMapper.MapToHealthInsuranceResponseModel(contractEntity.User.HealthInsurance) : null,
                Guardians = contractEntity.User.Guardians?.Count > 0 ? contractEntity.User.Guardians.Select(g => _guardianMapper.MapToGuardianResponseModel(g)).ToList() : [],
                Vehicles = contractEntity.User.Vehicles?.Count > 0 ? contractEntity.User.Vehicles.Select(vehicle => _vehicleMapper.MapToVehicleResponseModel(vehicle)).ToList() : [],
            };
        }

        public ContractEntity MapToContractEntity(ContractRequestModel model)
        {
            return new ContractEntity
            {
                SubmissionDate = DateTime.Today,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                Status = ContractStatusEnum.PENDING,
                NumberExtension = 0,
                RoomId = model.RoomId,
                UserId = model.UserId,
                CreatedDateUtc = DateTime.UtcNow,
                LastUpdatedDateUtc = DateTime.UtcNow,
            };
        }
    }
}
