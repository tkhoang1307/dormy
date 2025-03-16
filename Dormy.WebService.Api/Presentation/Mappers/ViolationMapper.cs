using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Models.Enums;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;

namespace Dormy.WebService.Api.Presentation.Mappers
{
    public class ViolationMapper
    {
        public ViolationEntity MapToViolationEntity(ViolationRequestModel model)
        {
            return new ViolationEntity
            {
                UserId = model.UserId,
                Description = model.Description,
                Penalty = model.Penalty,
                ViolationDate = model.ViolationDate,
                CreatedDateUtc = DateTime.UtcNow,
                LastUpdatedDateUtc = DateTime.UtcNow,
            };
        }

        public ViolationResponseModel MapToViolationResponseModel(ViolationEntity entity)
        {
            return new ViolationResponseModel
            {
                Id = entity.Id,
                UserId = entity.UserId,
                Description = entity.Description,
                Penalty = entity.Penalty,
                ViolationDate = entity.ViolationDate,
                DateOfBirth = entity.User.DateOfBirth,
                FullName = entity.User.LastName + " " + entity.User.FirstName,
                Email = entity.User.Email,
                PhoneNumber = entity.User.PhoneNumber,
                CreatedBy = entity.CreatedBy,
                LastUpdatedBy = entity.LastUpdatedBy,
                CreatedDateUtc = entity.CreatedDateUtc,
                LastUpdatedDateUtc = entity.LastUpdatedDateUtc,
                IsDeleted = entity.IsDeleted,
            };
        }
    }
}
