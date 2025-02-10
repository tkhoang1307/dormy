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
            return new UserResponseModel
            {
                UserId = entity.Id,
                UserName = entity.UserName,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Email = entity.Email,
                PhoneNumber = entity.PhoneNumber,
                Gender = entity.Gender.ToString(),
                Guardians = entity.Guardians != null ? entity.Guardians.Select(x => new GuardiansResponseModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                }).ToList() : [],
                DateOfBirth = entity.DateOfBirth,
                HealthInsuranceId = entity.HealthInsuranceId,
                NationalIdNumber = entity.NationalIdNumber,
                Status = entity.Status.ToString(),
                WorkplaceId = entity.WorkplaceId,
                WorkplaceName = entity.Workplace?.Name ?? string.Empty,
                HealthInsurance = entity.HealthInsurance != null ? _healthInsuranceMapper.MapToHealthInsuranceResponseModel(entity.HealthInsurance) : null,
            };
        }

        public UserEntity MapToUserEntity(UserRequestModel model)
        {
            return new UserEntity()
            {
                Email = model.Email,
                DateOfBirth = model.DateOfBirth,
                Gender = (GenderEnum)Enum.Parse(typeof(GenderEnum), model.Gender),
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
