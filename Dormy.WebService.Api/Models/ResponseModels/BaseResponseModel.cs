using Dormy.WebService.Api.Core.Entities;

namespace Dormy.WebService.Api.Models.ResponseModels
{
    public class BaseResponseModel : BaseEntity
    {
        public string CreatedByAdminName { get; set; } = string.Empty;

        public string LastUpdatedByAdminName { get; set; } = string.Empty;
    }
}
