using Dormy.WebService.Api.ApplicationLogic;
using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Models.RequestModels;

namespace Dormy.WebService.Api.Presentation.Mappers
{
    public class RoomTypeServiceMapper
    {
        public RoomTypeServiceEntity MapToRoomTypeServiceEntity(RoomTypeServiceRequestModel model)
        {
            return new RoomTypeServiceEntity
            {
                RoomServiceId = model.RoomServiceId,
                RoomTypeId = model.RoomTypeId,
                CreatedDateUtc = DateTime.UtcNow,
                LastUpdatedDateUtc = DateTime.UtcNow,
            };
        }
    }
}
