using Dormy.WebService.Api.Models.RequestModels;

namespace Dormy.WebService.Api.Core.Utilities
{
    public class RoomHelper
    {
        public static int CalculateTotalRoomsWereCreatedBeforeInARequest(List<RoomCreationRequestModel> rooms, int positionCreatingRoomInRequest)
        {
            int totalRoomsCreated = 0;
            var room = rooms[positionCreatingRoomInRequest];
            for (int jRoom = 0; jRoom < positionCreatingRoomInRequest; jRoom++)
            {
                if (room.FloorNumber == rooms[jRoom].FloorNumber)
                {
                    totalRoomsCreated = totalRoomsCreated + rooms[jRoom].TotalRoomsWantToCreate;
                }
            }

            return totalRoomsCreated;
        }

        public static int CalculateStartedRoomNumberInARequest(int floorNumber, int maxRoomNumberOnFloor, int totalRoomsCreatedBefore)
        {
            int roomNumberStartToMark = 0;
            if (maxRoomNumberOnFloor > 0)
            {
                roomNumberStartToMark = maxRoomNumberOnFloor + totalRoomsCreatedBefore + 1;
            }
            else
            {
                roomNumberStartToMark = floorNumber * 100 + totalRoomsCreatedBefore;
            }

            return roomNumberStartToMark;
        }
    }
}
