using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;

namespace Dormy.WebService.Api.Presentation.Mappers
{
    public class HealthInsuranceMapper
    {
        public HealthInsuranceEntity MapToHealthInsuranceEntity(HealthInsuranceRequestModel model)
        {
            return new HealthInsuranceEntity
            {
                InsuranceCardNumber = model.InsuranceCardNumber,
                RegisteredHospital = model.RegisteredHospital,
                ExpirationDate = model.ExpirationDate,
                CreatedDateUtc = DateTime.UtcNow,
                LastUpdatedDateUtc = DateTime.UtcNow,
            };
        }

        public HealthInsuranceResponseModel MapToHealthInsuranceResponseModel(HealthInsuranceEntity model)
        {
            return new HealthInsuranceResponseModel
            {
                Id = model.Id,
                InsuranceCardNumber = model.InsuranceCardNumber,
                RegisteredHospital = model.RegisteredHospital,
                ExpirationDate = model.ExpirationDate,
                //User = _userMapper.MapToUserResponseModel(model.User),
                CreatedDateUtc = model.CreatedDateUtc,
                CreatedBy = model.CreatedBy,
                LastUpdatedBy = model.LastUpdatedBy,
                LastUpdatedDateUtc = model.LastUpdatedDateUtc,
                IsDeleted = model.IsDeleted,
            };
        }
    }
}
