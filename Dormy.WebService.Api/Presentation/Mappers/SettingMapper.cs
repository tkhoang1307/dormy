using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Models.Enums;
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
                KeyName = model.KeyName,
                Value = model.Value,
                DataType = (SettingDataTypeEnum)Enum.Parse(typeof(SettingDataTypeEnum), model.DataType),
                IsApplied = model?.IsApplied ?? false,
                CreatedDateUtc = DateTime.Now,
                LastUpdatedDateUtc = DateTime.Now,
            };
        }

        public SettingResponseModel MapToSettingResponseModel(SettingEntity entity)
        {
            return new()
            {
                Id = entity.Id,
                KeyName = entity.KeyName,
                Value = entity.Value,
                DataType = entity.DataType.ToString(),
                IsApplied = entity.IsApplied,
                CreatedDateUtc = entity.CreatedDateUtc,
                CreatedBy = entity.CreatedBy,
                LastUpdatedBy = entity.LastUpdatedBy,
                LastUpdatedDateUtc = entity.LastUpdatedDateUtc,
                IsDeleted = entity.IsDeleted,
            };
        }
    }
}
