using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Core.Interfaces;
using Dormy.WebService.Api.Models.Constants;
using Dormy.WebService.Api.Models.Enums;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;
using Dormy.WebService.Api.Startup;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace Dormy.WebService.Api.ApplicationLogic
{
    public class ContractExtensionService : IContractExtensionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContextService _userContextService;
        private readonly IContractService _contractService;

        public ContractExtensionService(IUnitOfWork unitOfWork, IUserContextService userContextService, IContractService contractService)
        {
            _unitOfWork = unitOfWork;
            _userContextService = userContextService;
            _contractService = contractService;
        }

        public async Task<ApiResponse> CreateContractExtension(ContractExtensionRequestModel model)
        {
            // Verify startdate and enddate and not in the past
            if (model.StartDate < DateTime.Now || model.EndDate < DateTime.Now)
            {
                return new ApiResponse().SetBadRequest("Start date and end date must be in the future");
            }

            if (model.StartDate < DateTime.Now || model.EndDate < DateTime.Now)
            {
                return new ApiResponse().SetBadRequest(message: "Start date and end date must be in the future");
            }

            if (model.StartDate > model.EndDate)
            {
                return new ApiResponse().SetUnprocessableEntity(message: "Start date cannot be after end date");
            }

            var contractEntity = await _unitOfWork.ContractRepository.GetAsync(x => x.Id == model.ContractId && x.UserId == _userContextService.UserId, x => x.Include(x => x.Room), isNoTracking: true);
            if (contractEntity == null)
            {
                return new ApiResponse().SetNotFound("Contract not found");
            }

            if (model.StartDate < contractEntity.EndDate || contractEntity.Status != ContractStatusEnum.EXPIRED)
            {
                return new ApiResponse().SetConflict(message: "Contract has not been expired yet");
            }

            if (contractEntity.NumberExtension >= 3)
            {
                return new ApiResponse().SetConflict(message: "Contract has reached maximum number of extension");
            }

            if (contractEntity.Room.Status == RoomStatusEnum.FULL)
            {
                return new ApiResponse().SetConflict(message: "Room is full");
            }

            var contractExtensionEntity = new ContractExtensionEntity
            {
                ContractId = model.ContractId,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                Status = ContractExtensionStatusEnum.PENDING,
                SubmissionDate = DateTime.UtcNow,
                CreatedDateUtc = DateTime.UtcNow,
                CreatedBy = _userContextService.UserId,
            };

            contractEntity.NumberExtension += 1;

            await _unitOfWork.ContractExtensionRepository.AddAsync(contractExtensionEntity);
            await _unitOfWork.SaveChangeAsync();

            return new ApiResponse().SetOk(contractExtensionEntity.Id);
        }

        public async Task<ApiResponse> GetContractExtensionBatch(GetBatchRequestModel model)
        {
            var userId = _userContextService.UserId;
            if (userId == Guid.Empty)
            {
                return new ApiResponse().SetNotFound(message: "User not found");
            }

            var contractExtensionEntities = new List<ContractExtensionEntity>();

            if (_userContextService.UserRoles.Contains(Role.ADMIN))
            {
                if (model.IsGetAll)
                {
                    contractExtensionEntities =
                        await _unitOfWork.ContractExtensionRepository
                            .GetAllAsync(x => true, isNoTracking: true);
                }
                else
                {
                    contractExtensionEntities =
                        await _unitOfWork.ContractExtensionRepository
                            .GetAllAsync(x => model.Ids.Contains(x.Id), isNoTracking: true);
                }
            }
            else
            {
                if (model.IsGetAll)
                {
                    contractExtensionEntities =
                        await _unitOfWork.ContractExtensionRepository
                        .GetAllAsync(x => x.Contract.UserId == userId, x => x.Include(x => x.Contract),
                            isNoTracking: true);
                }
                else
                {
                    contractExtensionEntities =
                        await _unitOfWork.ContractExtensionRepository
                        .GetAllAsync(x => x.Contract.UserId == userId && model.Ids.Contains(x.Id), x => x.Include(x => x.Contract),
                            isNoTracking: true);
                }
            }

            if (!model.IsGetAll)
            {
                if (contractExtensionEntities.Count != model.Ids.Count)
                {
                    // Find the missing request IDs
                    var foundRequestIds = contractExtensionEntities.Select(r => r.Id).ToList();
                    var missingRequestIds = model.Ids.Except(foundRequestIds).ToList();

                    // Return with error message listing the missing request IDs
                    var errorMessage = $"Contract(s) not found: {string.Join(", ", missingRequestIds)}";
                    return new ApiResponse().SetNotFound(message: errorMessage);
                }
            }

            var response = contractExtensionEntities.Select(async x => new ContractExtensionResponseModel
            {
                Id = x.Id,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                Status = x.Status.ToString(),
                SubmissionDate = x.SubmissionDate,
                Contract = (await _contractService.GetSingleContract(x.ContractId)).Result as ContractResponseModel
            }).ToList();

            return new ApiResponse().SetOk(response);
        }

        public async Task<ApiResponse> GetSingleContractExtensionById(Guid id)
        {
            var contractExtensionEntity = await _unitOfWork.ContractExtensionRepository.GetAsync(x => x.Id == id, isNoTracking: true);
            if (contractExtensionEntity == null)
            {
                return new ApiResponse().SetNotFound("Contract extension not found");
            }

            var contractExtensionResponse = new ContractExtensionResponseModel
            {
                Id = contractExtensionEntity.Id,
                StartDate = contractExtensionEntity.StartDate,
                EndDate = contractExtensionEntity.EndDate,
                Status = contractExtensionEntity.Status.ToString(),
                SubmissionDate = contractExtensionEntity.SubmissionDate,
                Contract = (await _contractService.GetSingleContract(contractExtensionEntity.ContractId)).Result as ContractResponseModel
            };

            return new ApiResponse().SetOk(contractExtensionResponse);
        }

        public async Task<ApiResponse> UpdateContractExtension(Guid id, ContractExtensionRequestModel model)
        {
            // Verify startdate and enddate and not in the past
            if (model.StartDate < DateTime.Now || model.EndDate < DateTime.Now)
            {
                return new ApiResponse().SetBadRequest("Start date and end date must be in the future");
            }

            if (model.StartDate < DateTime.Now || model.EndDate < DateTime.Now)
            {
                return new ApiResponse().SetBadRequest(message: "Start date and end date must be in the future");
            }

            if (model.StartDate > model.EndDate)
            {
                return new ApiResponse().SetUnprocessableEntity(message: "Start date cannot be after end date");
            }

            var contractExtensionEntity = await _unitOfWork.ContractExtensionRepository.GetAsync(x => x.Id == id, isNoTracking: false);
            if (contractExtensionEntity == null)
            {
                return new ApiResponse().SetNotFound("Contract extension not found");
            }

            if (contractExtensionEntity.Status != ContractExtensionStatusEnum.PENDING)
            {
                return new ApiResponse().SetConflict("Contract extension is not pending");
            }

            var contractEntity = await _unitOfWork.ContractRepository.GetAsync(x => x.Id == contractExtensionEntity.ContractId && x.UserId == _userContextService.UserId, x => x.Include(x => x.Room), isNoTracking: true);
            if (contractEntity == null)
            {
                return new ApiResponse().SetNotFound("Contract not found");
            }

            if (model.StartDate < contractEntity.EndDate || contractEntity.Status != ContractStatusEnum.EXPIRED)
            {
                return new ApiResponse().SetConflict(message: "Contract has not been expired yet");
            }

            if (contractEntity.NumberExtension >= 3)
            {
                return new ApiResponse().SetConflict(message: "Contract has reached maximum number of extension");
            }

            if (contractEntity.Room.Status == RoomStatusEnum.FULL)
            {
                return new ApiResponse().SetConflict(message: "Room is full");
            }

            contractExtensionEntity.StartDate = model.StartDate;
            contractExtensionEntity.EndDate = model.EndDate;
            contractExtensionEntity.LastUpdatedBy = _userContextService.UserId;
            contractExtensionEntity.ContractId = model.ContractId;
            contractExtensionEntity.LastUpdatedDateUtc = DateTime.UtcNow;

            await _unitOfWork.SaveChangeAsync();

            return new ApiResponse().SetOk();
        }

        public async Task<ApiResponse> UpdateContractExtensionStatus(Guid id, ContractExtensionStatusEnum status)
        {
            var userId = _userContextService.UserId;
            if (userId == Guid.Empty)
            {
                return new ApiResponse().SetForbidden(message: "User not authenticated");
            }

            using (var scope = new TransactionScope(
                TransactionScopeOption.Required,
                new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                TransactionScopeAsyncFlowOption.Enabled))
            {
                ContractExtensionEntity? contractExtensionEntity;

                if (_userContextService.UserRoles.Contains(Role.ADMIN))
                {
                    contractExtensionEntity = await _unitOfWork.ContractExtensionRepository.GetAsync(x => x.Id == id, x => x.Include(x => x.Contract));
                }
                else
                {
                    contractExtensionEntity = await _unitOfWork.ContractExtensionRepository.GetAsync(x => x.Id == id && x.Contract.UserId == userId, x => x.Include(x => x.Contract));
                }

                if (contractExtensionEntity == null)
                {
                    return new ApiResponse().SetNotFound(message: "Contract not found");
                }

                contractExtensionEntity.Status = status;
                contractExtensionEntity.LastUpdatedBy = userId;
                contractExtensionEntity.LastUpdatedDateUtc = DateTime.UtcNow;

                switch (status)
                {
                    case ContractExtensionStatusEnum.ACTIVE:
                        {
                            if (contractExtensionEntity.Contract.Status != ContractStatusEnum.WAITING_PAYMENT)
                            {
                                return new ApiResponse().SetConflict(message: "Contract has not complete payment yet");
                            }

                            var roomEntity = await _unitOfWork.RoomRepository.GetAsync(x => x.Id == contractExtensionEntity.Contract.RoomId);
                            if (roomEntity == null)
                            {
                                return new ApiResponse().SetNotFound(message: "Room not found");
                            }

                            if (roomEntity.Status == RoomStatusEnum.FULL)
                            {
                                return new ApiResponse().SetConflict(message: "Room is full");
                            }

                            roomEntity.TotalUsedBed += 1;
                            roomEntity.Status = roomEntity.TotalUsedBed == roomEntity.TotalAvailableBed ? RoomStatusEnum.FULL : RoomStatusEnum.AVAILABLE;
                            roomEntity.LastUpdatedBy = userId;
                            roomEntity.LastUpdatedDateUtc = DateTime.UtcNow;

                            contractExtensionEntity.Contract.StartDate = contractExtensionEntity.StartDate;
                            contractExtensionEntity.Contract.EndDate = contractExtensionEntity.EndDate;
                            contractExtensionEntity.Contract.Status = ContractStatusEnum.EXTENDED;
                            contractExtensionEntity.Contract.LastUpdatedBy = userId;
                            contractExtensionEntity.Contract.LastUpdatedDateUtc = DateTime.UtcNow;

                            break;
                        }
                    case ContractExtensionStatusEnum.EXPIRED:
                    case ContractExtensionStatusEnum.TERMINATED:
                        {
                            var roomEntity = await _unitOfWork.RoomRepository.GetAsync(x => x.Id == contractExtensionEntity.Contract.RoomId);
                            if (roomEntity == null)
                            {
                                return new ApiResponse().SetNotFound(message: "Room not found");
                            }

                            roomEntity.TotalUsedBed = roomEntity.TotalUsedBed > 0 ? roomEntity.TotalUsedBed - 1 : 0;
                            roomEntity.Status = roomEntity.TotalUsedBed == roomEntity.TotalAvailableBed ? RoomStatusEnum.FULL : RoomStatusEnum.AVAILABLE;
                            roomEntity.LastUpdatedBy = userId;
                            roomEntity.LastUpdatedDateUtc = DateTime.UtcNow;

                            contractExtensionEntity.Contract.Status = ContractStatusEnum.TERMINATED;

                            break;
                        }
                }

                await _unitOfWork.SaveChangeAsync();
                scope.Complete();
            }
            return new ApiResponse().SetOk();
        }
    }
}
