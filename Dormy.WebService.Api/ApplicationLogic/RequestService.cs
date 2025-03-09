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
                    requestEntities = await _unitOfWork.RequestRepository.GetAllAsync(x => true, x => x.Include(x => x.Approver).Include(x => x.User).Include(x => x.Room));
                }
                else
                {
                    requestEntities = await _unitOfWork.RequestRepository.GetAllAsync(x => model.Ids.Contains(x.Id), x => x.Include(x => x.Approver).Include(x => x.User).Include(x => x.Room));
                }
            }
            else
            {
                if (model.IsGetAll)
                {
                    requestEntities = await _unitOfWork.RequestRepository.GetAllAsync(x => x.UserId == userId, x => x.Include(x => x.Approver).Include(x => x.User).Include(x => x.Room));
                }
                else
                {
                    requestEntities = await _unitOfWork.RequestRepository.GetAllAsync(x => model.Ids.Contains(x.Id) && x.UserId == userId, x => x.Include(x => x.Approver).Include(x => x.User).Include(x => x.Room));
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
                requestEntity = await _unitOfWork.RequestRepository.GetAsync(x => x.Id == id, x => x.Include(x => x.Approver).Include(x => x.User).Include(x => x.Room));
            }
            else
            {
                requestEntity = await _unitOfWork.RequestRepository.GetAsync(x => x.Id == id && x.UserId == userId, x => x.Include(x => x.Approver).Include(x => x.User).Include(x => x.Room));
            }

            if (requestEntity == null)
            {
                return new ApiResponse().SetNotFound("Request not found");
            }

            var response = _requestMapper.MapToRequestModel(requestEntity);
            return new ApiResponse().SetOk(response);
        }

        public async Task<ApiResponse> UpdateRequestStatus(Guid id, RequestStatusEnum status)
        {
            var userId = _userContextService.UserId;
            if (userId == Guid.Empty)
            {
                return new ApiResponse().SetNotFound("User not found");
            }

            using (var scope = new TransactionScope(
                TransactionScopeOption.Required,
                new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                TransactionScopeAsyncFlowOption.Enabled))
            {
                var requestEntity = new RequestEntity();

                if (_userContextService.UserRoles.Contains(Role.ADMIN))
                {
                    requestEntity = await _unitOfWork.RequestRepository.GetAsync(x => x.Id == id, x => x.Include(x => x.Approver).Include(x => x.User).Include(x => x.Room));
                }
                else
                {
                    requestEntity = await _unitOfWork.RequestRepository.GetAsync(x => x.Id == id && x.UserId == userId, x => x.Include(x => x.Approver).Include(x => x.User).Include(x => x.Room));
                }

                if (requestEntity == null)
                {
                    return new ApiResponse().SetNotFound("Request not found");
                }

                requestEntity.Status = status;
                await _unitOfWork.SaveChangeAsync();

                // Complete transaction
                scope.Complete();
            }

            return new ApiResponse().SetOk();
        }
    }
}
