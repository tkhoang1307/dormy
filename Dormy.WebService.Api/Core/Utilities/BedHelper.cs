using Dormy.WebService.Api.Core.Entities;

namespace Dormy.WebService.Api.Core.Utilities
{
    public static class BedHelper
    {
        public static bool IsBedOccupied(List<RoomEntity>? roomEntities)
        {
            return roomEntities != null && roomEntities.Any(r => r.Beds != null && r.Beds.Any(b => b.Status == Models.Enums.BedStatusEnum.OCCUPIED));
        }
    }
}
