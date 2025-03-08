using Dormy.WebService.Api.Core.Constants;
using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Core.Interfaces;
using Dormy.WebService.Api.Core.Utilities;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;
using Dormy.WebService.Api.Presentation.Mappers;
using Dormy.WebService.Api.Startup;
using Microsoft.EntityFrameworkCore;

namespace Dormy.WebService.Api.ApplicationLogic
{
    public class ServiceIndicatorService : IServiceIndicatorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContextService _userContextService;
        private readonly IRoomService _roomService;
        private readonly ServiceIndicatorMapper _serviceIndicatorMapper;
        public ServiceIndicatorService(IUnitOfWork unitOfWork, IUserContextService userContextService, IRoomService roomService)
        {
            _unitOfWork = unitOfWork;
            _userContextService = userContextService;
            _serviceIndicatorMapper = new ServiceIndicatorMapper();
            _roomService = roomService;
        }

        public async Task<ApiResponse> AddServiceIndicator(ServiceIndicatorRequestModel model)
        {
            var roomEntity = await _unitOfWork.RoomRepository.GetAsync(x => x.Id.Equals(model.RoomId));
            if (roomEntity == null)
            {
                return new ApiResponse().SetNotFound(model.RoomId, message: string.Format(ErrorMessages.PropertyDoesNotExist, "Room"));
            }

            var roomServiceEntity = await _unitOfWork.RoomServiceRepository.GetAsync(x => x.Id.Equals(model.RoomServiceId));
            if (roomServiceEntity == null)
            {
                return new ApiResponse().SetNotFound(model.RoomServiceId, message: string.Format(ErrorMessages.PropertyDoesNotExist, "Room service"));
            }

            var roomTypeServiceEntity = await _unitOfWork.RoomTypeServiceRepository.GetAsync(x => x.RoomServiceId.Equals(model.RoomServiceId) && x.RoomTypeId.Equals(roomEntity.RoomTypeId));
            if (roomTypeServiceEntity == null)
            {
                return new ApiResponse().SetBadRequest(model.RoomServiceId,
                            message: string.Format(ErrorMessages.RoomServiceIsNotExistedInRoom, model.RoomServiceId, model.RoomId));
            }

            var serviceIndicatorEntityCheck = await _unitOfWork.ServiceIndicatorRepository
                                                               .GetAsync(si => si.RoomId.Equals(model.RoomId) &&
                                                                               si.RoomServiceId.Equals(model.RoomServiceId) &&
                                                                               si.Month.Equals(model.Month) &&
                                                                               si.Year.Equals(model.Year));

            if (serviceIndicatorEntityCheck != null)
            {
                return new ApiResponse().SetConflict(serviceIndicatorEntityCheck.Id,
                    message: string.Format(ErrorMessages.CreateServiceIndicatorConflict,
                                            serviceIndicatorEntityCheck.RoomServiceName,
                                            serviceIndicatorEntityCheck.Month, 
                                            serviceIndicatorEntityCheck.Year));
            }

            var currentIndicator = model.OldIndicator;
            if (model.OldIndicator == null)
            {
                var theLatestServiceIndicator = await GetLatestServiceIndicator(roomEntity.Id, model.RoomServiceId);
                if (theLatestServiceIndicator > model.NewIndicator)
                {
                    return new ApiResponse().SetPreconditionFailed(message:
                        string.Format(ErrorMessages.PropertyAMustBeLessThanOrEqualToPropertyB, "OldIndicator", nameof(model.NewIndicator)));
                }

                currentIndicator = theLatestServiceIndicator;
            }

            var serviceIndicator = _serviceIndicatorMapper.MapToServiceIndicatorEntity(model);

            serviceIndicator.OldIndicator = currentIndicator ?? 0;
            serviceIndicator.RoomServiceName = roomServiceEntity.RoomServiceName;
            serviceIndicator.CreatedBy = _userContextService.UserId;
            serviceIndicator.LastUpdatedBy = _userContextService.UserId;

            await _unitOfWork.ServiceIndicatorRepository.AddAsync(serviceIndicator);
            await _unitOfWork.SaveChangeAsync();

            return new ApiResponse().SetCreated(serviceIndicator.Id);
        }

