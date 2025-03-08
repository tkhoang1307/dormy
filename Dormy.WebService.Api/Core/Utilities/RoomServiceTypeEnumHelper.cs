using Dormy.WebService.Api.Models.Enums;
using Dormy.WebService.Api.Models.ResponseModels;
using System.ComponentModel;

namespace Dormy.WebService.Api.Core.Utilities
{
    public class RoomServiceTypeEnumHelper
    {
        public static List<RoomServiceTypeResponseModel> GetAllRoomServiceTypes()
        {
            return Enum.GetValues(typeof(RoomServiceTypeEnum))
                       .Cast<RoomServiceTypeEnum>()
                       .Select(service => GetRoomServiceTypeResponse(service))
                       .ToList();
        }

        private static RoomServiceTypeResponseModel GetRoomServiceTypeResponse(RoomServiceTypeEnum serviceType)
        {
            var description = EnumHelper.GetEnumDescription(serviceType);
            var parts = description.Split(" /*-*/ ", StringSplitOptions.None);

            return new RoomServiceTypeResponseModel
            {
                RoomServiceType = serviceType.ToString(),
                VietnameseRoomServiceTypeName = parts.Length > 1 ? parts[1] : string.Empty,
                EnglishRoomServiceTypeName = parts.Length > 0 ? parts[0] : string.Empty
            };
        }
    }
}
