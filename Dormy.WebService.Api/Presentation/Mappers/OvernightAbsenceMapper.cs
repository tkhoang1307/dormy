using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Models.Enums;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;

namespace Dormy.WebService.Api.Presentation.Mappers
{
    public class OvernightAbsenceMapper
    {
        public OvernightAbsenceEntity MapToOvernightAbsenceEntity(OvernightAbsentRequestModel model)
        {
            return new OvernightAbsenceEntity
            {
                Reason = model.Reason,
                StartDateTime = model.StartDateTime,
                EndDateTime = model.EndDateTime,
                Status = OvernightAbsenceStatusEnum.SUBMITTED,
                CreatedDateUtc = DateTime.UtcNow,
                LastUpdatedDateUtc = DateTime.UtcNow,
            };
        }

        public OvernightAbsentModel MapToOvernightAbsentModel(OvernightAbsenceEntity entity)
        {
            return new OvernightAbsentModel
            {
                Id = entity.Id,
                Reason = entity.Reason,
                StartDateTime = entity.StartDateTime,
                EndDateTime = entity.EndDateTime,
                Status = entity.Status.ToString(),
                UserId = entity.UserId,
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
