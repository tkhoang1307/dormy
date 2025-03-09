using Dormy.WebService.Api.Core.CustomExceptions;
using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Core.Interfaces;
using Dormy.WebService.Api.Models.Constants;
using Dormy.WebService.Api.Models.Enums;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;
using Dormy.WebService.Api.Presentation.Mappers;
using Dormy.WebService.Api.Startup;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace Dormy.WebService.Api.ApplicationLogic
{
    public class RequestService : IRequestService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContextService _userContextService;
        private readonly RequestMapper _requestMapper;

        public RequestService(IUnitOfWork unitOfWork, IUserContextService userContextService)
        {
            _unitOfWork = unitOfWork;
            _userContextService = userContextService;
            _requestMapper = new RequestMapper();
        }

        public async Task<ApiResponse> CreateRequest(RequestRequestModel model)
        {
            var requestEntity = new RequestEntity
            {
                Description = model.Description,
                RequestType = model.RequestType,
                UserId = _userContextService.UserId,
                Status = RequestStatusEnum.SUBMITTED,
                CreatedBy = _userContextService.UserId,
            };

            if (model.RoomId != null)
            {
                var roomEntity = await _unitOfWork.RoomRepository.GetAsync(x => x.Id == model.RoomId.Value, isNoTracking: true);
                if (roomEntity == null)
                {
                    return new ApiResponse().SetNotFound("Room not found");
                }
                requestEntity.RoomId = model.RoomId.Value;
            }

            await _unitOfWork.RequestRepository.AddAsync(requestEntity);
            await _unitOfWork.SaveChangeAsync();

            return new ApiResponse().SetOk(requestEntity.Id);
        }

        public async Task<ApiResponse> GetRequests(GetBatchRequestModel model)
        {
            var userId = _userContextService.UserId;
            if (userId == Guid.Empty)
            {
                return new ApiResponse().SetNotFound("User not found");
            }

            var requestEntities = new List<RequestEntity>();

            if (_userContextService.UserRoles.Contains(Role.ADMIN))
            {
                if (model.IsGetAll)
                {
                    requestEntities = await _unitOfWork.RequestRepository.GetAllAsync(x => true, x => x.Include(x => x.Approver).Include(x => x.User).Include(x => x.Room).ThenInclude(x => x.Building));
                }
                else
                {
                    requestEntities = await _unitOfWork.RequestRepository.GetAllAsync(x => model.Ids.Contains(x.Id), x => x.Include(x => x.Approver).Include(x => x.User).Include(x => x.Room).ThenInclude(x => x.Building));
                }
            }
            else
            {
                if (model.IsGetAll)
                {
                    requestEntities = await _unitOfWork.RequestRepository.GetAllAsync(x => x.UserId == userId, x => x.Include(x => x.Approver).Include(x => x.User).Include(x => x.Room).ThenInclude(x => x.Building));
                }
                else
                {
                    requestEntities = await _unitOfWork.RequestRepository.GetAllAsync(x => model.Ids.Contains(x.Id) && x.UserId == userId, x => x.Include(x => x.Approver).Include(x => x.User).Include(x => x.Room).ThenInclude(x => x.Building));
                }
            }

            if (!model.IsGetAll)
            {
                if (requestEntities.Count != model.Ids.Count)
                {
                    // Find the missing request IDs
                    var foundRequestIds = requestEntities.Select(r => r.Id).ToList();
                    var missingRequestIds = model.Ids.Except(foundRequestIds).ToList();

                    // Return with error message listing the missing request IDs
                    var errorMessage = $"Request(s) not found: {string.Join(", ", missingRequestIds)}";
                    return new ApiResponse().SetNotFound(message: errorMessage);
                }
            }

            var response = requestEntities.Select(x => _requestMapper.MapToRequestModel(x)).ToList();
            return new ApiResponse().SetOk(response);
        }

        public async Task<ApiResponse> GetSingleRequest(Guid id)
        {
            var userId = _userContextService.UserId;
            if (userId == Guid.Empty)
            {
                return new ApiResponse().SetNotFound("User not found");
            }

            var requestEntity = new RequestEntity();

            if (_userContextService.UserRoles.Contains(Role.ADMIN))
            {
                requestEntity = await _unitOfWork.RequestRepository.GetAsync(x => x.Id == id, x => x.Include(x => x.Approver).Include(x => x.User).Include(x => x.Room).ThenInclude(x => x.Building));
            }
            else
            {
                requestEntity = await _unitOfWork.RequestRepository.GetAsync(x => x.Id == id && x.UserId == userId, x => x.Include(x => x.Approver).Include(x => x.User).Include(x => x.Room).ThenInclude(x => x.Building));
            }

            if (requestEntity == null)
            {
                return new ApiResponse().SetNotFound("Request not found");
            }

            var response = _requestMapper.MapToRequestModel(requestEntity);
            return new ApiResponse().SetOk(response);
        }

        public async Task<ApiResponse> UpdateRequest(Guid id, RequestRequestModel model)
        {
            var requestEntity = await _unitOfWork.RequestRepository.GetAsync(x => x.Id == id);
            if (requestEntity == null)
            {
                return new ApiResponse().SetNotFound("Request not found");
            }

            if (requestEntity.UserId != _userContextService.UserId)
            {
                return new ApiResponse().SetForbidden("You are not allowed to update this request");
            }

            if (requestEntity.Status != RequestStatusEnum.SUBMITTED)
            {
                return new ApiResponse().SetBadRequest("Request is not in SUBMITTED status");
            }

            requestEntity.Description = model.Description;
            requestEntity.RequestType = model.RequestType;

            if (model.RoomId != null)
            {
                var roomEntity = await _unitOfWork.RoomRepository.GetAsync(x => x.Id == model.RoomId.Value, isNoTracking: true);
                if (roomEntity == null)
                {
                    return new ApiResponse().SetNotFound("Room not found");
                }
                requestEntity.RoomId = model.RoomId.Value;
            }

            await _unitOfWork.SaveChangeAsync();

            return new ApiResponse().SetOk(requestEntity.Id);
        }

        public async Task<ApiResponse> UpdateRequestStatus(Guid id, RequestStatusEnum status)
        {
            var userId = _userContextService.UserId;
            if (userId == Guid.Empty)
            {
                return new ApiResponse().SetNotFound("User not found");
            }

            var requestEntity = await _unitOfWork.RequestRepository.GetAsync(x => x.Id == id, isNoTracking: false);

            if (requestEntity == null)
            {
                return new ApiResponse().SetNotFound("Request not found");
            }

            switch (status)
            {
                case RequestStatusEnum.APPROVED:
                case RequestStatusEnum.REJECTED:
                    {
                        if (requestEntity.Status != RequestStatusEnum.SUBMITTED)
                        {
                            throw new BadRequestException(message: "Overnight absence is not available for approve/ reject");
                        }
                    }
                    break;
                case RequestStatusEnum.CANCELLED:
                    {
                        if (requestEntity.Status == RequestStatusEnum.APPROVED || requestEntity.Status == RequestStatusEnum.REJECTED)
                        {
                            throw new BadRequestException(message: "Overnight absence is already approved or rejected");
                        }
                    }
                    break;
            }

            requestEntity.Status = status;
            requestEntity.LastUpdatedBy = userId;
            requestEntity.LastUpdatedDateUtc = DateTime.UtcNow;

            await _unitOfWork.SaveChangeAsync();

            return new ApiResponse().SetOk();
        }
    }
}
