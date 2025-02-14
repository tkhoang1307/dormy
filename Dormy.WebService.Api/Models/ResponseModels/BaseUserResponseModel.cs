using Dormy.WebService.Api.Core.Entities;

namespace Dormy.WebService.Api.Models.ResponseModels
{
    public class BaseUserResponseModel : BaseEntity
    {
        public string CreatedByUserFullName { get; set; } = string.Empty;

        public string LastUpdatedByUserFullName { get; set; } = string.Empty;
    }
}
