using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;

namespace Dormy.WebService.Api.Presentation.Mappers
{
    public class GuardianMapper
    {
        public GuardianEntity MapToGuardianEntity(GuardianRequestModel model)
        {
            return new GuardianEntity
            {
                Name = model.Name,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                RelationshipToUser = model.RelationshipToUser,
                CreatedDateUtc = DateTime.UtcNow,
                LastUpdatedDateUtc = DateTime.UtcNow,
            };
        }

        public GuardianResponseModel MapToGuardianResponseModel(GuardianEntity model)
        {
            return new GuardianResponseModel
            {
                Id = model.Id,
                Name = model.Name,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                RelationshipToUser = model.RelationshipToUser,
                CreatedDateUtc = model.CreatedDateUtc,
                CreatedBy = model.CreatedBy,
                LastUpdatedBy = model.LastUpdatedBy,
                LastUpdatedDateUtc = model.LastUpdatedDateUtc,
                IsDeleted = model.IsDeleted,
            };
        }
    }
}
