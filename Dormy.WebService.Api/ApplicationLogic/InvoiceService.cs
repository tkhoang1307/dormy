using Dormy.WebService.Api.Core.Constants;
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
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Newtonsoft.Json.Linq;
using System.Transactions;

namespace Dormy.WebService.Api.ApplicationLogic
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContextService _userContextService;
        private readonly InvoiceMapper _invoiceMapper;
        private readonly InvoiceItemMapper _invoiceItemMapper;
        private readonly InvoiceUserMapper _invoiceUserMapper;
        private readonly IRoomService _roomService;
        private readonly IInvoiceItemService _invoiceItemService;
        private readonly IInvoiceUserService _invoiceUserService;
        private readonly IRoomServiceService _roomServiceService;

        public InvoiceService(IUnitOfWork unitOfWork, 
                              IUserContextService userContextService, 
                              IRoomService roomService, 
                              IInvoiceItemService invoiceItemService, 
                              IInvoiceUserService invoiceUserService,
                              IRoomServiceService roomServiceService)
        {
            _unitOfWork = unitOfWork;
            _invoiceMapper = new InvoiceMapper();
            _invoiceItemMapper = new InvoiceItemMapper();
            _invoiceUserMapper = new InvoiceUserMapper();
            _userContextService = userContextService;
            _roomService = roomService;
            _invoiceItemService = invoiceItemService;
            _invoiceUserService = invoiceUserService;
            _roomServiceService = roomServiceService;
        }

        public async Task<ApiResponse> CreateNewInvoice(InvoiceRequestModel model)
        {
            var roomEntity = await _unitOfWork.RoomRepository.GetAsync(r => r.Id == model.RoomId);
            if (roomEntity == null)
            {
                return new ApiResponse().SetNotFound(model.RoomId, message: string.Format(ErrorMessages.PropertyDoesNotExist, "Room"));
            }

            if (model.Type == InvoiceTypeEnum.ROOM_SERVICE_MONTHLY.ToString())
            {
                var invoiceEntityCheck = await _unitOfWork.InvoiceRepository.GetAsync(i => i.RoomId == model.RoomId &&
                                                                                  i.Month == model.Month &&
                                                                                  i.Year == model.Year);

                if (invoiceEntityCheck != null)
                {
                    return new ApiResponse().SetConflict(invoiceEntityCheck.Id, 
                        message: string.Format(ErrorMessages.CreateInvoiceConflict, invoiceEntityCheck.Month, invoiceEntityCheck.Year));
                }
            }

            Guid invoiceIdTracking = Guid.Empty;
            using (var scope = new TransactionScope(
                TransactionScopeOption.Required,
                new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                TransactionScopeAsyncFlowOption.Enabled))
            {
                var invoiceItemsMapperRequestModel = new List<InvoiceItemMapperRequestModel>();
                decimal amountBeforePromotion = 0;
                foreach (var invoiceItem in model.InvoiceItems)
                {
                    var roomServiceName = invoiceItem.RoomServiceName;
                    var unit = invoiceItem.Unit;
                    var cost = invoiceItem.Cost;
                    var oldIndicator = invoiceItem.OldIndicator;
                    var newIndicator = invoiceItem.NewIndicator;
                    var quantity = (oldIndicator != null && newIndicator != null) ? (newIndicator - oldIndicator) : invoiceItem.Quantity;
                    if (invoiceItem?.RoomServiceId != null)
                    {
                        var roomService = await _unitOfWork.RoomServiceRepository.GetAsync(x => x.Id == invoiceItem.RoomServiceId);
                        if (roomService == null && 
                            (string.IsNullOrEmpty(roomServiceName) || string.IsNullOrEmpty(unit) || cost == null))
                        {
                            return new ApiResponse().SetNotFound(invoiceItem.RoomServiceId, message: string.Format(ErrorMessages.PropertyDoesNotExist, "Room service"));
                        }

                        roomServiceName = roomService.RoomServiceName; 
                        unit = roomService.Unit;
                        cost = roomService?.Cost ?? 0;
                        oldIndicator = roomService.IsServiceIndicatorUsed ? invoiceItem.OldIndicator : null;
                        newIndicator = roomService.IsServiceIndicatorUsed ? invoiceItem.NewIndicator : null;
                    }
                    invoiceItemsMapperRequestModel.Add(new InvoiceItemMapperRequestModel()
                    {
                        RoomServiceId = invoiceItem?.RoomServiceId,
                        RoomServiceName = roomServiceName,
                        Cost = cost ?? 0,
                        Quantity = quantity ?? 0,
                        Unit = unit,
                        OldIndicator = oldIndicator,
                        NewIndicator = newIndicator,
                    });
                    amountBeforePromotion = amountBeforePromotion + (cost * quantity ?? 0);
                }

                var invoiceUsersModel = new List<InvoiceUserMapperModel>();
                if (model.Type == InvoiceTypeEnum.ROOM_SERVICE_MONTHLY.ToString())
                {
                    var userIds = _roomService.GetAllUsersOfRoomByRoomId(model.RoomId).Result;
                    foreach (var userId in userIds)
                    {
                        invoiceUsersModel.Add(new InvoiceUserMapperModel()
                        {
                            UserId = userId,
                        });
                    }
                }

                if (model.Type == InvoiceTypeEnum.PAYMENT_CONTRACT.ToString())
                {
                    var contractExtensionEntity = await _unitOfWork.ContractExtensionRepository.GetAsync(x => x.Id == model.ContractId, x => x.Include(x => x.Contract));
                    invoiceUsersModel.Add(new InvoiceUserMapperModel()
                    {
                        UserId = contractExtensionEntity.Contract.UserId,
                    });
                }

                var invoiceName = "Hóa đơn tháng " + model.Month + "/" + model.Year + " (Invoice for month " + +model.Month + "/" + model.Year + ")";
                var invoiceStatus = InvoiceStatusEnum.DRAFT.ToString();
                if (model.Type == InvoiceTypeEnum.PAYMENT_CONTRACT.ToString())
                {
                    invoiceName = "Hóa đơn hợp đồng lưu trú (Accommodation contract invoice)";
                    invoiceStatus = InvoiceStatusEnum.UNPAID.ToString();
                }
                var invoiceMapperRequestModel = new InvoiceMapperRequestModel()
                {
                    InvoiceName = invoiceName,
                    DueDate = model.DueDate ?? DateTime.Now,
                    AmountBeforePromotion = amountBeforePromotion,
                    AmountAfterPromotion = amountBeforePromotion,
                    Month = model.Month,
                    Year = model.Year,
                    Type = model.Type,
                    Status = invoiceStatus,
                    RoomId = model.RoomId,
                    ContractId = model.Type == InvoiceTypeEnum.PAYMENT_CONTRACT.ToString() ? model.ContractId : null,
                    InvoiceItems = invoiceItemsMapperRequestModel,
                    InvoiceUsers = invoiceUsersModel,
                };

                var invoiceEntity = _invoiceMapper.MapToInvoiceEntity(invoiceMapperRequestModel);

                if (_userContextService.UserId != Guid.Empty)
                {
                    invoiceEntity.CreatedBy = _userContextService.UserId;
                    invoiceEntity.LastUpdatedBy = _userContextService.UserId;
                }

                if (invoiceEntity.InvoiceItems != null && invoiceEntity.InvoiceItems.Count > 0)
                {
                    foreach (var invoiceItem in invoiceEntity.InvoiceItems)
                    {
                        invoiceItem.CreatedBy = _userContextService.UserId;
                        invoiceItem.LastUpdatedBy = _userContextService.UserId;
                    }
                }

                if (invoiceEntity.InvoiceUsers != null && invoiceEntity.InvoiceUsers.Count > 0)
                {
                    foreach (var invoiceUser in invoiceEntity.InvoiceUsers)
                    {
                        invoiceUser.InvoiceId = invoiceEntity.Id;
                        invoiceUser.CreatedBy = _userContextService.UserId;
                        invoiceUser.LastUpdatedBy = _userContextService.UserId;
                    }
                }
                invoiceIdTracking = invoiceEntity.Id;

                await _unitOfWork.InvoiceRepository.AddAsync(invoiceEntity);

                await _unitOfWork.SaveChangeAsync();

                // Complete transaction
                scope.Complete();
            }

            return new ApiResponse().SetCreated(invoiceIdTracking);
        }

        public async Task<ApiResponse> UpdateInvoice(InvoiceUpdationRequestModel model)
        {
            var invoiceEntity = await _unitOfWork.InvoiceRepository.GetAsync(i => i.Id == model.Id);
            if (invoiceEntity == null)
            {
                return new ApiResponse().SetNotFound(model.Id, message: string.Format(ErrorMessages.PropertyDoesNotExist, "Invoice"));
            }

            if (invoiceEntity.Status != InvoiceStatusEnum.DRAFT)
            {
                return new ApiResponse().SetConflict(invoiceEntity.Id,
                       message: string.Format(ErrorMessages.UpdateInvoiceConflict));
            }

            var roomEntity = await _unitOfWork.RoomRepository.GetAsync(r => r.Id == model.RoomId);
            if (roomEntity == null)
            {
                return new ApiResponse().SetNotFound(model.RoomId, message: string.Format(ErrorMessages.PropertyDoesNotExist, "Room"));
            }

            if (model.Type == InvoiceTypeEnum.ROOM_SERVICE_MONTHLY.ToString())
            {
                var invoiceEntityCheck = await _unitOfWork.InvoiceRepository.GetAsync(i => i.Id != invoiceEntity.Id &&
                                                                                  i.RoomId == model.RoomId &&
                                                                                  i.Month == model.Month &&
                                                                                  i.Year == model.Year);

                if (invoiceEntityCheck != null)
                {
                    return new ApiResponse().SetConflict(invoiceEntityCheck.Id,
                        message: string.Format(ErrorMessages.CreateInvoiceConflict, invoiceEntityCheck.Month, invoiceEntityCheck.Year));
                }
            }

            var responseDeleteBatchInvoiceItem = await _invoiceItemService.HardDeleteInvoiceItemsBatchByInvoiceId(invoiceEntity.Id);
            if (!responseDeleteBatchInvoiceItem.IsSuccess)
            {
                return responseDeleteBatchInvoiceItem;
            }

            var invoiceItemEntities = new List<InvoiceItemEntity>();
            decimal amountBeforePromotion = 0;
            foreach (var invoiceItem in model.InvoiceItems)
            {
                var roomServiceName = invoiceItem.RoomServiceName;
                var unit = invoiceItem.Unit;
                var cost = invoiceItem.Cost;
                var oldIndicator = invoiceItem.OldIndicator;
                var newIndicator = invoiceItem.NewIndicator;
                var quantity = (oldIndicator != null && newIndicator != null) ? (newIndicator - oldIndicator) : invoiceItem.Quantity;
                if (invoiceItem?.RoomServiceId != null)
                {
                    var roomService = await _unitOfWork.RoomServiceRepository.GetAsync(x => x.Id == invoiceItem.RoomServiceId);
                    if (roomService == null &&
                        (string.IsNullOrEmpty(roomServiceName) || string.IsNullOrEmpty(unit) || cost == null))
                    {
                        return new ApiResponse().SetNotFound(invoiceItem.RoomServiceId, message: string.Format(ErrorMessages.PropertyDoesNotExist, "Room service"));
                    }

                    roomServiceName = roomService.RoomServiceName;
                    unit = roomService.Unit;
                    cost = roomService?.Cost ?? 0;
                    oldIndicator = roomService.IsServiceIndicatorUsed ? invoiceItem.OldIndicator : null;
                    newIndicator = roomService.IsServiceIndicatorUsed ? invoiceItem.NewIndicator : null;
                }
                var invoiceItemMapperRequestModel = new InvoiceItemMapperRequestModel()
                {
                    RoomServiceId = invoiceItem?.RoomServiceId,
                    RoomServiceName = roomServiceName,
                    Cost = cost ?? 0,
                    Quantity = quantity ?? 0,
                    Unit = unit,
                    OldIndicator = oldIndicator,
                    NewIndicator = newIndicator,
                };

                var invoiceItemEntity = _invoiceItemMapper.MapToInvoiceItemEntity(invoiceItemMapperRequestModel);
                invoiceItemEntity.InvoiceId = invoiceEntity.Id;
                invoiceItemEntity.CreatedBy = _userContextService.UserId;
                invoiceItemEntity.LastUpdatedBy = _userContextService.UserId;
                invoiceItemEntities.Add(invoiceItemEntity);
                amountBeforePromotion = amountBeforePromotion + (cost * quantity ?? 0);
            }


            var responseDeleteBatchInvoiceUser = await _invoiceUserService.HardDeleteInvoiceUsersBatchByInvoiceId(invoiceEntity.Id);
            if (!responseDeleteBatchInvoiceUser.IsSuccess)
            {
                return responseDeleteBatchInvoiceUser;
            }
            var userIds = _roomService.GetAllUsersOfRoomByRoomId(model.RoomId).Result;
            var invoiceUserEntities = new List<InvoiceUserEntity>();
            foreach (var userId in userIds)
            {
                var invoiceUserMapperModel = new InvoiceUserMapperModel()
                {
                    UserId = userId,
                    InvoiceId = invoiceEntity.Id,
                };

                var invoiceUserEntity = _invoiceUserMapper.MapToInvoiceUserEntity(invoiceUserMapperModel);

                invoiceUserEntity.CreatedBy = _userContextService.UserId;
                invoiceUserEntity.LastUpdatedBy = _userContextService.UserId;

                invoiceUserEntities.Add(invoiceUserEntity);
            }

            invoiceEntity.InvoiceName = "Hóa đơn tháng " + model.Month + "/" + model.Year + " (Invoice for month " + +model.Month + "/" + model.Year + ")";
            invoiceEntity.DueDate = model.DueDate ?? DateTime.Now;
            invoiceEntity.AmountBeforePromotion = amountBeforePromotion;
            invoiceEntity.AmountAfterPromotion = amountBeforePromotion;
            invoiceEntity.Month = model.Month;
            invoiceEntity.Year = model.Year;
            invoiceEntity.Type = (InvoiceTypeEnum)Enum.Parse(typeof(InvoiceTypeEnum), model.Type);
            invoiceEntity.RoomId = model.RoomId;

            if (_userContextService.UserId != Guid.Empty)
            {
                invoiceEntity.LastUpdatedBy = _userContextService.UserId;
            }

            await _unitOfWork.InvoiceItemRepository.AddRangeAsync(invoiceItemEntities);

            await _unitOfWork.InvoiceUserRepository.AddRangeAsync(invoiceUserEntities);

            await _unitOfWork.SaveChangeAsync();

            return new ApiResponse().SetAccepted(invoiceEntity.Id);
        }

        public async Task<ApiResponse> UpdateInvoiceStatus(InvoiceStatusUpdationRequestModel model)
        {
            var invoiceEntity = await _unitOfWork.InvoiceRepository.GetAsync(i => i.Id == model.Id);
            if (invoiceEntity == null)
            {
                return new ApiResponse().SetNotFound(model.Id, message: string.Format(ErrorMessages.PropertyDoesNotExist, "Invoice"));
            }
            var statusChanged = (InvoiceStatusEnum)Enum.Parse(typeof(InvoiceStatusEnum), model.Status);
            var (isError, errorMessage) = InvoiceStatusChangeValidator.VerifyInvoiceStatusChangeValidator(invoiceEntity.Status, statusChanged);
            if (isError)
            {
                return new ApiResponse().SetConflict(invoiceEntity.Id,
                                                     message: string.Format(errorMessage, "invoice"));
            }

            invoiceEntity.Status = statusChanged;
            invoiceEntity.LastUpdatedBy = _userContextService.UserId;
            invoiceEntity.LastUpdatedDateUtc = DateTime.Now;

            await _unitOfWork.SaveChangeAsync();

            return new ApiResponse().SetAccepted(invoiceEntity.Id);
        }

        public async Task<ApiResponse> GetInitialInvoiceCreation(GetInitialInvoiceCreationRequestModel model)
        {
            var roomEntity = await _unitOfWork.RoomRepository.GetAsync(r => r.Id == model.RoomId);
            if (roomEntity == null)
            {
                return new ApiResponse().SetNotFound(model.RoomId, message: string.Format(ErrorMessages.PropertyDoesNotExist, "Room"));
            }

            var roomServiceIds = await _roomService.GetAllRoomServicesOfRoomByRoomId(roomEntity.Id);
            var initialRoomServices = new List<GetInitialInvoiceItemCreationResponseModel>();
            foreach (var roomServiceId in roomServiceIds)
            {
                var roomServiceEntity = await _unitOfWork.RoomServiceRepository.GetAsync(x => x.Id.Equals(roomServiceId));
                if (roomServiceEntity.RoomServiceType != RoomServiceTypeEnum.RENTAL_PAYMENT)
                {
                    var serviceIndicatorEntity = await _unitOfWork.ServiceIndicatorRepository
                                                               .GetAsync(si => si.RoomId.Equals(model.RoomId) &&
                                                                               si.RoomServiceId.Equals(roomServiceId) &&
                                                                               si.Month.Equals(model.Month) &&
                                                                               si.Year.Equals(model.Year));

                    initialRoomServices.Add(new GetInitialInvoiceItemCreationResponseModel()
                    {
                        RoomServiceId = roomServiceId,
                        RoomServiceName = roomServiceEntity.RoomServiceName,
                        RoomServiceType = roomServiceEntity.RoomServiceType.ToString(),
                        Cost = roomServiceEntity.Cost,
                        Unit = roomServiceEntity.Unit,
                        IsServiceIndicatorUsed = roomServiceEntity.IsServiceIndicatorUsed,
                        CurrentIndicator = roomServiceEntity.IsServiceIndicatorUsed ? (serviceIndicatorEntity?.NewIndicator ?? 0) : null,
                    });
                }
            }

            var initialInvoiceCreation = new GetInitialInvoiceCreationResponseModel()
            {
                Month = model.Month,
                Year = model.Year,
                RoomId = model.RoomId,
                RoomServices = initialRoomServices,
            };

            return new ApiResponse().SetOk(initialInvoiceCreation);
        }

        public async Task<ApiResponse> GetInitialInvoiceEdit(Guid id)
        {
            var invoiceEntity = await _unitOfWork.InvoiceRepository
                                                .GetAsync(i => i.Id == id,
                                                    include: q => q.Include(i => i.InvoiceItems));
                                                                    //.Include(i => i.InvoiceUsers)
                                                                    //    .ThenInclude(iu => iu.User)
                                                                    //.Include(i => i.Room));
            if (invoiceEntity == null)
            {
                return new ApiResponse().SetNotFound(id, message: string.Format(ErrorMessages.PropertyDoesNotExist, "Invoice"));
            }

            if (invoiceEntity.Status != InvoiceStatusEnum.DRAFT)
            {
                return new ApiResponse().SetConflict(invoiceEntity.Id,
                       message: string.Format(ErrorMessages.UpdateInvoiceConflict));
            }

            var responseModel = new GetInitialInvoiceEditResponseModel()
            {
                InvoiceId = invoiceEntity.Id,
                InvoiceName = invoiceEntity.InvoiceName,
                DueDate = invoiceEntity.DueDate,
                Type = invoiceEntity.Type.ToString(),
                Status = invoiceEntity.Status.ToString(),
                Month = invoiceEntity.Month ?? 0,
                Year = invoiceEntity?.Year ?? 0,
                RoomId = invoiceEntity.RoomId,
                InvoiceItems = invoiceEntity.InvoiceItems.Select(ii => new GetInitialInvoiceItemEditResponseModel()
                {
                    InvoiceItemId = ii.Id,
                    RoomServiceId = ii.RoomServiceId,
                    RoomServiceName = ii.RoomServiceName,
                    Cost = ii.Cost,
                    Unit = ii.Unit,
                    Quantity = ii.Quantity,
                    Total = ii.Cost * ii.Quantity,
                    OldIndicator = ii.OldIndicator,
                    NewIndicator = ii.NewIndicator,
                    IsServiceIndicatorUsed = ii.OldIndicator.HasValue && ii.NewIndicator.HasValue,
                }).ToList()
            };

            return new ApiResponse().SetOk(responseModel);
        }

        public async Task<ApiResponse> GetInvoiceBatch(GetBatchInvoiceRequestModel model)
        {
            var entities = new List<InvoiceEntity>();

            switch (_userContextService.UserRoles.FirstOrDefault())
            {
                case Role.ADMIN:
                    entities = await _unitOfWork.InvoiceRepository.GetAllAsync(x => true);

                    if (model.RoomId != null)
                    {
                        entities = entities.Where(x => x.RoomId == model.RoomId).ToList();
                    }

                    if (!string.IsNullOrEmpty(model.InvoiceType))
                    {
                        entities = entities.Where(x => x.Type.ToString() == model.InvoiceType).ToList();
                    }

                    if (model.Ids.Count > 0)
                    {
                        entities = entities.Where(x => model.Ids.Contains(x.Id)).ToList();
                    }

                    break;
                case Role.USER:
                    var invoiceUsers = await _unitOfWork.InvoiceUserRepository.GetAllAsync(iu => iu.UserId == _userContextService.UserId);
                    var invoiceIds = invoiceUsers.Select(iu => iu.InvoiceId).ToList();
                    entities = await _unitOfWork.InvoiceRepository.GetAllAsync(x => invoiceIds.Contains(x.Id) && x.Status != InvoiceStatusEnum.DRAFT);
                    if (!string.IsNullOrEmpty(model.InvoiceType))
                    {
                        entities = entities.Where(x => x.Type.ToString() == model.InvoiceType).ToList();
                    }

                    break;
            }

            var invoiceModels = entities.Select(x => _invoiceMapper.MapToInvoiceBatchResponseModel(x)).ToList();

            for (int i = 0; i < invoiceModels.Count; i++)
            {
                var invoice = invoiceModels[i];

                var (createdUser, lastUpdatedUser) = await _unitOfWork.AdminRepository.GetAuthors(invoice.CreatedBy, invoice.LastUpdatedBy);
                var roomNumber = await _unitOfWork.RoomRepository.GetRoomName(invoice.RoomId);

                invoice.CreatedByCreator = UserHelper.ConvertAdminIdToAdminFullname(createdUser);
                invoice.LastUpdatedByUpdater = UserHelper.ConvertAdminIdToAdminFullname(lastUpdatedUser);
                invoice.RoomName = roomNumber != null ? ("P" + roomNumber) : "";
            }

            return new ApiResponse().SetOk(invoiceModels);
        }

        public async Task<ApiResponse> GetInvoiceById(Guid id)
        {
            var invoiceEntity = await _unitOfWork.InvoiceRepository
                                                .GetAsync(i => i.Id == id,
                                                    include: q => q.Include(i => i.InvoiceItems)
                                                                    .Include(i => i.InvoiceUsers)
                                                                        .ThenInclude(iu => iu.User)
                                                                    .Include(i => i.Room));
            if (invoiceEntity == null)
            {
                return new ApiResponse().SetNotFound(id, message: string.Format(ErrorMessages.PropertyDoesNotExist, "Invoice"));
            }

            if (_userContextService.UserRoles.Contains(Role.USER))
            {
                if (invoiceEntity.Status == InvoiceStatusEnum.DRAFT)
                {
                    return new ApiResponse().SetBadRequest(id, message: string.Format(ErrorMessages.AccountDoesNotHavePermissionEntity, "invoice"));
                }

                var invoiceUserEntity = await _unitOfWork.InvoiceUserRepository.GetAsync(iu => iu.UserId == _userContextService.UserId && iu.InvoiceId == invoiceEntity.Id);
                if (invoiceUserEntity == null)
                {
                    return new ApiResponse().SetForbidden(id, message: ErrorMessages.AccountDoesNotHavePermission);
                }
            }

            var invoiceModel = _invoiceMapper.MapToInvoiceResponseModel(invoiceEntity);

            var (createdUser, lastUpdatedUser) = await _unitOfWork.AdminRepository.GetAuthors(invoiceModel.CreatedBy, invoiceModel.LastUpdatedBy);

            invoiceModel.CreatedByCreator = UserHelper.ConvertAdminIdToAdminFullname(createdUser);
            invoiceModel.LastUpdatedByUpdater = UserHelper.ConvertAdminIdToAdminFullname(lastUpdatedUser);

            return new ApiResponse().SetOk(invoiceModel);
        }

        public async Task<ApiResponse> HardDeleteInvoiceById(Guid id)
        {
            var invoiceEntity = await _unitOfWork.InvoiceRepository.GetAsync(i => i.Id == id);
            if (invoiceEntity == null)
            {
                return new ApiResponse().SetNotFound(id, message: string.Format(ErrorMessages.PropertyDoesNotExist, "Invoice"));
            }

            if (invoiceEntity.Status != InvoiceStatusEnum.DRAFT)
            {
                return new ApiResponse().SetConflict(invoiceEntity.Id,
                       message: string.Format(ErrorMessages.DeleteInvoiceConflict));
            }

            await _unitOfWork.InvoiceRepository.DeleteByIdAsync(invoiceEntity.Id);

            await _unitOfWork.SaveChangeAsync();

            return new ApiResponse().SetOk(invoiceEntity.Id);
        }

        public async Task<ApiResponse> GetRoomsForInitialInvoiceCreation()
        {
            var roomEntities = await _unitOfWork.RoomRepository.GetAllAsync(x => x.TotalUsedBed > 0, include: q => q.Include(i => i.Building));

            var roomModels = roomEntities
                .OrderBy(room => room.Building.Name)
                .ThenBy(room => room.RoomNumber)
                .Select(room => new RoomsForInvoiceResponseModel
                {
                    RoomId = room.Id,
                    RoomNumber = room.RoomNumber,
                    FloorNumber = room.FloorNumber,
                    BuildingName = room.Building.Name,
                });

            return new ApiResponse().SetOk(roomModels);
        }
    }
}
