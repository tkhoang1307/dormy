﻿using Dormy.WebService.Api.Core.Constants;
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
        private readonly IContractExtensionService _contractExtensionService;
        private readonly ContractExtensionMapper _contractExtensionMapper;
        private readonly INotificationService _notificationService;
        private readonly IEmailService _emailService;

        public ContractService(IUnitOfWork unitOfWork,
                               IUserContextService userContextService,
                               IRoomTypeService roomTypeService,
                               IUserService userService,
                               IHealthInsuranceService healthInsuranceService,
                               IGuardianService guardianService,
                               IInvoiceService invoiceService,
                               IVehicleService vehicleService,
                               IContractExtensionService contractExtensionService,
                               INotificationService notificationService,
                               IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _userContextService = userContextService;
            _roomTypeService = roomTypeService;
            _userService = userService;
            _healthInsuranceService = healthInsuranceService;
            _guardianService = guardianService;
            _invoiceService = invoiceService;
            _vehicleService = vehicleService;
            _contractExtensionService = contractExtensionService;
            _notificationService = notificationService;
            _emailService = emailService;
            _userMapper = new UserMapper();
            _workplaceMapper = new WorkplaceMapper();
            _roomTypeMapper = new RoomTypeMapper();
            _contractMapper = new ContractMapper();
            _contractExtensionMapper = new ContractExtensionMapper();
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

                var registrationDate = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");

                await _notificationService.CreateNotification(new NotificationRequestModel()
                {
                    Title = NotificationMessages.CreateRegisterTitle,
                    Content = string.Format(NotificationMessages.CreateRegisterContent, $"{model.User.FirstName} {model.User.LastName}", registrationDate),
                    UserId = userIdTracking,
                    NotificationType = NotificationTypeEnum.REGISTRATION_CREATION,
                });

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

            Guid contractIdTracking = Guid.Empty;
            Guid contractExtensionIdTracking = Guid.Empty;
            using (var scope = new TransactionScope(
                TransactionScopeOption.Required,
                new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                TransactionScopeAsyncFlowOption.Enabled))
            {
                var contractEntity = _contractMapper.MapToContractEntity(model);
                var payloadContractExtension = new ContractExtensionRequestModel()
                {
                    StartDate = model.StartDate, 
                    EndDate = model.EndDate,
                };
                var contractExtensionEntity = _contractExtensionMapper.MapToContractExtensionEntity(payloadContractExtension);
                contractExtensionEntity.CreatedBy = model?.UserId ?? _userContextService.UserId;
                contractExtensionEntity.LastUpdatedBy = model?.UserId ?? _userContextService.UserId;
                contractExtensionEntity.OrderNo = 0;
                contractExtensionEntity.RoomId = model.RoomId;
                contractEntity.CreatedBy = model?.UserId ?? _userContextService.UserId;
                contractEntity.LastUpdatedBy = model?.UserId ?? _userContextService.UserId;

                List<ContractExtensionEntity> contractExtensions = new List<ContractExtensionEntity>();
                contractExtensions.Add(contractExtensionEntity);
                contractEntity.ContractExtensions = contractExtensions;

                roomEntity.TotalUsedBed += 1;
                if (roomEntity.TotalUsedBed == roomEntity.TotalAvailableBed)
                {
                    roomEntity.Status = RoomStatusEnum.FULL;
                }

                contractIdTracking = contractEntity.Id;
                contractExtensionIdTracking = contractExtensionEntity.Id;
                await _unitOfWork.ContractRepository.AddAsync(contractEntity);

                await _notificationService.CreateNotification(new NotificationRequestModel()
                {
                    Title = NotificationMessages.ContractCreateTitle,
                    Content = string.Format(NotificationMessages.ContractCreateContent, $"{userEntity.LastName} {userEntity.FirstName}", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")),
                    UserId = model?.UserId ?? _userContextService.UserId,
                    NotificationType = NotificationTypeEnum.CONTRACT_CREATION,
                });

                await _unitOfWork.SaveChangeAsync();

                // Complete transaction
                scope.Complete();
            }

            return new ApiResponse().SetCreated(contractIdTracking);
        }

        public async Task<ApiResponse> SendContractEmail(Guid contractExtensionId, bool isContractExtension = true)
        {
            if (!isContractExtension)
            {
                var responseCE = (await _unitOfWork.ContractExtensionRepository
                                                            .GetAllAsync(x => x.ContractId == contractExtensionId))
                                                            .OrderByDescending(x => x.OrderNo)
                                                            .FirstOrDefault();
                contractExtensionId = responseCE.Id;
            }
            var ce = await _unitOfWork.ContractExtensionRepository
                                                             .GetAsync(x => x.Id == contractExtensionId,
                                                                            x => x.Include(x => x.Contract)
                                                                                    .ThenInclude(xu => xu.User)
                                                                                        .ThenInclude(xuw => xuw.Workplace)
                                                                                .Include(x => x.Contract)
                                                                                    .ThenInclude(xu => xu.User)
                                                                                        .ThenInclude(xuh => xuh.HealthInsurance)
                                                                                .Include(x => x.Room)
                                                                                    .ThenInclude(rt => rt.RoomType)
                                                                                .Include(x => x.Room)
                                                                                    .ThenInclude(rb => rb.Building));
            if (ce != null)
            {
                var responseSendEmail = new RegistrationAccommodationResponseModel()
                {
                    ContractExtensionId = ce.Id,
                    OrderNo = ce.OrderNo,
                    SubmissionDate = ce.SubmissionDate,
                    StartDate = ce.StartDate,
                    EndDate = ce.EndDate,
                    Status = ce.Status.ToString(),
                    UserId = ce.Contract.UserId,
                    UserFullname = ce.Contract.User == null ? string.Empty : $"{ce.Contract.User.LastName} {ce.Contract.User.FirstName}",
                    RoomId = ce.RoomId,
                    RoomNumber = ce.Room.RoomNumber,
                    RoomTypeId = ce.Room.RoomTypeId,
                    RoomTypeName = ce.Room.RoomType.RoomTypeName,
                    BuildingId = ce.Room.BuildingId,
                    BuildingName = ce.Room.Building.Name,
                    WorkplaceName = ce.Contract.User.Workplace.Name,
                    InsuranceCardNumber = ce.Contract.User.HealthInsurance.InsuranceCardNumber,
                    RegisteredHospital = ce.Contract.User.HealthInsurance.RegisteredHospital,
                    ExpirationDate = ce.Contract.User.HealthInsurance.ExpirationDate,
                    ContractInformation = new RegistrationAccommodationContractResponseModel()
                    {
                        ContractId = ce.Contract.Id,
                        SubmissionDate = ce.Contract.SubmissionDate,
                        StartDate = ce.Contract.StartDate,
                        EndDate = ce.Contract.EndDate,
                        Status = ce.Contract.Status.ToString(),
                        NumberExtension = ce.Contract.NumberExtension,
                    }
                };

                await _emailService.SendContractEmailAsync(ce.Contract.User.Email, responseSendEmail);
            }

            return new ApiResponse().SetOk();
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

            //var (isError, errorMessage) = ContractStatusChangeValidator.VerifyContractStatusChangeValidator(contractEntity.Status, status);
            //if (isError)
            //{
            //    return new ApiResponse().SetConflict(id, message: string.Format(errorMessage, "Contract"));
            //}

            var contractExtensionEntity = (await _unitOfWork.ContractExtensionRepository
                                                            .GetAllAsync(x => x.ContractId == contractEntity.Id))
                                                            .OrderByDescending(x => x.OrderNo)
                                                            .FirstOrDefault();

            if (contractExtensionEntity == null)
            {
                return new ApiResponse().SetNotFound(id, message: string.Format(ErrorMessages.PropertyDoesNotExist, "Contract"));
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
                        var responseContractExtensionStatusWT = await _contractExtensionService.UpdateContractExtensionStatus(contractExtensionEntity.Id, ContractExtensionStatusEnum.WAITING_PAYMENT);
                        if (!responseContractExtensionStatusWT.IsSuccess)
                        {
                            return responseContractExtensionStatusWT;
                        }
                        await _notificationService.CreateNotification(new NotificationRequestModel()
                        {
                            Title = NotificationMessages.ContractAcceptTitle,
                            Content = string.Format(NotificationMessages.ContractAcceptContent, $"{_userContextService.UserName}", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")),
                            UserId = contractEntity.UserId,
                            AdminId = _userContextService.UserId,
                            NotificationType = NotificationTypeEnum.CONTRACT_ACCEPTION,
                        });
                        break;
                    case ContractStatusEnum.ACTIVE:
                        var responseContractExtensionStatusA = await _contractExtensionService.UpdateContractExtensionStatus(contractExtensionEntity.Id, ContractExtensionStatusEnum.ACTIVE);
                        if (!responseContractExtensionStatusA.IsSuccess)
                        {
                            return responseContractExtensionStatusA;
                        }
                        contractEntity.EndDate = contractExtensionEntity.EndDate.Date;
                        // Create notification
                        var user = await _unitOfWork.UserRepository.GetAsync(x => x.Id == contractEntity.UserId, isNoTracking: true);

                        await _notificationService.CreateNotification(new NotificationRequestModel()
                        {
                            Title = NotificationMessages.ContractActiveTitle,
                            Content = string.Format(NotificationMessages.ContractActiveContent, DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")),
                            UserId = contractEntity.UserId,
                            AdminId = _userContextService.UserId,
                            NotificationType = NotificationTypeEnum.CONTRACT_ACTIVATION,
                        });
                        break;
                    case ContractStatusEnum.EXTENDED:
                        await _notificationService.CreateNotification(new NotificationRequestModel()
                        {
                            Title = NotificationMessages.ContractExtendTitle,
                            Content = string.Format(NotificationMessages.ContractExtendContent, $"{_userContextService.UserName}", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")),
                            UserId = contractEntity.UserId,
                            NotificationType = NotificationTypeEnum.CONTRACT_EXTENSION,
                        });
                        break;
                    case ContractStatusEnum.EXPIRED:
                        var responseContractExtensionStatusE = await _contractExtensionService.UpdateContractExtensionStatus(contractExtensionEntity.Id, ContractExtensionStatusEnum.EXPIRED);
                        if (!responseContractExtensionStatusE.IsSuccess)
                        {
                            return responseContractExtensionStatusE;
                        }
                        break;
                    case ContractStatusEnum.TERMINATED:
                        var responseContractExtensionStatusT = await _contractExtensionService.UpdateContractExtensionStatus(contractExtensionEntity.Id, ContractExtensionStatusEnum.TERMINATED, isTriggerRule: false);
                        if (!responseContractExtensionStatusT.IsSuccess)
                        {
                            return responseContractExtensionStatusT;
                        }
                        await _notificationService.CreateNotification(new NotificationRequestModel()
                        {
                            Title = NotificationMessages.ContractTerminateTitle,
                            Content = string.Format(NotificationMessages.ContractTerminateContent, $"{_userContextService.UserName}", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")),
                            UserId = contractEntity.UserId,
                            NotificationType = NotificationTypeEnum.CONTRACT_TERMINATION,
                        });
                        break;
                    case ContractStatusEnum.REJECTED:
                        var responseContractExtensionStatusR = await _contractExtensionService.UpdateContractExtensionStatus(contractExtensionEntity.Id, ContractExtensionStatusEnum.REJECTED);
                        if (!responseContractExtensionStatusR.IsSuccess)
                        {
                            return responseContractExtensionStatusR;
                        }
                        await _notificationService.CreateNotification(new NotificationRequestModel()
                        {
                            Title = NotificationMessages.ContractRejectTitle,
                            Content = string.Format(NotificationMessages.ContractRejectContent, $"{_userContextService.UserName}", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")),
                            UserId = contractEntity.UserId,
                            AdminId = _userContextService.UserId,
                            NotificationType = NotificationTypeEnum.CONTRACT_REJECTION,
                        });
                        break;
                }

                if (contractExtensionEntity?.OrderNo == 0 || 
                    status == ContractStatusEnum.EXTENDED ||
                    status == ContractStatusEnum.TERMINATED)
                {
                    contractEntity.Status = status;
                    contractEntity.LastUpdatedBy = userId;
                    contractEntity.LastUpdatedDateUtc = DateTime.UtcNow;
                }
                
                await _unitOfWork.SaveChangeAsync();

                // Complete transaction
                scope.Complete();
            }

            return new ApiResponse().SetAccepted(id);
        }

        public async Task<ApiResponse> GetSingleContract(Guid id)
        {
            var contractEntity =
                    await _unitOfWork.ContractRepository
                    .GetAsync(x => x.Id == id, x => x
                        .Include(u => u.User)
                            .ThenInclude(u => u.HealthInsurance)
                        .Include(u => u.User)
                            .ThenInclude(u => u.Workplace)
                        .Include(x => x.Room)
                            .ThenInclude(r => r.Building)
                        .Include(x => x.Room)
                            .ThenInclude(r => r.RoomType)
                        .Include(x => x.ContractExtensions)
                            .ThenInclude(u => u.Approver)
                        .Include(x => x.ContractExtensions)
                            .ThenInclude(u => u.Room),
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
            foreach(var contractExtension in response.ContractExtensions)
            {
                if (contractExtension.InvoiceId != null)
                {
                    var responseInvoice = await _unitOfWork.InvoiceRepository.GetAsync(x => x.Id == contractExtension.InvoiceId);
                    if (responseInvoice != null)
                    {
                        contractExtension.InvoiceStatus = responseInvoice.Status.ToString();
                    }

                }
            }

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
                        .Include(u => u.User)
                        //    .ThenInclude(u => u.HealthInsurance)
                        //.Include(u => u.User)
                        //    .ThenInclude(u => u.Workplace)
                        .Include(x => x.Room)
                            .ThenInclude(r => r.Building)
                        .Include(x => x.Room)
                            .ThenInclude(r => r.RoomType),
                        //.Include(x => x.ContractExtensions)
                        //    .ThenInclude(u => u.Approver),
                    isNoTracking: true);
            }
            else
            {
                contractEntities =
                        await _unitOfWork.ContractRepository
                        .GetAllAsync(x => x.UserId == userId, x => x
                            .Include(u => u.User)
                                .ThenInclude(u => u.HealthInsurance)
                            .Include(u => u.User)
                                .ThenInclude(u => u.Workplace)
                            .Include(x => x.Room)
                                .ThenInclude(r => r.Building)
                            .Include(x => x.Room)
                                .ThenInclude(r => r.RoomType)
                            .Include(x => x.ContractExtensions)
                                .ThenInclude(u => u.Approver)
                            .Include(x => x.ContractExtensions)
                                .ThenInclude(ce => ce.Room)
                                    .ThenInclude(r => r.Building)
                            .Include(x => x.ContractExtensions)
                                .ThenInclude(ce => ce.Room)
                                    .ThenInclude(r => r.RoomType),
                        isNoTracking: true);
            }

            if (model.Ids.Count > 0)
            {
                contractEntities = contractEntities.Where(x => model.Ids.Contains(x.Id)).ToList();
            }

            if (_userContextService.UserRoles.Contains(Role.ADMIN))
            {
                var responseTemp = contractEntities.Select(x => _contractMapper.MapToContractBatchResponseModel(x)).ToList();
                return new ApiResponse().SetOk(responseTemp);
            }

            var response = contractEntities.Select(x => _contractMapper.MapToContractModel(x)).ToList();
            foreach(var contract in response)
            {
                foreach (var contractExtension in contract.ContractExtensions)
                {
                    if (contractExtension.InvoiceId != null)
                    {
                        var responseInvoice = await _unitOfWork.InvoiceRepository.GetAsync(x => x.Id == contractExtension.InvoiceId);
                        if (responseInvoice != null)
                        {
                            contractExtension.InvoiceStatus = responseInvoice.Status.ToString();
                        }

                    }
                }
            }
            return new ApiResponse().SetOk(response);
        }

        public async Task<ApiResponse> GetInitialRegistrationData()
        {
            var genderEnums = EnumHelper.GetAllEnumDescriptions<GenderEnum>()
                                        .Where(x => x.EnumValue != GenderEnum.OTHER.ToString())
                                        .ToList();
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
                    Description = entity.Description,
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
                ListRooms = building.Rooms.Where(room => room.RoomTypeId == model.RoomTypeId && room.TotalUsedBed < room.TotalAvailableBed)
                                          .Select(room => new SearchRoomsResponseModel()
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

        public async Task<ApiResponse> GetAllRoomTypesData()
        {
            var roomTypeEntities = await _unitOfWork.RoomTypeRepository.GetAllAsync(x => x.IsDeleted == false);

            var listRoomTypes = roomTypeEntities.Select(entity =>
                new InitialDataRoomTypeResponseModel()
                {
                    Id = entity.Id,
                    RoomTypeName = entity.RoomTypeName,
                    Capacity = entity.Capacity,
                    Price = entity.Price,
                    Description = entity.Description,
                }).ToList();

            return new ApiResponse().SetOk(listRoomTypes);
        }

        public async Task<ApiResponse> GetInitialCreateContractData()
        {
            var roomTypeEntities = await _unitOfWork.RoomTypeRepository.GetAllAsync(x => x.IsDeleted == false);
            var contractEntities = await _unitOfWork.ContractRepository
                .GetAllAsync(
                    x => x.UserId == _userContextService.UserId/* &&
                         (x.Status == ContractStatusEnum.ACTIVE ||
                          x.Status == ContractStatusEnum.EXPIRED ||
                          x.Status == ContractStatusEnum.EXTENDED)*/,
                    include: x => x
                        .Include(u => u.User)
                        .Include(c => c.Room)
                            .ThenInclude(r => r.RoomType)
                        .Include(c => c.Room)
                            .ThenInclude(r => r.Building)
                );

            var contractEntity = contractEntities
                .OrderByDescending(x => x.EndDate)
                .FirstOrDefault();


            var informationOfTheLatestContract = new InformationOfTheLatestContract()
            {
                ContractId = contractEntity.Id,
                StartDate = contractEntity.StartDate,
                EndDate = contractEntity.EndDate,
                Status = contractEntity.Status.ToString(),
                NumberExtension = contractEntity.NumberExtension,
                RoomId = contractEntity.Room.Id,
                RoomNumber = contractEntity.Room.RoomNumber,
                BuildingId = contractEntity.Room.Building.Id,
                BuildingName = contractEntity.Room.Building.Name,
                RoomTypeId = contractEntity.Room.RoomType.Id,
                RoomTypeName = contractEntity.Room.RoomType.RoomTypeName,
                Price = contractEntity.Room.RoomType.Price,
            };

            var listRoomTypes = roomTypeEntities.Select(entity =>
                new InitialDataRoomTypeResponseModel()
                {
                    Id = entity.Id,
                    RoomTypeName = entity.RoomTypeName,
                    Capacity = entity.Capacity,
                    Price = entity.Price,
                    Description = entity.Description,
                }).ToList();

            var responseModel = new InitialCreateEntendContractDataResponseModel()
            {
                UserInformation = new UserInformation()
                {
                    Gender = contractEntity.User.Gender.ToString(),
                },
                ContractInformation = informationOfTheLatestContract,
                ListRoomTypes = listRoomTypes,
            };

            return new ApiResponse().SetOk(responseModel);
        }

        public async Task<ApiResponse> GetInitialExtendContractData(Guid contractId)
        {
            var contractEntity = await _unitOfWork.ContractRepository
                .GetAsync(
                    x => x.UserId == _userContextService.UserId &&
                         x.Id == contractId/* &&
                         (x.Status == ContractStatusEnum.ACTIVE ||
                          x.Status == ContractStatusEnum.EXPIRED ||
                          x.Status == ContractStatusEnum.EXTENDED)*/,
                    include: x => x
                        .Include(u => u.User)
                        .Include(c => c.Room)
                            .ThenInclude(r => r.RoomType)
                        .Include(c => c.Room)
                            .ThenInclude(r => r.Building)
                );

            if (contractEntity == null)
            {
                return new ApiResponse().SetNotFound(contractId, message: string.Format(ErrorMessages.PropertyDoesNotExist, "Contract"));
            }

            var informationOfTheLatestContract = new InformationOfTheLatestContract()
            {
                ContractId = contractEntity.Id,
                StartDate = contractEntity.StartDate,
                EndDate = contractEntity.EndDate,
                Status = contractEntity.Status.ToString(),
                NumberExtension = contractEntity.NumberExtension,
                RoomId = contractEntity.Room.Id,
                RoomNumber = contractEntity.Room.RoomNumber,
                BuildingId = contractEntity.Room.Building.Id,
                BuildingName = contractEntity.Room.Building.Name,
                RoomTypeId = contractEntity.Room.RoomType.Id,
                RoomTypeName = contractEntity.Room.RoomType.RoomTypeName,
                Price = contractEntity.Room.RoomType.Price,
            };

            var roomTypeEntities = await _unitOfWork.RoomTypeRepository.GetAllAsync(x => x.IsDeleted == false);

            var listRoomTypes = roomTypeEntities.Select(entity =>
                new InitialDataRoomTypeResponseModel()
                {
                    Id = entity.Id,
                    RoomTypeName = entity.RoomTypeName,
                    Capacity = entity.Capacity,
                    Price = entity.Price,
                    Description = entity.Description,
                }).ToList();

            var responseModel = new InitialCreateEntendContractDataResponseModel()
            {
                UserInformation = new UserInformation()
                {
                    Gender = contractEntity.User.Gender.ToString(),
                },
                ContractInformation = informationOfTheLatestContract,
                ListRoomTypes = listRoomTypes,
            };

            return new ApiResponse().SetOk(responseModel);
        }
    }
}