        public async Task<ApiResponse> AddBatchServiceIndicators(ServiceIndicatorCreationBatchRequestModel model)
        {
            var roomEntity = await _unitOfWork.RoomRepository.GetAsync(x => x.Id.Equals(model.RoomId));

            if (roomEntity == null)
            {
                return new ApiResponse().SetNotFound(model.RoomId, message: string.Format(ErrorMessages.PropertyDoesNotExist, "Room"));
            }

            var responseGetBatchRoomServiceByRoomId = await _roomService.GetAllRoomServicesOfRoomByRoomId(model.RoomId);
            var roomServiceIds = responseGetBatchRoomServiceByRoomId.ToList();

            var serviceIndicatorEntities = new List<ServiceIndicatorEntity>();
            var serviceIndicatorIds = new List<Guid>();
            foreach (var roomServiceIndicator in model.RoomServiceIndicators)
            {
                if (!roomServiceIds.Contains(roomServiceIndicator.RoomServiceId))
                {
                    return new ApiResponse().SetBadRequest(roomServiceIndicator.RoomServiceId,
                                message: string.Format(ErrorMessages.RoomServiceIsNotExistedInRoom, roomServiceIndicator.RoomServiceId, model.RoomId));
                }
                var roomServiceEntity = await _unitOfWork.RoomServiceRepository.GetAsync(x => x.Id.Equals(roomServiceIndicator.RoomServiceId));

                var serviceIndicatorEntityCheck = await _unitOfWork.ServiceIndicatorRepository
                                                               .GetAsync(si => si.RoomId.Equals(model.RoomId) &&
                                                                               si.RoomServiceId.Equals(roomServiceIndicator.RoomServiceId) &&
                                                                               si.Month.Equals(model.Month) &&
                                                                               si.Year.Equals(model.Year));

                if (serviceIndicatorEntityCheck != null)
                {
                    return new ApiResponse().SetConflict(serviceIndicatorEntityCheck.Id,
                        message: string.Format(ErrorMessages.CreateServiceIndicatorConflict,
                                                roomServiceIndicator.RoomServiceId + " (" + serviceIndicatorEntityCheck.RoomServiceName + ")",
                                                serviceIndicatorEntityCheck.Month,
                                                serviceIndicatorEntityCheck.Year));
                }

                var currentIndicator = roomServiceIndicator.OldIndicator;
                if (roomServiceIndicator.OldIndicator == null)
                {
                    var theLatestServiceIndicator = await GetLatestServiceIndicator(roomEntity.Id, roomServiceIndicator.RoomServiceId);
                    if (theLatestServiceIndicator > roomServiceIndicator.NewIndicator)
                    {
                        return new ApiResponse().SetPreconditionFailed(message:
                            string.Format(ErrorMessages.PropertyAMustBeLessThanOrEqualToPropertyB, "OldIndicator", nameof(roomServiceIndicator.NewIndicator)));
                    }

                    currentIndicator = theLatestServiceIndicator;
                }

                var serviceIndicatorEntity = _serviceIndicatorMapper.MapToServiceIndicatorEntity(
                    new ServiceIndicatorRequestModel()
                    {
                        RoomId = model.RoomId,
                        RoomServiceId = roomServiceIndicator.RoomServiceId,
                        Month = model.Month,
                        Year = model.Year,
                        NewIndicator = roomServiceIndicator.NewIndicator,
                    });

                serviceIndicatorEntity.OldIndicator = currentIndicator ?? 0;
                serviceIndicatorEntity.RoomServiceName = roomServiceEntity != null ? roomServiceEntity.RoomServiceName : "";
                serviceIndicatorEntity.CreatedBy = _userContextService.UserId;
                serviceIndicatorEntity.LastUpdatedBy = _userContextService.UserId;

                serviceIndicatorIds.Add(serviceIndicatorEntity.Id);
                serviceIndicatorEntities.Add(serviceIndicatorEntity);
            }

            await _unitOfWork.ServiceIndicatorRepository.AddRangeAsync(serviceIndicatorEntities);
            await _unitOfWork.SaveChangeAsync();

            return new ApiResponse().SetCreated(serviceIndicatorIds);
        }

