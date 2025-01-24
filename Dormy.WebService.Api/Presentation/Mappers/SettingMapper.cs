using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;

namespace Dormy.WebService.Api.Presentation.Mappers
{
    public class SettingMapper
    {
        public SettingEntity MapToSettingEntity(SettingRequestModel model)
        {
            return new SettingEntity
            {
                Id = Guid.NewGuid(),
                Name = model.Name,
                CreatedDateUtc = DateTime.Now,
                ParameterBool = model.ParameterBool,
                ParameterDate = model.ParameterDate,
            };
        }

        public SettingResponseModel MapToSettingResponseModel(SettingEntity entity)
        {
            return new()
            {
                Id = entity.Id,
                Name = entity.Name,
                CreatedDateUtc = DateTime.Now,
                ParameterBool = entity.ParameterBool,
                ParameterDate = entity.ParameterDate,
                CreatedBy = entity.CreatedBy,
                LastUpdatedBy = entity.LastUpdatedBy,
                LastUpdatedDateUtc = entity.LastUpdatedDateUtc,
                IsDeleted = entity.IsDeleted,
            };
        }
    }
}
