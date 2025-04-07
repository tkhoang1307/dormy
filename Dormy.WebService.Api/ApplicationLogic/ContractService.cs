using Dormy.WebService.Api.Core.Constants;
using Dormy.WebService.Api.Core.CustomExceptions;
using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Core.Interfaces;
using Dormy.WebService.Api.Core.Utilities;
using Dormy.WebService.Api.Models.Constants;
using Dormy.WebService.Api.Models.Enums;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;
using Dormy.WebService.Api.Presentation.Mappers;
using Dormy.WebService.Api.Presentation.Validations;
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
        private readonly IInvoiceService _invoiceService;
        private readonly IVehicleService _vehicleService;

        public ContractService(IUnitOfWork unitOfWork,
                               IUserContextService userContextService,
                               IRoomTypeService roomTypeService,
                               IUserService userService,
                               IHealthInsuranceService healthInsuranceService,
                               IGuardianService guardianService,
                               IInvoiceService invoiceService,
                               IVehicleService vehicleService)
        {
            _unitOfWork = unitOfWork;
            _userContextService = userContextService;
            _roomTypeService = roomTypeService;
            _userService = userService;
            _healthInsuranceService = healthInsuranceService;
            _guardianService = guardianService;
            _invoiceService = invoiceService;
            _vehicleService = vehicleService;
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

            // Validate startDate and endDate
            if (model.StartDate.Date < DateTime.Now.Date)
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.DateMustNotBeInThePast, nameof(model.StartDate)));
            }
            if (model.EndDate.Date < DateTime.Now.Date)
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.DateMustNotBeInThePast, nameof(model.EndDate)));
            }
            if (!DateTimeHelper.AreValidStartDateEndDateWithoutTime(model.StartDate, model.EndDate, false))
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.StartDateMustBeLessThanEndDate));
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

                // Check if user have any workplace
                if (model.WorkplaceId != null)
                {
                    var workplaceEntity = await _unitOfWork.WorkplaceRepository.GetAsync(x => x.Id == model.WorkplaceId);
                    if (workplaceEntity == null)
                    {
                        return new ApiResponse().SetNotFound(model.WorkplaceId, message: string.Format(ErrorMessages.PropertyDoesNotExist, "Workplace"));
                    }

                    userEntity.WorkplaceId = model.WorkplaceId;
                }

                // Add HealthInsurance entity if provided
                if (model.HealthInsurance != null)
                {
                    var healthInsuranceCreationRequestModel = model.HealthInsurance;
                    healthInsuranceCreationRequestModel.UserId = userIdTracking;
                    var responseCreateHealthInsurance = await _healthInsuranceService.AddHealthInsurance(healthInsuranceCreationRequestModel);
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

                // Add Vehicles entity if provided
                if (model.Vehicles != null && model.Vehicles.Count > 0)
                {
                    foreach (var item in model.Vehicles)
                    {
                        var vehicleCreationRequestModel = item;
                        vehicleCreationRequestModel.UserId = userIdTracking;
                        var responseCreateVehicle = await _vehicleService.AddNewVehicle(vehicleCreationRequestModel);
                        if (!responseCreateVehicle.IsSuccess)
                        {
                            return responseCreateVehicle;
                        }
                        guardianIdsTracking.Add((Guid)responseCreateVehicle.Result);
                    }
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

            return new ApiResponse().SetCreated(new RegistrationResponseModel
            {
                ContractId = contractIdTracking,
                UserId = userIdTracking,
                HealthInsuranceId = healthInsuranceIdTracking,
                GuardianIds = guardianIdsTracking,
                VehicleIds = vehicleIdsTracking,
            });
        }

        public async Task<ApiResponse> AddNewContract(ContractRequestModel model)
        {
            // Validate startDate and endDate
            if (model.StartDate.Date < DateTime.Now.Date)
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.DateMustNotBeInThePast, nameof(model.StartDate)));
            }
            if (model.EndDate.Date < DateTime.Now.Date)
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.DateMustNotBeInThePast, nameof(model.EndDate)));
            }
            if (!DateTimeHelper.AreValidStartDateEndDateWithoutTime(model.StartDate, model.EndDate, false))
            {
                return new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.StartDateMustBeLessThanEndDate));
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

            if (roomEntity.Building.GenderRestriction != userEntity.Gender)
            {
                return new ApiResponse().SetBadRequest(message: string.Format(ErrorMessages.ConflictGenderWhenChooseRoom, userEntity.Gender.ToString(), roomEntity.Building.GenderRestriction.ToString()));
            }

            if (roomEntity.TotalUsedBed >= roomEntity.TotalAvailableBed)
            {
                return new ApiResponse().SetBadRequest(message: string.Format(ErrorMessages.RoomIsFull));
            }

            var contractEntity = _contractMapper.MapToContractEntity(model);

            contractEntity.CreatedBy = model?.UserId ?? _userContextService.UserId;
            contractEntity.LastUpdatedBy = model?.UserId ?? _userContextService.UserId;

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
                return new ApiResponse().SetNotFound(id, message: string.Format(ErrorMessages.PropertyDoesNotExist, "Contract"));
            }

            var (isError, errorMessage) = ContractStatusChangeValidator.VerifyContractStatusChangeValidator(contractEntity.Status, status);
            if (isError)
            {
                return new ApiResponse().SetConflict(id, message: string.Format(errorMessage, "Contract"));
            }

            Guid invoiceIdTracking = Guid.Empty;
            using (var scope = new TransactionScope(
                TransactionScopeOption.Required,
                new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                TransactionScopeAsyncFlowOption.Enabled))
            {
                switch (status)
                {
                    case ContractStatusEnum.WAITING_PAYMENT:
                        var roomTypeId = (await _unitOfWork.RoomRepository.GetAsync(x => x.Id == contractEntity.RoomId)).RoomTypeId;
                        var roomTypeServices = await _unitOfWork.RoomTypeServiceRepository.GetAllAsync(x => x.RoomTypeId == roomTypeId, x => x.Include(x => x.RoomService));
                        var roomServiceIdRentalPayment = roomTypeServices.Where(x => x.RoomService.RoomServiceType == RoomServiceTypeEnum.RENTAL_PAYMENT)
                                                                         .Select(x => x.RoomServiceId).FirstOrDefault();
                        var responseCreateInvoice = await _invoiceService.CreateNewInvoice(new InvoiceRequestModel()
                        {
                            DueDate = DateTime.Now.AddDays(15),
                            Type = InvoiceTypeEnum.PAYMENT_CONTRACT.ToString(),
                            RoomId = contractEntity.RoomId,
                            ContractId = contractEntity.Id,
                            InvoiceItems = new List<InvoiceItemRequestModel>()
                        {
                            new InvoiceItemRequestModel()
                            {
                                RoomServiceId = roomServiceIdRentalPayment,
                                Quantity = 1
                            }
                        }
                        });

                        if (!responseCreateInvoice.IsSuccess)
                        {
                            return responseCreateInvoice;
                        }

                        invoiceIdTracking = (Guid)responseCreateInvoice.Result;
                        contractEntity.InvoiceId = invoiceIdTracking;
                        contractEntity.ApproverId = userId;
                        break;
                    case ContractStatusEnum.ACTIVE:
                        break;
                    case ContractStatusEnum.EXTENDED:
                        break;
                    case ContractStatusEnum.EXPIRED:
                        break;
                    case ContractStatusEnum.TERMINATED:
                    case ContractStatusEnum.REJECTED:
                        contractEntity.Room.TotalUsedBed = contractEntity.Room.TotalUsedBed > 0 ? contractEntity.Room.TotalUsedBed - 1 : 0;
                        contractEntity.Room.Status = contractEntity.Room.TotalAvailableBed == contractEntity.Room.TotalUsedBed ? RoomStatusEnum.FULL : RoomStatusEnum.AVAILABLE;
                        contractEntity.Room.LastUpdatedBy = userId;
                        contractEntity.Room.LastUpdatedDateUtc = DateTime.UtcNow;
                        break;
                }

                contractEntity.Status = status;
                contractEntity.LastUpdatedBy = userId;
                contractEntity.LastUpdatedDateUtc = DateTime.UtcNow;

                await _unitOfWork.SaveChangeAsync();

                // Complete transaction
                scope.Complete();
            }

            return new ApiResponse().SetOk(id);
        }

        public async Task<ApiResponse> GetSingleContract(Guid id)
        {
            var contractEntity =
                    await _unitOfWork.ContractRepository
                    .GetAsync(x => x.Id == id, x => x
                        .Include(x => x.Approver)
                        .Include(u => u.User)
                            .ThenInclude(u => u.HealthInsurance)
                        .Include(u => u.User)
                            .ThenInclude(u => u.Workplace)
                        .Include(x => x.Room)
                            .ThenInclude(r => r.Building)
                        .Include(x => x.Room)
                            .ThenInclude(r => r.RoomType),
                    isNoTracking: true);

            if (contractEntity == null)
            {
                return new ApiResponse().SetNotFound(id, message: string.Format(ErrorMessages.PropertyDoesNotExist, "Contract"));
            }

            if (_userContextService.UserRoles.Contains(Role.USER) && contractEntity.UserId != _userContextService.UserId)
            {
                return new ApiResponse().SetForbidden(contractEntity.Id, message: string.Format(ErrorMessages.AccountDoesNotHavePermissionEntity, "contract"));
            }

            var response = _contractMapper.MapToContractModel(contractEntity);

            return new ApiResponse().SetOk(response);
        }

        public async Task<ApiResponse> GetContractBatch(GetBatchRequestModel model)
        {
            var userId = _userContextService.UserId;
            if (userId == Guid.Empty)
            {
                return new ApiResponse().SetNotFound(userId, message: string.Format(ErrorMessages.PropertyDoesNotExist, "User"));
            }

            var contractEntities = new List<ContractEntity>();

            if (_userContextService.UserRoles.Contains(Role.ADMIN))
            {
                contractEntities =
                    await _unitOfWork.ContractRepository
                    .GetAllAsync(x => true, x => x
                        .Include(x => x.Approver)
                        .Include(u => u.User)
                            .ThenInclude(u => u.HealthInsurance)
                        .Include(u => u.User)
                            .ThenInclude(u => u.Workplace)
                        .Include(x => x.Room)
                            .ThenInclude(r => r.Building)
                        .Include(x => x.Room)
                            .ThenInclude(r => r.RoomType),
                    isNoTracking: true);
            }
            else
            {
                contractEntities =
                        await _unitOfWork.ContractRepository
                        .GetAllAsync(x => x.UserId == userId, x => x
                            .Include(x => x.Approver)
                            .Include(u => u.User)
                                .ThenInclude(u => u.HealthInsurance)
                            .Include(u => u.User)
                                .ThenInclude(u => u.Workplace)
                            .Include(x => x.Room)
                                .ThenInclude(r => r.Building)
                            .Include(x => x.Room)
                                .ThenInclude(r => r.RoomType),
                        isNoTracking: true);
            }

            if (model.Ids.Count > 0)
            {
                contractEntities = contractEntities.Where(x => model.Ids.Contains(x.Id)).ToList();
            }

            var response = contractEntities.Select(x => _contractMapper.MapToContractModel(x)).ToList();

            return new ApiResponse().SetOk(response);
        }

        public async Task<ApiResponse> GetInitialRegistrationData()
        {
            var genderEnums = EnumHelper.GetAllEnumDescriptions<GenderEnum>();
            var relationshipEnums = EnumHelper.GetAllEnumDescriptions<RelationshipEnum>();
            var workplaceEntities = await _unitOfWork.WorkplaceRepository.GetAllAsync(x => x.IsDeleted == false);
            var roomTypeEntities = await _unitOfWork.RoomTypeRepository.GetAllAsync(x => x.IsDeleted == false);
            var listWorkplaces = workplaceEntities.Select(entity => 
                new InitialDataWorkplaceResponseModel()
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Abbrevation = entity.Abbrevation,
                }).ToList();

            var listRoomTypes = roomTypeEntities.Select(entity =>
                new InitialDataRoomTypeResponseModel()
                {
                    Id = entity.Id,
                    RoomTypeName = entity.RoomTypeName,
                    Capacity = entity.Capacity,
                    Price = entity.Price,
                }).ToList();

            var initialRegistrationData = new InitialRegistrationDataResponseModel() 
            {
                GenderEnums = genderEnums,
                RelationshipEnums = relationshipEnums,
                ListWorkplaces = listWorkplaces,
                ListRoomTypes = listRoomTypes,
            };

            return new ApiResponse().SetOk(initialRegistrationData);
        }

        public async Task<ApiResponse> SearchBuildingsAndRoomsByGenderAndRoomType(SearchBuildingAndRoomRequestModel model)
        {
            GenderEnum genderEnum = Enum.Parse<GenderEnum>(model.Gender);

            var buildingEntities = await _unitOfWork.BuildingRepository.GetAllAsync(
                building => building.GenderRestriction == genderEnum &&
                            building.IsDeleted == false &&
                            building.Rooms.Any(room => room.RoomTypeId == model.RoomTypeId && room.IsDeleted == false),
                building => building.Include(b => b.Rooms)
            );

            var result = buildingEntities.Select(building => new SearchBuildingsAndRoomsResponseModel()
            {
                BuildingId = building.Id,
                BuildingName = building.Name,
                // copy other necessary building fields
                ListRooms = building.Rooms.Select(room => new SearchRoomsResponseModel()
                {
                    RoomId = room.Id,
                    RoomNumber = room.RoomNumber,
                    FloorNumber = room.FloorNumber,
                    TotalUsedBed = room.TotalUsedBed,
                    TotalAvailableBed = room.TotalAvailableBed,
                    Status = room.Status.ToString(),
                }).ToList()
            }).ToList();

            return new ApiResponse().SetOk(result);
        }
    }
}