        public async Task<ApiResponse> GetDetailServiceIndicatorById(Guid id)
        {
            var serviceIndicatorEntity = await _unitOfWork.ServiceIndicatorRepository
                                                          .GetAsync(si => si.Id.Equals(id),
                                                          include: q => q.Include(si => si.RoomService)
                                                                         .Include(si => si.Room));
            if (serviceIndicatorEntity == null)
            {
                return new ApiResponse().SetNotFound(id, message: string.Format(ErrorMessages.PropertyDoesNotExist, "Service indicator"));
            }

            var serviceIndicatorModel = _serviceIndicatorMapper.MapToServiceIndicatorResponseModel(serviceIndicatorEntity);

            var (createdUser, lastUpdatedUser) = await _unitOfWork.AdminRepository.GetAuthors(serviceIndicatorModel.CreatedBy, serviceIndicatorModel.LastUpdatedBy);

            serviceIndicatorModel.CreatedByAdminName = UserHelper.ConvertAdminIdToAdminFullname(createdUser);
            serviceIndicatorModel.LastUpdatedByAdminName = UserHelper.ConvertAdminIdToAdminFullname(lastUpdatedUser);

            return new ApiResponse().SetOk(serviceIndicatorModel);
        }

        public async Task<ApiResponse> GetServiceIndicatorBatch(GetBatchServiceIndicatorRequestModel model)
        {
            List<ServiceIndicatorEntity> serviceIndicators = new List<ServiceIndicatorEntity>();
            if (model.RoomId != null && model.RoomServiceId != null)
            {
                serviceIndicators = await _unitOfWork.ServiceIndicatorRepository
                                                     .GetAllAsync(si => si.RoomId.Equals(model.RoomId) &&
                                                                        si.RoomServiceId.Equals(model.RoomServiceId),
                                                                        include: q => q.Include(si => si.RoomService)
                                                                                       .Include(si => si.Room));  
            }
            else
            {
                if (model.RoomId != null)
                {
                    serviceIndicators = await _unitOfWork.ServiceIndicatorRepository
                                                     .GetAllAsync(si => si.RoomId.Equals(model.RoomId),
                                                                        include: q => q.Include(si => si.RoomService)
                                                                                       .Include(si => si.Room));
                }

                if (model.RoomServiceId != null)
                {
                    serviceIndicators = await _unitOfWork.ServiceIndicatorRepository
                                                     .GetAllAsync(si => si.RoomServiceId.Equals(model.RoomServiceId),
                                                                        include: q => q.Include(si => si.RoomService)
                                                                                       .Include(si => si.Room));
                }

                if (model.RoomId == null && model.RoomServiceId == null)
                {
                    serviceIndicators = await _unitOfWork.ServiceIndicatorRepository.GetAllAsync(x => model.Ids.Contains(x.Id),
                                                                                                      include: q => q.Include(si => si.RoomService)
                                                                                                                     .Include(si => si.Room));
                    
                }    
            }

            var serviceIndicatorsModel = serviceIndicators.Select(si => _serviceIndicatorMapper.MapToServiceIndicatorResponseModel(si)).ToList();

            return new ApiResponse().SetOk(serviceIndicatorsModel);
        }

