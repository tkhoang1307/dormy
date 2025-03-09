﻿using Dormy.WebService.Api.Core.Constants;
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
    public class ContractService : IContractService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContextService _userContextService;
        private readonly IRoomTypeService _roomTypeService;
        private readonly UserMapper _userMapper;
        private readonly IUserService _userService;
        private readonly IHealthInsuranceService _healthInsuranceService;
        private readonly IGuardianService _guardianService;
        private readonly WorkplaceMapper _workplaceMapper;
        private readonly RoomTypeMapper _roomTypeMapper;
        private readonly ContractMapper _contractMapper;

        public ContractService(IUnitOfWork unitOfWork,
                               IUserContextService userContextService,
                               IRoomTypeService roomTypeService,
                               IUserService userService,
                               IHealthInsuranceService healthInsuranceService,
                               IGuardianService guardianService)
        {
            _unitOfWork = unitOfWork;
            _userContextService = userContextService;
            _roomTypeService = roomTypeService;
            _userService = userService;
            _healthInsuranceService = healthInsuranceService;
            _guardianService = guardianService;
            _userMapper = new UserMapper();
            _workplaceMapper = new WorkplaceMapper();
            _roomTypeMapper = new RoomTypeMapper();
            _contractMapper = new ContractMapper();
        }

        public async Task<ApiResponse> Register(RegisterRequestModel model)
        {
            if (model == null)
            {
                return new ApiResponse().SetBadRequest(message: "Model cannot be null");
            }

            // Validate User
            if (model.User == null)
            {
                return new ApiResponse().SetBadRequest(message: "User information is required");
            }

            // Validate startDate cannot be after enddate
            if (model.StartDate > model.EndDate)
            {
                return new ApiResponse().SetUnprocessableEntity(message: "Start date cannot be after end date");
            }

            // Get user gender
            Enum.TryParse<GenderEnum>(model.User.Gender, true, out var gender);
            gender = gender == default ? GenderEnum.OTHER : gender;
            Guid userIdTracking = Guid.Empty;
            Guid healthInsuranceIdTracking = Guid.Empty;
            List<Guid> guardianIdsTracking = [];
            List<Guid> vehicleIdsTracking = [];
            Guid contractIdTracking = Guid.Empty;
            Guid invoiceIdTracking = Guid.Empty;
            UserLoginResponseModel userData = new();

            using (var scope = new TransactionScope(
                TransactionScopeOption.Required,
                new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                TransactionScopeAsyncFlowOption.Enabled))
            {
                //Create User
                var userCreationRequestModel = new UserRequestModel()
                {
                    FirstName = model.User.FirstName,
                    LastName = model.User.LastName,
                    Email = model.User.Email,
                    UserName = model.User.UserName,
                    Password = model.User.Password,
                    DateOfBirth = model.User.DateOfBirth,
                    PhoneNumber = model.User.PhoneNumber,
                    NationalIdNumber = model.User.NationalIdNumber,
                    Gender = model.User.Gender,
                };

                var responseCreateUser = await _userService.SignUp(userCreationRequestModel);
                if (!responseCreateUser.IsSuccess)
                {
                    return responseCreateUser;
                }
                userIdTracking = (Guid)responseCreateUser.Result;
                var userEntity = await _unitOfWork.UserRepository.GetAsync(x => x.Id.Equals(userIdTracking));

                // Check if user have any active contract
                if (userEntity == null)
                {
                    return new ApiResponse().SetNotFound(message: "User not found");
                }

                //Sign in to have userContext
                var responseLogin = await _userService.Login(new LoginRequestModel()
                {
                    Username = userCreationRequestModel.UserName,
                    Password = userCreationRequestModel.Password,
                });

                if (!responseLogin.IsSuccess)
                {
                    throw new EntityNotFoundException("User not found");
                }
                userData = (UserLoginResponseModel)responseLogin.Result;

                // Check if user have any workplace
                if (model.WorkplaceId != null)
                {
                    var workplaceEntity = await _unitOfWork.WorkplaceRepository.GetAsync(x => x.Id == model.WorkplaceId);
                    if (workplaceEntity == null)
                    {
                        return new ApiResponse().SetNotFound(message: "Workplace not found");
                    }

                    userEntity.WorkplaceId = model.WorkplaceId;
                }

                // Add HealthInsurance entity if provided
                if (model.HealthInsurance != null)
                {
                    var responseCreateHealthInsurance = await _healthInsuranceService.AddHealthInsurance(model.HealthInsurance);
                    if (!responseCreateHealthInsurance.IsSuccess)
                    {
                        return responseCreateHealthInsurance;
                    }
                    healthInsuranceIdTracking = (Guid)responseCreateHealthInsurance.Result;
                    userEntity.HealthInsuranceId = healthInsuranceIdTracking;
                }

                // Add Guardians entity if provided
                if (model.Guardians != null && model.Guardians.Count > 0)
                {
                    foreach (var item in model.Guardians)
                    {
                        var guardianCreationRequestModel = item;
                        guardianCreationRequestModel.UserId = userIdTracking;
                        var responseCreateGuardian = await _guardianService.AddNewGuardian(guardianCreationRequestModel);
                        if (!responseCreateGuardian.IsSuccess)
                        {
                            return responseCreateGuardian;
                        }
                        guardianIdsTracking.Add((Guid)responseCreateGuardian.Result);
                    }
                }

                // Add Vehicles
                if (model.Vehicles?.Count > 0)
                {
                    var vehicleEntities = model.Vehicles.Select(x => new VehicleEntity()
                    {
                        UserId = userIdTracking,
                        LicensePlate = x.LicensePlate,
                        VehicleType = x.VehicleType,
                        CreatedBy = userIdTracking,
                        LastUpdatedBy = userIdTracking,
                    }).ToList();

                    await _unitOfWork.VehicleRepository.AddRangeAsync(vehicleEntities);
                    await _unitOfWork.SaveChangeAsync();
                }

                // Create contract
                var contractCreationRequestModel = new ContractRequestModel()
                {
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    RoomId = model.RoomId,
                    UserId = userIdTracking,
                };
                var responseCreateContract = await AddNewContract(contractCreationRequestModel);
                if (!responseCreateContract.IsSuccess)
                {
                    return responseCreateContract;
                }
                contractIdTracking = (Guid)responseCreateContract.Result;

                await _unitOfWork.SaveChangeAsync();

                // Complete transaction
                scope.Complete();
            }

            return new ApiResponse().SetCreated(new RegisterModel
            {
                CcontractId = contractIdTracking,
                User = userData,
            });
        }

        public async Task<ApiResponse> AddNewContract(ContractRequestModel model)
        {
            // Validate startDate cannot be after enddate
            if (model.StartDate > model.EndDate)
            {
                return new ApiResponse().SetUnprocessableEntity(message: "Start date cannot be after end date");
            }

            var userEntity = await _unitOfWork.UserRepository.GetAsync(x => x.Id.Equals(model.UserId), isNoTracking: true);
            if (userEntity == null)
            {
                return new ApiResponse().SetNotFound(model.UserId, message: string.Format(ErrorMessages.PropertyDoesNotExist, "User"));
            }

            var roomEntity = await _unitOfWork.RoomRepository.GetAsync(x => x.Id.Equals(model.RoomId), x => x.Include(x => x.Building));
            if (roomEntity == null)
            {
                return new ApiResponse().SetNotFound(model.RoomId, message: string.Format(ErrorMessages.PropertyDoesNotExist, "Room"));
            }

            if (roomEntity.Building.GenderRestriction == userEntity.Gender)
            {
                return new ApiResponse().SetBadRequest(message: string.Format(ErrorMessages.ConflictGenderWhenChooseRoom, userEntity.Gender.ToString(), roomEntity.Building.GenderRestriction.ToString()));
            }

            if (roomEntity.TotalUsedBed >= roomEntity.TotalAvailableBed)
            {
                return new ApiResponse().SetBadRequest(message: string.Format(ErrorMessages.RoomIsFull));
            }

            var contractEntity = _contractMapper.MapToContractEntity(model);

            contractEntity.CreatedBy = _userContextService.UserId;
            contractEntity.LastUpdatedBy = _userContextService.UserId;

            roomEntity.TotalUsedBed += 1;
            if (roomEntity.TotalUsedBed == roomEntity.TotalAvailableBed)
            {
                roomEntity.Status = RoomStatusEnum.FULL;
            }

            await _unitOfWork.ContractRepository.AddAsync(contractEntity);
            await _unitOfWork.SaveChangeAsync();

            return new ApiResponse().SetCreated(contractEntity.Id);
        }

        public async Task<ApiResponse> UpdateContractStatus(Guid id, ContractStatusEnum status)
        {
            var userId = _userContextService.UserId;
            if (userId == Guid.Empty)
            {
                return new ApiResponse().SetForbidden(message: "User not authenticated");
            }

            ContractEntity? contractEntity;

            if (_userContextService.UserRoles.Contains(Role.ADMIN))
            {
                contractEntity = await _unitOfWork.ContractRepository.GetAsync(x => x.Id == id, x => x.Include(x => x.Room));
            }
            else
            {
                contractEntity = await _unitOfWork.ContractRepository.GetAsync(x => x.Id == id && x.UserId == userId, x => x.Include(x => x.Room));
            }

            if (contractEntity == null)
            {
                return new ApiResponse().SetNotFound(message: "Contract not found");
            }

            contractEntity.Status = status;
            contractEntity.LastUpdatedBy = userId;
            contractEntity.LastUpdatedDateUtc = DateTime.UtcNow;

            if (status == ContractStatusEnum.REJECTED || status == ContractStatusEnum.TERMINATED || status == ContractStatusEnum.EXPIRED)
            {
                // Release bed and update room status
                contractEntity.Room.TotalUsedBed -= 1;
                contractEntity.Room.Status = contractEntity.Room.TotalAvailableBed == contractEntity.Room.TotalUsedBed ? RoomStatusEnum.FULL : RoomStatusEnum.AVAILABLE;
                contractEntity.Room.LastUpdatedBy = userId;
                contractEntity.Room.LastUpdatedDateUtc = DateTime.UtcNow;
            }

            await _unitOfWork.SaveChangeAsync();

            return new ApiResponse().SetOk();
        }

        public async Task<ApiResponse> GetSingleContract(Guid id)
        {
            var contractEntity =
                    await _unitOfWork.ContractRepository
                    .GetAsync(x => x.Id == id, x => x
                        .Include(x => x.Approver)
                        .Include(x => x.User)
                            .ThenInclude(u => u.Guardians)
                        .Include(u => u.User)
                            .ThenInclude(u => u.Vehicles)
                        .Include(u => u.User)
                            .ThenInclude(u => u.HealthInsurance)
                        .Include(u => u.User)
                            .ThenInclude(u => u.Workplace)
                        .Include(x => x.Room)
                            .ThenInclude(r => r.Building)
                        .Include(x => x.Room)
                            .ThenInclude(r => r.RoomType)
                                .ThenInclude(t => t.RoomTypeServices)
                                    .ThenInclude(s => s.RoomService),
                    isNoTracking: true);

            if (contractEntity == null)
            {
                return new ApiResponse().SetNotFound(message: "Contract not found");
            }

            var response = _contractMapper.MapToContractModel(contractEntity);

            return new ApiResponse().SetOk(response);
        }

        public async Task<ApiResponse> GetContractBatch(GetBatchRequestModel model)
        {
            var userId = _userContextService.UserId;
            if (userId == Guid.Empty)
            {
                return new ApiResponse().SetNotFound(message: "User not found");
            }

            var contractEntities = new List<ContractEntity>();

            if (_userContextService.UserRoles.Contains(Role.ADMIN))
            {
                if (model.IsGetAll)
                {
                    contractEntities =
                    await _unitOfWork.ContractRepository
                    .GetAllAsync(x => true, x => x
                        .Include(x => x.Approver)
                        .Include(x => x.User)
                            .ThenInclude(u => u.Guardians)
                        .Include(u => u.User)
                            .ThenInclude(u => u.Vehicles)
                        .Include(u => u.User)
                            .ThenInclude(u => u.HealthInsurance)
                        .Include(u => u.User)
                            .ThenInclude(u => u.Workplace)
                        .Include(x => x.Room)
                            .ThenInclude(r => r.Building)
                        .Include(x => x.Room)
                            .ThenInclude(r => r.RoomType)
                                .ThenInclude(t => t.RoomTypeServices)
                                    .ThenInclude(s => s.RoomService),
                    isNoTracking: true);
                }
                else
                {
                    contractEntities =
                    await _unitOfWork.ContractRepository
                    .GetAllAsync(x => model.Ids.Contains(x.Id), x => x
                        .Include(x => x.Approver)
                        .Include(x => x.User)
                            .ThenInclude(u => u.Guardians)
                        .Include(u => u.User)
                            .ThenInclude(u => u.Vehicles)
                        .Include(u => u.User)
                            .ThenInclude(u => u.HealthInsurance)
                        .Include(u => u.User)
                            .ThenInclude(u => u.Workplace)
                        .Include(x => x.Room)
                            .ThenInclude(r => r.Building)
                        .Include(x => x.Room)
                            .ThenInclude(r => r.RoomType)
                                .ThenInclude(t => t.RoomTypeServices)
                                    .ThenInclude(s => s.RoomService),
                    isNoTracking: true);
                }
            }
            else
            {
                if (model.IsGetAll)
                {
                    contractEntities =
                    await _unitOfWork.ContractRepository
                    .GetAllAsync(x => x.UserId == userId, x => x
                        .Include(x => x.Approver)
                        .Include(x => x.User)
                            .ThenInclude(u => u.Guardians)
                        .Include(u => u.User)
                            .ThenInclude(u => u.Vehicles)
                        .Include(u => u.User)
                            .ThenInclude(u => u.HealthInsurance)
                        .Include(u => u.User)
                            .ThenInclude(u => u.Workplace)
                        .Include(x => x.Room)
                            .ThenInclude(r => r.Building)
                        .Include(x => x.Room)
                            .ThenInclude(r => r.RoomType)
                                .ThenInclude(t => t.RoomTypeServices)
                                    .ThenInclude(s => s.RoomService),
                    isNoTracking: true);
                }
                else
                {
                    contractEntities =
                    await _unitOfWork.ContractRepository
                    .GetAllAsync(x => x.UserId == userId && model.Ids.Contains(x.Id), x => x
                        .Include(x => x.Approver)
                        .Include(x => x.User)
                            .ThenInclude(u => u.Guardians)
                        .Include(u => u.User)
                            .ThenInclude(u => u.Vehicles)
                        .Include(u => u.User)
                            .ThenInclude(u => u.HealthInsurance)
                        .Include(u => u.User)
                            .ThenInclude(u => u.Workplace)
                        .Include(x => x.Room)
                            .ThenInclude(r => r.Building)
                        .Include(x => x.Room)
                            .ThenInclude(r => r.RoomType)
                                .ThenInclude(t => t.RoomTypeServices)
                                    .ThenInclude(s => s.RoomService),
                    isNoTracking: true);
                }
            }

            if (!model.IsGetAll)
            {
                if (contractEntities.Count != model.Ids.Count)
                {
                    // Find the missing request IDs
                    var foundRequestIds = contractEntities.Select(r => r.Id).ToList();
                    var missingRequestIds = model.Ids.Except(foundRequestIds).ToList();

                    // Return with error message listing the missing request IDs
                    var errorMessage = $"Contract(s) not found: {string.Join(", ", missingRequestIds)}";
                    return new ApiResponse().SetNotFound(message: errorMessage);
                }
            }

            var response = contractEntities.Select(x => _contractMapper.MapToContractModel(x)).ToList();

            return new ApiResponse().SetOk(response);
        }
    }
}
