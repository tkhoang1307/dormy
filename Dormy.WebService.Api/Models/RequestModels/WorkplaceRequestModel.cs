using Dormy.WebService.Api.Core.Entities;

namespace Dormy.WebService.Api.Models.RequestModels
{
    public class WorkplaceRequestModel
    {
        public string Name { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string Abbrevation { get; set; } = string.Empty;
    }

    public class WorkplaceUpdateRequestModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string Abbrevation { get; set; } = string.Empty;
    }
}
