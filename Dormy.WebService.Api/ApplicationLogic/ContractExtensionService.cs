﻿using Dormy.WebService.Api.Core.Constants;
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
    public class ContractExtensionService : IContractExtensionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContextService _userContextService;
        private readonly ContractExtensionMapper _contractExtensionMapper;
        private readonly IInvoiceService _invoiceService;

        public ContractExtensionService(IUnitOfWork unitOfWork, 
                                        IUserContextService userContextService,
                                        IInvoiceService invoiceService)
        {
            _unitOfWork = unitOfWork;
            _userContextService = userContextService;
            _invoiceService = invoiceService;
            _contractExtensionMapper = new ContractExtensionMapper();
        }

        public async Task<ApiResponse> CreateContractExtension(ContractExtensionRequestModel model)
        {
            var contractEntity = await _unitOfWork.ContractRepository.GetAsync(x => x.Id == model.ContractId && 
                                                                                     x.UserId == _userContextService.UserId &&
                                                                                    (x.Status == ContractStatusEnum.ACTIVE ||
                                                                                    x.Status == ContractStatusEnum.EXPIRED ||
                                                                                    x.Status == ContractStatusEnum.EXTENDED),
                                                                                include: x => x.Include(x => x.Room));

            if (contractEntity == null)
            {
                return new ApiResponse().SetConflict(message: string.Format(ErrorMessages.ConflictContractExtension));
            }    

            if (!DateTimeHelper.AreValidStartDateEndDateWithoutTime(contractEntity.EndDate, model.StartDate, true))
            {
                return new ApiResponse().SetConflict(contractEntity.Id, message: string.Format(ErrorMessages.ConflictDateWhenExtendContract));
            }

            var settingEntity = await _unitOfWork.SettingRepository.GetAsync(x => x.KeyName == SettingKeyname.MaximumNumberCcontractExtensions);
            int maxNumberExtension = 5;
            if (settingEntity != null && settingEntity.IsApplied)
            {
                if (!int.TryParse(settingEntity.Value.ToString(), out maxNumberExtension))
                {
                    maxNumberExtension = 5;
                }
            }
            if (contractEntity.NumberExtension > maxNumberExtension)
            {
                return new ApiResponse().SetConflict(contractEntity.Id, message: string.Format(ErrorMessages.ContractHasReachedMaxNumberOfExtension));
            }

            var roomEntity = await _unitOfWork.RoomRepository.GetAsync(x => x.Id.Equals(model.RoomId), x => x.Include(x => x.Building));
            if (roomEntity == null)
            {
                return new ApiResponse().SetNotFound(model.RoomId, message: string.Format(ErrorMessages.PropertyDoesNotExist, "Room"));
            }

            if (roomEntity.TotalUsedBed >= roomEntity.TotalAvailableBed)
            {
                return new ApiResponse().SetBadRequest(message: string.Format(ErrorMessages.RoomIsFull));
            }

            var contractExtensionEntity = _contractExtensionMapper.MapToContractExtensionEntity(model);
            contractExtensionEntity.ContractId = contractEntity.Id;
            contractExtensionEntity.RoomId = model.RoomId;
            contractExtensionEntity.CreatedBy = _userContextService.UserId;
            contractExtensionEntity.LastUpdatedBy = _userContextService.UserId;

            if (contractEntity.NumberExtension == 0)
            {
                contractEntity.Status = ContractStatusEnum.EXTENDED;
            }
            contractEntity.NumberExtension += 1;
            contractEntity.EndDate = model.EndDate;
            contractExtensionEntity.OrderNo = contractEntity.NumberExtension;

            await _unitOfWork.ContractExtensionRepository.AddAsync(contractExtensionEntity);
            await _unitOfWork.SaveChangeAsync();

            return new ApiResponse().SetCreated(contractExtensionEntity.Id);
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
                contractExtensionEntities = await _unitOfWork.ContractExtensionRepository
                                                             .GetAllAsync(x => true,
                                                                          x => x.Include(x => x.Contract)
                                                                                .Include(x => x.Approver), isNoTracking: true);
            }
            else
            {
                contractExtensionEntities = await _unitOfWork.ContractExtensionRepository
                                                    .GetAllAsync(x => x.Contract.UserId == userId, 
                                                                 x => x.Include(x => x.Contract)
                                                                       .Include(x => x.Approver),
                                                                        isNoTracking: true);
            }

            if (model.Ids.Count > 0)
            {
                contractExtensionEntities = contractExtensionEntities.Where(x => model.Ids.Contains(x.Id)).ToList();
            }

            var response = contractExtensionEntities.Select(x => new ContractExtensionResponseModel
            {
                Id = x.Id,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                Status = x.Status.ToString(),
                SubmissionDate = x.SubmissionDate,
                InvoiceId = x.InvoiceId,
                ApproverId = x.ApproverId,
                ApproverFullName = x.Approver == null ? string.Empty : $"{x.Approver?.LastName} {x.Approver?.FirstName}",
            }).ToList();

            return new ApiResponse().SetOk(response);
        }

        public async Task<ApiResponse> GetRegistrationAccommodationBatch()
        {
            var contractExtensionEntities = await _unitOfWork.ContractExtensionRepository
                                                             .GetAllAsync(x => x.Status == ContractExtensionStatusEnum.PENDING || x.Status == ContractExtensionStatusEnum.WAITING_PAYMENT,
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
            var response = contractExtensionEntities.Select(ce => new RegistrationAccommodationResponseModel()
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
                    NumberExtension =ce.Contract.NumberExtension,
                }
            }).ToList();
            
            return new ApiResponse().SetOk(response);
        }

        public async Task<ApiResponse> GetSingleContractExtensionById(Guid id)
        {
            var contractExtensionEntity = await _unitOfWork.ContractExtensionRepository.GetAsync(x => x.Id == id, isNoTracking: true);
            if (contractExtensionEntity == null)
            {
                return new ApiResponse().SetNotFound(id, message: string.Format(ErrorMessages.PropertyDoesNotExist, "Contract extension"));
            }

            var contractExtensionResponse = new ContractExtensionResponseModel
            {
                Id = contractExtensionEntity.Id,
                StartDate = contractExtensionEntity.StartDate,
                EndDate = contractExtensionEntity.EndDate,
                Status = contractExtensionEntity.Status.ToString(),
                SubmissionDate = contractExtensionEntity.SubmissionDate,
                //Contract = (await _contractService.GetSingleContract(contractExtensionEntity.ContractId)).Result as ContractResponseModel
            };

            return new ApiResponse().SetOk(contractExtensionResponse);
        }

        public async Task<ApiResponse> UpdateContractExtension(ContractExtensionUpdationRequestModel model)
        {
            var contractExtensionEntity = await _unitOfWork.ContractExtensionRepository.GetAsync(x => x.Id == model.Id, isNoTracking: false);
            if (contractExtensionEntity == null)
            {
                return new ApiResponse().SetNotFound(model.Id, message: string.Format(ErrorMessages.PropertyDoesNotExist, "Contract extension"));
            }

            if (contractExtensionEntity.Status != ContractExtensionStatusEnum.PENDING)
            {
                return new ApiResponse().SetConflict(model.Id, message: string.Format(ErrorMessages.UpdateContractConflict));
            }

            var contractEntity = await _unitOfWork.ContractRepository.GetAsync(x => x.Id == contractExtensionEntity.ContractId, x => x.Include(x => x.Room), isNoTracking: true);
            if (contractEntity == null)
            {
                return new ApiResponse().SetNotFound(contractExtensionEntity.ContractId, message: string.Format(ErrorMessages.PropertyDoesNotExist, "Contract"));
            }

            if (contractEntity.UserId != _userContextService.UserId)
            {
                return new ApiResponse().SetForbidden(model.Id, message: string.Format(ErrorMessages.AccountDoesNotHavePermissionEntity, "contract"));
            }

            if (!DateTimeHelper.AreValidStartDateEndDateWithoutTime(contractEntity.EndDate, model.StartDate, true) ||
                contractEntity.Status != ContractStatusEnum.EXPIRED)
            {
                return new ApiResponse().SetConflict(contractEntity.Id, message: string.Format(ErrorMessages.ContractHasNotBeenExpiredYet));
            }

            contractExtensionEntity.StartDate = model.StartDate;
            contractExtensionEntity.EndDate = model.EndDate;
            contractExtensionEntity.LastUpdatedBy = _userContextService.UserId;
            contractExtensionEntity.LastUpdatedDateUtc = DateTime.UtcNow;

            await _unitOfWork.SaveChangeAsync();

            return new ApiResponse().SetAccepted(contractExtensionEntity.Id);
        }

        public async Task<ApiResponse> UpdateContractExtensionStatus(Guid id, ContractExtensionStatusEnum status, bool isTriggerRule = true)
        {
            var userId = _userContextService.UserId;
            if (userId == Guid.Empty)
            {
                return new ApiResponse().SetForbidden(message: "User not authenticated");
            }

            ContractExtensionEntity? contractExtensionEntity;
            if (_userContextService.UserRoles.Contains(Role.ADMIN))
            {
                contractExtensionEntity = await _unitOfWork.ContractExtensionRepository.GetAsync(x => x.Id == id, 
                                                                                                 x => x.Include(x => x.Contract)
                                                                                                       .Include(x => x.Room));
            }
            else
            {
                contractExtensionEntity = await _unitOfWork.ContractExtensionRepository.GetAsync(x => x.Id == id && x.Contract.UserId == userId, 
                                                                                                 x => x.Include(x => x.Contract)
                                                                                                       .Include(x => x.Room));
            }

            if (contractExtensionEntity == null)
            {
                return new ApiResponse().SetNotFound(id, message: string.Format(ErrorMessages.PropertyDoesNotExist, "Contract extension"));
            }

            if (isTriggerRule)
            {
                var (isError, errorMessage) = ContractExtensionStatusChangeValidator.VerifyContractExtensionStatusChangeValidator(contractExtensionEntity.Status, status);
                if (isError)
                {
                    return new ApiResponse().SetConflict(id, message: string.Format(errorMessage, "Contract extension"));
                }
            }

            Guid invoiceIdTracking = Guid.Empty;
            using (var scope = new TransactionScope(
                TransactionScopeOption.Required,
                new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                TransactionScopeAsyncFlowOption.Enabled))
            {
                switch (status)
                {
                    case ContractExtensionStatusEnum.WAITING_PAYMENT:
                        var roomTypeId = (await _unitOfWork.RoomRepository.GetAsync(x => x.Id == contractExtensionEntity.Contract.RoomId)).RoomTypeId;
                        var roomTypeServices = await _unitOfWork.RoomTypeServiceRepository.GetAllAsync(x => x.RoomTypeId == roomTypeId, x => x.Include(x => x.RoomService));
                        var roomServiceIdRentalPayment = roomTypeServices.Where(x => x.RoomService.RoomServiceType == RoomServiceTypeEnum.RENTAL_PAYMENT)
                                                                         .Select(x => x.RoomServiceId).FirstOrDefault();
                        TimeSpan duration = contractExtensionEntity.EndDate - contractExtensionEntity.StartDate;
                        var responseCreateInvoice = await _invoiceService.CreateNewInvoice(new InvoiceRequestModel()
                        {
                            DueDate = DateTime.Now.AddDays(15),
                            Type = InvoiceTypeEnum.PAYMENT_CONTRACT.ToString(),
                            RoomId = contractExtensionEntity.Contract.RoomId,
                            ContractId = contractExtensionEntity.Id,
                            InvoiceItems = new List<InvoiceItemRequestModel>()
                            {
                                new InvoiceItemRequestModel()
                                {
                                    RoomServiceId = roomServiceIdRentalPayment,
                                    Quantity = (decimal)Math.Round(duration.TotalDays / 30, 2),
                                }
                            }
                        });

                        if (!responseCreateInvoice.IsSuccess)
                        {
                            return responseCreateInvoice;
                        }

                        invoiceIdTracking = (Guid)responseCreateInvoice.Result;
                        contractExtensionEntity.InvoiceId = invoiceIdTracking;
                        contractExtensionEntity.ApproverId = userId;
                        break;
                    case ContractExtensionStatusEnum.ACTIVE:
                        var invoiceId = contractExtensionEntity.InvoiceId;
                        var payload = new InvoiceStatusUpdationRequestModel()
                        {
                            Id = contractExtensionEntity.InvoiceId ?? Guid.Empty,
                            Status = InvoiceStatusEnum.PAID.ToString(),
                        };
                        var responseUpdateInvoiceStatus = await _invoiceService.UpdateInvoiceStatus(payload);
                        if (!responseUpdateInvoiceStatus.IsSuccess)
                        {
                            return responseUpdateInvoiceStatus;
                        }
                        break;
                    case ContractExtensionStatusEnum.EXPIRED:
                        break;
                    case ContractExtensionStatusEnum.TERMINATED:
                    case ContractExtensionStatusEnum.REJECTED:
                        contractExtensionEntity.Room.TotalUsedBed = contractExtensionEntity.Room.TotalUsedBed > 0 ? contractExtensionEntity.Room.TotalUsedBed - 1 : 0;
                        contractExtensionEntity.Room.Status = contractExtensionEntity.Room.TotalAvailableBed == contractExtensionEntity.Room.TotalUsedBed ? RoomStatusEnum.FULL : RoomStatusEnum.AVAILABLE;
                        contractExtensionEntity.Room.LastUpdatedBy = userId;
                        contractExtensionEntity.Room.LastUpdatedDateUtc = DateTime.UtcNow;
                        if (status == ContractExtensionStatusEnum.REJECTED)
                        {
                            contractExtensionEntity.ApproverId = userId;
                        }
                        break;
                }

                contractExtensionEntity.Status = status;
                contractExtensionEntity.LastUpdatedBy = userId;
                contractExtensionEntity.LastUpdatedDateUtc = DateTime.UtcNow;

                await _unitOfWork.SaveChangeAsync();
                scope.Complete();
            }
            return new ApiResponse().SetOk(contractExtensionEntity.Id);
        }
    }
}