        public async Task<ApiResponse> UpdateServiceIndicator(ServiceIndicatorUpdationRequestModel model)
        {
            var serviceIndicatorEntity = await _unitOfWork.ServiceIndicatorRepository.GetAsync(si => si.Id.Equals(model.Id));
            if (serviceIndicatorEntity == null)
            {
                return new ApiResponse().SetNotFound(model.Id, message: string.Format(ErrorMessages.PropertyDoesNotExist, "Service indicator"));
            }

            if (serviceIndicatorEntity.OldIndicator > model.NewIndicator)
            {
                return new ApiResponse().SetPreconditionFailed(message:
                    string.Format(ErrorMessages.PropertyAMustBeLessThanOrEqualToPropertyB, "OldIndicator", nameof(model.NewIndicator)));
            }

            var roomEntity = await _unitOfWork.RoomRepository.GetAsync(x => x.Id.Equals(model.RoomId));
            if (roomEntity == null)
            {
                return new ApiResponse().SetNotFound(model.RoomId, message: string.Format(ErrorMessages.PropertyDoesNotExist, "Room"));
            }

            var roomServiceEntity = await _unitOfWork.RoomServiceRepository.GetAsync(x => x.Id.Equals(model.RoomServiceId));
            if (roomServiceEntity == null)
            {
                return new ApiResponse().SetNotFound(model.RoomServiceId, message: string.Format(ErrorMessages.PropertyDoesNotExist, "Room service"));
            }

            var serviceIndicatorEntityCheck = await _unitOfWork.ServiceIndicatorRepository
                                                               .GetAsync(si => si.Id != serviceIndicatorEntity.Id && 
                                                                               si.RoomId == model.RoomId &&
                                                                               si.RoomServiceId == model.RoomServiceId &&
                                                                               si.Month == model.Month &&
                                                                               si.Year == model.Year);

            if (serviceIndicatorEntityCheck != null)
            {
                return new ApiResponse().SetConflict(serviceIndicatorEntityCheck.Id,
                    message: string.Format(ErrorMessages.CreateServiceIndicatorConflict,
                                            serviceIndicatorEntityCheck.RoomServiceName,
                                            serviceIndicatorEntityCheck.Month,
                                            serviceIndicatorEntityCheck.Year));
            }

            var currentIndicator = model.OldIndicator;
            if (model.OldIndicator == null)
            {
                var theLatestServiceIndicator = await GetLatestServiceIndicator(roomEntity.Id, model.RoomServiceId);
                if (theLatestServiceIndicator > model.NewIndicator)
                {
                    return new ApiResponse().SetPreconditionFailed(message:
                        string.Format(ErrorMessages.PropertyAMustBeLessThanOrEqualToPropertyB, "OldIndicator", nameof(model.NewIndicator)));
                }

                currentIndicator = theLatestServiceIndicator;
            }

            serviceIndicatorEntity.RoomId = model.RoomId;
            serviceIndicatorEntity.RoomServiceId = model.RoomServiceId;
            serviceIndicatorEntity.RoomServiceName = roomServiceEntity.RoomServiceName;
            serviceIndicatorEntity.Month = model.Month;
            serviceIndicatorEntity.Year = model.Year;
            serviceIndicatorEntity.OldIndicator = currentIndicator ?? 0;
            serviceIndicatorEntity.NewIndicator = model.NewIndicator;
            serviceIndicatorEntity.LastUpdatedBy = _userContextService.UserId;
            serviceIndicatorEntity.LastUpdatedDateUtc = DateTime.Now;

            await _unitOfWork.SaveChangeAsync();

            return new ApiResponse().SetCreated(serviceIndicatorEntity.Id);
        }

        public async Task<ApiResponse> HardDeleteBatchServiceIndicators(List<Guid> ids)
        {
            if (ids == null || ids.Count == 0)
            {
                return new ApiResponse().SetBadRequest("Service Indicator IDs must contain value");
            }

            var entities = await _unitOfWork.ServiceIndicatorRepository.GetAllAsync(x => ids.Contains(x.Id));

            if (entities == null || entities.Count != ids.Count)
            {
                var foundedIds = entities?.Select(x => x.Id).ToList() ?? new List<Guid>();
                var notFoundEntities = ids.Except(foundedIds).ToList();

                return new ApiResponse().SetNotFound(notFoundEntities, message: string.Format(ErrorMessages.PropertyDoesNotExist, "Service indicator"));
            }

            foreach(var id in ids)
            {
                await _unitOfWork.ServiceIndicatorRepository.DeleteByIdAsync(id);
            }

            await _unitOfWork.SaveChangeAsync();

            return new ApiResponse().SetOk(ids);
        }

        private async Task<decimal> GetLatestServiceIndicator(Guid roomId, Guid roomServiceId)
        {
            var serviceIndicatorEntities = await _unitOfWork.ServiceIndicatorRepository
                    .GetAllAsync(si => si.RoomId.Equals(roomId) && 
                                       si.RoomServiceId.Equals(roomServiceId));

            var latestServiceIndicatorEntity = serviceIndicatorEntities
                                                .OrderByDescending(si => si.Year)
                                                .ThenByDescending(si => si.Month)
                                                .FirstOrDefault();

            return latestServiceIndicatorEntity?.NewIndicator ?? 0;
        }
    }
}
