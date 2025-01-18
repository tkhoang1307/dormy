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
                GuardianId = entity.GuardianId,
                DateOfBirth = entity.DateOfBirth,
                GuardianName = entity.Guardian?.Name ?? string.Empty,
                HealthInsuranceId = entity.HealthInsuranceId,
                NationalIdNumber = entity.NationalIdNumber,
                Status = entity.Status.ToString(),
                WorkplaceId = entity.WorkplaceId,
                WorkplaceName = entity.Workplace?.Name ?? string.Empty,
            };
        }
    }
}
