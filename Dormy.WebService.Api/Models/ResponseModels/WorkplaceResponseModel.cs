using Dormy.WebService.Api.Core.Entities;

namespace Dormy.WebService.Api.Models.ResponseModels
{
    public class WorkplaceResponseModel: BaseEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string Abbrevation { get; set; } = string.Empty;

        public List<UserResponseModel> Users { get; set; } = [];

        public string CreatedByAdminName { get; set; } = string.Empty;

        public string LastUpdatedByAdminName { get; set; } = string.Empty;

    }
}
