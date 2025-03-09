using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Models.ResponseModels;

namespace Dormy.WebService.Api.Presentation.Mappers
{
    public class RequestMapper
    {
        private readonly RoomMapper _roomMapper;
        private readonly ContractMapper _contractMapper;

        public RequestMapper()
        {
            _roomMapper = new();
            _contractMapper = new();
        }

        public RequestResponseModel MapToRequestModel(RequestEntity requestEntity)
        {
            return new RequestResponseModel
            {
                Id = requestEntity.Id,
                Description = requestEntity.Description,
                Status = requestEntity.Status.ToString(),
                RequestType = requestEntity.RequestType,
                ApproverId = requestEntity.ApproverId,
                UserId = requestEntity.UserId,
                RoomId = requestEntity.RoomId,
                ApproverName = $"{requestEntity.Approver?.FirstName} {requestEntity.Approver?.LastName}",
                UserName = $"{requestEntity.User?.FirstName} {requestEntity.User?.LastName}",
                RoomNumber = requestEntity.Room?.RoomNumber,
                FloorNumber = requestEntity.Room?.FloorNumber,
                RoomStatus = requestEntity.Room?.Status.ToString(),
                BuildingId = requestEntity.Room?.BuildingId,
                BuildingName = requestEntity.Room?.Building?.Name
            };
        }
    }
}
