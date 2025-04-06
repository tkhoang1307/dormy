using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Models.Enums;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;

namespace Dormy.WebService.Api.Presentation.Mappers
{
    public class RequestMapper
    {
        public RequestEntity MapToRequestEntity(RequestRequestModel model)
        {
            return new RequestEntity
            {
                Description = model.Description,
                RequestType = model.RequestType,
                Status = RequestStatusEnum.SUBMITTED,
                CreatedDateUtc = DateTime.UtcNow,
                LastUpdatedDateUtc = DateTime.UtcNow,
            };
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
                BuildingId = requestEntity.Room?.BuildingId,
                BuildingName = requestEntity.Room?.Building?.Name,
                IsDeleted = requestEntity.IsDeleted,
                CreatedDateUtc = requestEntity.CreatedDateUtc,
            };
        }
    }
}
