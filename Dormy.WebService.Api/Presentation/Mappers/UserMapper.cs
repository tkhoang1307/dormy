using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Infrastructure.TokenRetriever;
using Dormy.WebService.Api.Models.Enums;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;

namespace Dormy.WebService.Api.Presentation.Mappers
{
    public class UserMapper
    {
        private readonly HealthInsuranceMapper _healthInsuranceMapper;

        public UserMapper()
        {
            _healthInsuranceMapper = new();
        }

        public UserResponseModel MapToUserResponseModel(UserEntity entity)
        {
            if (entity == null)
            {
                return new UserResponseModel();
            }

            var activeContract = entity?.Contracts?.FirstOrDefault(c => c.Status == ContractStatusEnum.ACTIVE || c.Status == ContractStatusEnum.EXTENDED);
            return new UserResponseModel
            {
                UserId = entity!.Id,
                UserName = entity.UserName,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Email = entity.Email,
                PhoneNumber = entity.PhoneNumber,
                Gender = entity.Gender.ToString(),
                DateOfBirth = entity.DateOfBirth,
                NationalIdNumber = entity.NationalIdNumber,
                Status = entity.Status.ToString(),
                ContractId = activeContract?.Id,
                ContractStatus = activeContract?.Status.ToString(),
                ContractStartDate = activeContract?.StartDate,
                ContractEndDate = activeContract?.EndDate,
                RoomId = activeContract?.RoomId,
            };
        }

        public UserEntity MapToUserEntity(UserMapperRequestModel model)
        {
            return new UserEntity()
            {
                Email = model.Email,
                DateOfBirth = model.DateOfBirth,
                Gender = (GenderEnum)Enum.Parse(typeof(GenderEnum), model.Gender),
                NationalIdNumber = model.NationalIdNumber,
                UserName = model.UserName,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Password = EncryptHelper.HashPassword(model.Password),
                PhoneNumber = model.PhoneNumber,
                CreatedDateUtc = DateTime.UtcNow,
                LastUpdatedDateUtc = DateTime.UtcNow,
            };
        }
    }
}
