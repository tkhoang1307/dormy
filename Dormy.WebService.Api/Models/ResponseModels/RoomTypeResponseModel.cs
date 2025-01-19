using Dormy.WebService.Api.Core.Entities;

namespace Dormy.WebService.Api.Models.ResponseModels
{
    public class RoomTypeResponseModel : BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string RoomTypeName { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int Capacity { get; set; }

        public decimal Price { get; set; }
    }
}
