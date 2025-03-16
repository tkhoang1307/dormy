using Dormy.WebService.Api.Core.Entities;

namespace Dormy.WebService.Api.Models.ResponseModels
{
    public class BaseResponseModel : BaseEntity
    {
        public string CreatedByCreator { get; set; } = string.Empty;

        public string LastUpdatedByUpdater { get; set; } = string.Empty;
    }
}
