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
                InvoiceId = contractEntity.InvoiceId,
                ApproverId = contractEntity.ApproverId,
                UserId = contractEntity.UserId,
                ApproverFullName = contractEntity.Approver == null ? string.Empty : $"{contractEntity.Approver?.FirstName} {contractEntity.Approver?.LastName}",
                UserFullname = contractEntity.User == null ? string.Empty : $"{contractEntity.User.FirstName} {contractEntity.User.LastName}",
                RoomId = contractEntity.Room.Id,
                RoomNumber = contractEntity.Room.RoomNumber,
                RoomTypeId = contractEntity.Room.RoomType.Id,
                RoomTypeName = contractEntity.Room.RoomType.RoomTypeName,
                Price = contractEntity.Room.RoomType.Price,
                BuildingId = contractEntity.Room.BuildingId,
                BuildingName = contractEntity.Room.Building.Name,
                WorkplaceId = contractEntity.User?.WorkplaceId,
                WorkplaceName = contractEntity.User?.Workplace.Name,
                InsuranceCardNumber = contractEntity.User.HealthInsurance.InsuranceCardNumber,
                RegisteredHospital = contractEntity.User.HealthInsurance.RegisteredHospital,
                ExpirationDate = contractEntity.User.HealthInsurance.ExpirationDate,
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
