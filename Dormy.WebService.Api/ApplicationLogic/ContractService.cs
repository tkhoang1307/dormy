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
        private readonly WorkplaceMapper _workplaceMapper;
        private readonly RoomTypeMapper _roomTypeMapper;
        private readonly ContractMapper _contractMapper;

        public ContractService(IUnitOfWork unitOfWork, IUserContextService userContextService, IRoomTypeService roomTypeService)
        {
            _unitOfWork = unitOfWork;
            _userContextService = userContextService;
            _roomTypeService = roomTypeService;
            _userMapper = new UserMapper();
            _workplaceMapper = new WorkplaceMapper();
            _roomTypeMapper = new RoomTypeMapper();
            _contractMapper = new ContractMapper();
        }

        public async Task<ApiResponse> GetRegistrationByRequestId(Guid requestId, Guid? userId)
        {
            if (_userContextService.UserRoles.FirstOrDefault() == Role.USER)
            {
                userId = _userContextService.UserId;
            }

            var userEntity = await _unitOfWork.UserRepository.GetAsync(
                x => x.Id == userId,
                x => x.Include(x => x.HealthInsurance)
                        .Include(x => x.Guardians)
                        .Include(x => x.Vehicles));
            if (userEntity == null)
            {
                return new ApiResponse().SetNotFound(message: "User not found");
            }

            var result = await GetRegistrationByRequestId(requestId, userEntity);

            return new ApiResponse().SetOk(result);
        }
        public async Task<ApiResponse> GetRegistrationBatchByRequestIds(List<Guid> requestIds, Guid? userId)
        {
            if (_userContextService.UserRoles.FirstOrDefault() == Role.USER)
            {
                userId = _userContextService.UserId;
            }

            var userEntity = await _unitOfWork.UserRepository.GetAsync(
                x => x.Id == userId,
                x => x.Include(x => x.HealthInsurance)
                        .Include(x => x.Guardians)
                        .Include(x => x.Vehicles));
            if (userEntity == null)
            {
                return new ApiResponse().SetNotFound(message: "User not found");
            }

            var requests = await _unitOfWork.RequestRepository.GetAllAsync(x => requestIds.Contains(x.Id), isNoTracking: true);
            if (requests.Count != requestIds.Count)
            {
                // Find the missing request IDs
                var foundRequestIds = requests.Select(r => r.Id).ToList();
                var missingRequestIds = requestIds.Except(foundRequestIds).ToList();

                // Return with error message listing the missing request IDs
                var errorMessage = $"Request(s) not found: {string.Join(", ", missingRequestIds)}";
                return new ApiResponse().SetNotFound(message: errorMessage);
            }

            var result = new List<RegistrationResponseModel>();

            for (int i = 0; i < requestIds.Count; i++)
            {
                var requestId = requestIds[i];
                var registration = await GetRegistrationByRequestId(requestId, userEntity);
                result.Add(registration);
            }

            return new ApiResponse().SetOk(result);
        }
        public async Task<ApiResponse> Register(RegisterRequestModel model)
        {
            if (model == null)
            {
                return new ApiResponse().SetBadRequest(null, "Model cannot be null");
            }

            var userId = _userContextService.UserId;

            if (userId == Guid.Empty)
            {
                return new ApiResponse().SetForbidden(null, "User not authenticated");
            }

            // Validate User
            if (model.User == null)
            {
                return new ApiResponse().SetBadRequest(null, "User information is required");
            }

            // Validate startDate cannot be after enddate
            if (model.StartDate > model.EndDate)
            {
                return new ApiResponse().SetUnprocessableEntity(null, "Start date cannot be after end date");
            }

            // Retrieve existing User entity
            var userEntity = await _unitOfWork.UserRepository.GetAsync(
                x => x.Id == userId,
                x => x.Include(x => x.HealthInsurance)
                        .Include(x => x.Guardians)
                        .Include(x => x.Vehicles)
                        .Include(x => x.Contracts)
                        .Include(x => x.Requests));

            if (userEntity == null)
            {
                return new ApiResponse().SetNotFound(message: "User not found");
            }

            if (IsActiveContractExisted(userEntity))
            {
                return new ApiResponse().SetConflict(message: "User is still have an available contract");
            }

            // Get user gender
            Enum.TryParse<GenderEnum>(model.User.Gender, true, out var gender);
            gender = gender == default ? GenderEnum.OTHER : gender;

            using (var scope = new TransactionScope(
                TransactionScopeOption.Required,
                new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                TransactionScopeAsyncFlowOption.Enabled))
            {
                // Check if there is any available room for user gender
                var roomAvailables = await _unitOfWork.RoomRepository
                    .GetAllAsync(x =>
                        x.RoomTypeId == model.RoomTypeId &&
                        x.Status == RoomStatusEnum.AVAILABLE &&
                        x.TotalAvailableBed > 0 &&
                        x.Building.GenderRestriction != gender,
                        x => x.Include(x => x.RoomType).Include(x => x.Building));

                if (roomAvailables.Count == 0)
                {
                    return new ApiResponse().SetUnprocessableEntity(message: "Out of available room");
                }

                // Get the first available room
                var bookedRoom = await _unitOfWork.RoomRepository.GetAsync(x => x.Id == roomAvailables.First().Id);

                if (bookedRoom == null)
                {
                    return new ApiResponse().SetNotFound(null, "Room not found");
                }

                // Update room
                bookedRoom.Status = bookedRoom.TotalAvailableBed == bookedRoom.TotalUsedBed ? RoomStatusEnum.FULL : RoomStatusEnum.AVAILABLE;
                bookedRoom.LastUpdatedBy = userId;
                bookedRoom.LastUpdatedDateUtc = DateTime.UtcNow;
                bookedRoom.TotalAvailableBed = bookedRoom.TotalAvailableBed - 1;
                bookedRoom.TotalUsedBed = bookedRoom.TotalUsedBed + 1;
                await _unitOfWork.SaveChangeAsync();

                // Check if user have any workplace
                if (model.WorkplaceId != null)
                {
                    var workplaceEntity = await _unitOfWork.WorkplaceRepository.GetAsync(x => x.Id == model.WorkplaceId);
                    if (workplaceEntity == null)
                    {
                        return new ApiResponse().SetNotFound(message: "Workplace not found");
                    }
                }

                // Update User entity
                userEntity.FirstName = model.User.FirstName;
                userEntity.LastName = model.User.LastName;
                userEntity.Email = model.User.Email;
                userEntity.DateOfBirth = model.User.DateOfBirth;
                userEntity.PhoneNumber = model.User.PhoneNumber;
                userEntity.NationalIdNumber = model.User.NationalIdNumber;
                userEntity.WorkplaceId = model.WorkplaceId;
                userEntity.Gender = gender;
                userEntity.LastUpdatedBy = userId;
                userEntity.LastUpdatedDateUtc = DateTime.UtcNow;

                await _unitOfWork.SaveChangeAsync();

                // Add HealthInsurance entity if provided
                if (model.HealthInsurance != null)
                {
                    var healthInsuranceEntity = new HealthInsuranceEntity
                    {
                        InsuranceCardNumber = model.HealthInsurance.InsuranceCardNumber,
                        RegisteredHospital = model.HealthInsurance.RegisteredHospital,
                        ExpirationDate = model.HealthInsurance.ExpirationDate,
                        CreatedBy = userId,
                        CreatedDateUtc = DateTime.UtcNow,
                    };
                    await _unitOfWork.HealthInsuranceRepository.AddAsync(healthInsuranceEntity);
                    await _unitOfWork.SaveChangeAsync();

                    userEntity.HealthInsuranceId = healthInsuranceEntity.Id;
                }

                // Add Guardians entity if provided
                if (model.Guardians != null && model.Guardians.Count > 0)
                {
                    var guardianEntities = model.Guardians.Select(guardian => new GuardianEntity
                    {
                        Name = guardian.Name,
                        Email = guardian.Email,
                        PhoneNumber = guardian.PhoneNumber,
                        Address = guardian.Address,
                        RelationshipToUser = guardian.RelationshipToUser,
                        UserId = userId,
                        CreatedBy = userId,
                        CreatedDateUtc = DateTime.UtcNow,
                    }).ToList();

                    await _unitOfWork.GuardianRepository.AddRangeAsync(guardianEntities);
                    await _unitOfWork.SaveChangeAsync();
                }

                // Create and add Vehicle entities if provided
                if (model.Vehicles != null && model.Vehicles.Count > 0)
                {
                    var vehicleEntities = model.Vehicles.Select(vehicle => new VehicleEntity
                    {
                        LicensePlate = vehicle.LicensePlate,
                        VehicleType = vehicle.VehicleType,
                        UserId = userId,
                        CreatedBy = userId,
                        CreatedDateUtc = DateTime.UtcNow,
                    }).ToList();

                    await _unitOfWork.VehicleRepository.AddRangeAsync(vehicleEntities);
                    await _unitOfWork.SaveChangeAsync();
                }

                // Create Request entity
                var request = new RequestEntity()
                {
                    UserId = userId,
                    RoomId = bookedRoom.Id,
                    RequestType = RequestTypeEnum.REGISTRATION.ToString(),
                    Status = RequestStatusEnum.SUBMITTED,
                    CreatedBy = userId,
                    CreatedDateUtc = DateTime.Now,
                    Description = "User registration request",
                    Contract = new ContractEntity()
                    {
                        UserId = userId,
                        RoomId = bookedRoom.Id,
                        StartDate = model.StartDate,
                        EndDate = model.EndDate,
                        SubmissionDate = DateTime.Now,
                        Status = ContractStatusEnum.PENDING,
                        CreatedBy = userId,
                        CreatedDateUtc = DateTime.Now,
                    },
                };

                await _unitOfWork.RequestRepository.AddAsync(request);
                await _unitOfWork.SaveChangeAsync();

                // Complete transaction
                scope.Complete();
            }
            return new ApiResponse().SetOk();
        }
        public async Task<ApiResponse> UpdateRegistration(Guid requestId, RegisterUpdateRequestModel model)
        {
            if (model == null)
            {
                return new ApiResponse().SetBadRequest(null, "Model cannot be null");
            }

            var userId = _userContextService.UserId;

            if (userId == Guid.Empty)
            {
                return new ApiResponse().SetForbidden(null, "User not authenticated");
            }

            var requestEntity = await _unitOfWork.RequestRepository.GetAsync(x => x.Id == requestId && x.UserId == userId, x => x.Include(x => x.Contract), isNoTracking: true);
            if (requestEntity == null || requestEntity.IsDeleted)
            {
                return new ApiResponse().SetNotFound(message: "Request not found");
            }

            if (requestEntity.Status == RequestStatusEnum.APPROVED)
            {
                return new ApiResponse().SetConflict(message: "Request is already approved");
            }

            if (requestEntity.Contract?.Status != ContractStatusEnum.REJECTED &&
                requestEntity.Contract?.Status != ContractStatusEnum.TERMINATED &&
                requestEntity.Contract?.Status != ContractStatusEnum.PENDING)
            {
                return new ApiResponse().SetConflict(message: "Contract is already active");
            }

            // Validate User
            if (model.User == null)
            {
                return new ApiResponse().SetBadRequest(null, "User information is required");
            }

            // Validate startDate cannot be after enddate
            if (model.StartDate > model.EndDate)
            {
                return new ApiResponse().SetUnprocessableEntity(null, "Start date cannot be after end date");
            }

            // Retrieve existing User entity
            var userEntity = await _unitOfWork.UserRepository.GetAsync(
                x => x.Id == userId,
                x => x.Include(x => x.HealthInsurance)
                        .Include(x => x.Guardians)
                        .Include(x => x.Vehicles)
                        .Include(x => x.Contracts)
                        .Include(x => x.Requests));

            if (userEntity == null)
            {
                return new ApiResponse().SetNotFound(message: "User not found");
            }

            // Get user gender
            Enum.TryParse<GenderEnum>(model.User.Gender, true, out var gender);
            gender = gender == default ? GenderEnum.OTHER : gender;

            using (var scope = new TransactionScope(
                TransactionScopeOption.Required,
                new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                TransactionScopeAsyncFlowOption.Enabled))
            {
                // Check if there is any available room for user gender
                var roomAvailables = await _unitOfWork.RoomRepository
                    .GetAllAsync(x =>
                        x.RoomTypeId == model.RoomTypeId &&
                        x.Status == RoomStatusEnum.AVAILABLE &&
                        x.TotalAvailableBed > 0 &&
                        x.Building.GenderRestriction != gender,
                        x => x.Include(x => x.RoomType).Include(x => x.Building),
                        isNoTracking: true);

                if (roomAvailables.Count == 0)
                {
                    return new ApiResponse().SetUnprocessableEntity(message: "Out of available room");
                }

                // Get the first available room
                var bookedRoom = await _unitOfWork.RoomRepository.GetAsync(x => x.Id == roomAvailables.First().Id);

                if (bookedRoom == null)
                {
                    return new ApiResponse().SetNotFound(null, "Room not found");
                }

                // Update room
                bookedRoom.Status = bookedRoom.TotalAvailableBed == bookedRoom.TotalUsedBed ? RoomStatusEnum.FULL : RoomStatusEnum.AVAILABLE;
                bookedRoom.LastUpdatedBy = userId;
                bookedRoom.LastUpdatedDateUtc = DateTime.UtcNow;
                bookedRoom.TotalAvailableBed = bookedRoom.TotalAvailableBed - 1;
                bookedRoom.TotalUsedBed = bookedRoom.TotalUsedBed + 1;
                await _unitOfWork.SaveChangeAsync();

                // Reset current booked room available bed
                var currentBookedRoom = await _unitOfWork.RoomRepository.GetAsync(x => x.Id == requestEntity.RoomId);
                if (currentBookedRoom == null)
                {
                    return new ApiResponse().SetNotFound(null, "Current booked room not found");
                }
                currentBookedRoom.TotalAvailableBed = currentBookedRoom.TotalAvailableBed + 1;
                currentBookedRoom.TotalUsedBed = currentBookedRoom.TotalUsedBed - 1;
                currentBookedRoom.Status = currentBookedRoom.TotalAvailableBed == currentBookedRoom.TotalUsedBed ? RoomStatusEnum.FULL : RoomStatusEnum.AVAILABLE;
                currentBookedRoom.LastUpdatedBy = userId;
                currentBookedRoom.LastUpdatedDateUtc = DateTime.UtcNow;
                await _unitOfWork.SaveChangeAsync();

                // Check if user have any workplace
                if (model.WorkplaceId != null)
                {
                    var workplaceEntity = await _unitOfWork.WorkplaceRepository.GetAsync(x => x.Id == model.WorkplaceId);
                    if (workplaceEntity == null)
                    {
                        return new ApiResponse().SetNotFound(message: "Workplace not found");
                    }
                }

                // Update User entity
                userEntity.FirstName = model.User.FirstName;
                userEntity.LastName = model.User.LastName;
                userEntity.Email = model.User.Email;
                userEntity.DateOfBirth = model.User.DateOfBirth;
                userEntity.PhoneNumber = model.User.PhoneNumber;
                userEntity.NationalIdNumber = model.User.NationalIdNumber;
                userEntity.WorkplaceId = model.WorkplaceId;
                userEntity.Gender = gender;
                userEntity.LastUpdatedBy = userId;
                userEntity.LastUpdatedDateUtc = DateTime.UtcNow;

                // Update HealthInsurance entity if provided
                if (model.HealthInsurance != null)
                {
                    if (userEntity.HealthInsurance == null)
                    {
                        userEntity.HealthInsurance = new HealthInsuranceEntity
                        {
                            InsuranceCardNumber = model.HealthInsurance.InsuranceCardNumber,
                            RegisteredHospital = model.HealthInsurance.RegisteredHospital,
                            ExpirationDate = model.HealthInsurance.ExpirationDate,
                            CreatedBy = userId,
                            CreatedDateUtc = DateTime.UtcNow,
                        };
                    }
                    else
                    {
                        userEntity.HealthInsurance.InsuranceCardNumber = model.HealthInsurance.InsuranceCardNumber;
                        userEntity.HealthInsurance.RegisteredHospital = model.HealthInsurance.RegisteredHospital;
                        userEntity.HealthInsurance.ExpirationDate = model.HealthInsurance.ExpirationDate;
                        userEntity.HealthInsurance.LastUpdatedBy = userId;
                        userEntity.HealthInsurance.LastUpdatedDateUtc = DateTime.UtcNow;
                    }
                }

                // Add Guardians entity if provided
                if (model.Guardians != null && model.Guardians.Count > 0)
                {
                    userEntity.Guardians = [];
                    var guardianEntities = model.Guardians.Select(guardian => new GuardianEntity
                    {
                        Name = guardian.Name,
                        Email = guardian.Email,
                        PhoneNumber = guardian.PhoneNumber,
                        Address = guardian.Address,
                        RelationshipToUser = guardian.RelationshipToUser,
                        UserId = userId,
                        CreatedBy = userId,
                        CreatedDateUtc = DateTime.UtcNow,
                    }).ToList();

                    userEntity.Guardians.AddRange(guardianEntities);
                }

                // Create and add Vehicle entities if provided
                if (model.Vehicles != null && model.Vehicles.Count > 0)
                {
                    userEntity.Vehicles = [];
                    var vehicleEntities = model.Vehicles.Select(vehicle => new VehicleEntity
                    {
                        LicensePlate = vehicle.LicensePlate,
                        VehicleType = vehicle.VehicleType,
                        UserId = userId,
                        CreatedBy = userId,
                        CreatedDateUtc = DateTime.UtcNow,
                    }).ToList();

                    userEntity.Vehicles.AddRange(vehicleEntities);
                }

                // Update Request and contract entity
                requestEntity.LastUpdatedBy = userId;
                requestEntity.LastUpdatedDateUtc = DateTime.Now;
                requestEntity.Description = "User update registration request";
                requestEntity.Contract.StartDate = model.StartDate;
                requestEntity.Contract.EndDate = model.EndDate;
                requestEntity.Contract.LastUpdatedBy = userId;
                requestEntity.Contract.LastUpdatedDateUtc = DateTime.Now;

                await _unitOfWork.SaveChangeAsync();

                // Complete transaction
                scope.Complete();

                return new ApiResponse().SetOk();
            }
        }
        public async Task<ApiResponse> UpdateContractStatus(Guid id, ContractStatusEnum status)
        {
            var userId = _userContextService.UserId;
            if (userId == Guid.Empty)
            {
                return new ApiResponse().SetForbidden(null, "User not authenticated");
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
                contractEntity.Room.TotalAvailableBed += 1;
                contractEntity.Room.TotalUsedBed -= 1;
                contractEntity.Room.Status = contractEntity.Room.TotalAvailableBed == contractEntity.Room.TotalUsedBed ? RoomStatusEnum.FULL : RoomStatusEnum.AVAILABLE;
                contractEntity.Room.LastUpdatedBy = userId;
                contractEntity.Room.LastUpdatedDateUtc = DateTime.UtcNow;
            }

            await _unitOfWork.SaveChangeAsync();

            return new ApiResponse().SetOk();
        }
        private bool IsActiveContractExisted(UserEntity user)
        {
            if (user == null || user.Contracts == null || user.Contracts.Count == 0)
            {
                return false;
            }
            // Check user have available contract
            var isUserHaveActiveContract = user.Contracts.Any(contract =>
                contract.EndDate >= DateTime.UtcNow &&
                (contract.Status == ContractStatusEnum.ACTIVE ||
                contract.Status == ContractStatusEnum.EXTENDED ||
                contract.Status == ContractStatusEnum.PENDING ||
                contract.Status == ContractStatusEnum.WAITING_PAYMENT));

            return isUserHaveActiveContract;
        }
        private async Task<RegistrationResponseModel> GetRegistrationByRequestId(Guid requestId, UserEntity userEntity)
        {
            var result = new RegistrationResponseModel();
            var requestEntity =
                await _unitOfWork.RequestRepository.GetAsync(
                    x => x.Id == requestId,
                    x => x.Include(x => x.Contract)
                    .Include(x => x.Room)
                    .ThenInclude(r => r.RoomType));
            if (requestEntity == null)
            {
                throw new EntityNotFoundException("Request not found");
            }

            // Map Request
            result.RequestId = requestEntity.Id;
            result.RequestType = requestEntity.RequestType;
            result.Status = requestEntity.Status.ToString();
            result.ContractId = requestEntity.ContractId;

            if (requestEntity.Contract == null)
            {
                throw new EntityNotFoundException("Contract not found");
            }

            // Map Contract Date
            result.StartDate = requestEntity.Contract.StartDate;
            result.EndDate = requestEntity.Contract.EndDate;

            // Map User
            result.User = _userMapper.MapToUserRegistrationResponseModel(userEntity);

            // Map Workplace
            if (userEntity.Workplace != null)
            {
                result.Workplace = _workplaceMapper.MapToWorkplaceRegistrationResponseModel(userEntity.Workplace);
            }

            // Map Room
            var roomTypeData = (await _roomTypeService.GetRoomTypeById(requestEntity.Room.RoomType.Id)).Result as RoomTypeResponseModel;
            if (roomTypeData == null || roomTypeData.GetType() != typeof(RoomTypeResponseModel))
            {
                throw new EntityNotFoundException("RoomType not found");
            }
            result.RoomType = _roomTypeMapper.MapToRoomTypeRegistrationResponseModel(roomTypeData);

            // Map HealthInsurance
            if (userEntity.HealthInsurance != null)
            {
                result.HealthInsurance = new HealthInsuranceRegistrationResponseModel
                {
                    Id = userEntity.HealthInsurance.Id,
                    InsuranceCardNumber = userEntity.HealthInsurance.InsuranceCardNumber,
                    RegisteredHospital = userEntity.HealthInsurance.RegisteredHospital,
                    ExpirationDate = userEntity.HealthInsurance.ExpirationDate,
                };
            }

            // Map Guardians
            if (userEntity.Guardians != null && userEntity.Guardians.Count > 0)
            {
                result.Guardians = userEntity.Guardians.Select(guardian => new GuardianResponseRegistrationModel
                {
                    Id = guardian.Id,
                    Name = guardian.Name,
                    Email = guardian.Email,
                    PhoneNumber = guardian.PhoneNumber,
                    Address = guardian.Address,
                    RelationshipToUser = guardian.RelationshipToUser,
                }).ToList();
            }

            // Map Vehicles
            if (userEntity.Vehicles != null && userEntity.Vehicles.Count > 0)
            {
                result.Vehicles = userEntity.Vehicles.Select(vehicle => new VehicleResponseRegistrationModel
                {
                    Id = vehicle.Id,
                    LicensePlate = vehicle.LicensePlate,
                    VehicleType = vehicle.VehicleType,
                }).ToList();
            }

            return result;
        }

        public async Task<ApiResponse> GetSingleContract(Guid id)
        {
            var userId = _userContextService.UserId;
            if (userId == Guid.Empty)
            {
                return new ApiResponse().SetNotFound("User not found");
            }

            ContractEntity? contractEntity;

            if (_userContextService.UserRoles.Contains(Role.ADMIN))
            {
                contractEntity = await _unitOfWork.ContractRepository.GetAsync(x => x.Id == id, x => x.Include(x => x.Approver).Include(x => x.User).Include(x => x.Room));
            }
            else
            {
                contractEntity = await _unitOfWork.ContractRepository.GetAsync(x => x.Id == id && x.UserId == userId, x => x.Include(x => x.Approver).Include(x => x.User).Include(x => x.Room));
            }

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
                return new ApiResponse().SetNotFound("User not found");
            }

            var contractEntities = new List<ContractEntity>();

            if (_userContextService.UserRoles.Contains(Role.ADMIN))
            {
                if (model.IsGetAll)
                {
                    contractEntities = await _unitOfWork.ContractRepository.GetAllAsync(x => true, x => x.Include(x => x.Approver).Include(x => x.User).Include(x => x.Room));
                }
                else
                {
                    contractEntities = await _unitOfWork.ContractRepository.GetAllAsync(x => model.Ids.Contains(x.Id), x => x.Include(x => x.Approver).Include(x => x.User).Include(x => x.Room));
                }
            }
            else
            {
                if (model.IsGetAll)
                {
                    contractEntities = await _unitOfWork.ContractRepository.GetAllAsync(x => x.UserId == userId, x => x.Include(x => x.Approver).Include(x => x.User).Include(x => x.Room));
                }
                else
                {
                    contractEntities = await _unitOfWork.ContractRepository.GetAllAsync(x => model.Ids.Contains(x.Id) && x.UserId == userId, x => x.Include(x => x.Approver).Include(x => x.User).Include(x => x.Room));
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
                    var errorMessage = $"Request(s) not found: {string.Join(", ", missingRequestIds)}";
                    return new ApiResponse().SetNotFound(message: errorMessage);
                }
            }

            var response = contractEntities.Select(x => _contractMapper.MapToContractModel(x)).ToList();

            return new ApiResponse().SetOk(response);
        }
    }
}
