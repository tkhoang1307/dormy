using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Models.ResponseModels;

namespace Dormy.WebService.Api.Presentation.Mappers
{
    public class UserMapper
    {
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
            };
        }
    }
}
