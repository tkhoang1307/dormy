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
        private readonly ServiceIndicatorMapper _serviceIndicatorMapper;
        public ServiceIndicatorService(IUnitOfWork unitOfWork, IUserContextService userContextService)
        {
            _unitOfWork = unitOfWork;
            _userContextService = userContextService;
            _serviceIndicatorMapper = new ServiceIndicatorMapper();
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

            var serviceIndicator = _serviceIndicatorMapper.MapToServiceIndicatorEntity(model);
            serviceIndicator.RoomServiceName = roomServiceEntity.RoomServiceName;
            serviceIndicator.CreatedBy = _userContextService.UserId;
            serviceIndicator.LastUpdatedBy = _userContextService.UserId;

            await _unitOfWork.ServiceIndicatorRepository.AddAsync(serviceIndicator);
            await _unitOfWork.SaveChangeAsync();

            return new ApiResponse().SetCreated(serviceIndicator.Id);
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

            serviceIndicatorEntity.RoomId = model.RoomId;
            serviceIndicatorEntity.RoomServiceId = model.RoomServiceId;
            serviceIndicatorEntity.Month = model.Month;
            serviceIndicatorEntity.Year = model.Year;
            serviceIndicatorEntity.OldIndicator = model.OldIndicator;
            serviceIndicatorEntity.NewIndicator = model.NewIndicator;

            serviceIndicatorEntity.LastUpdatedBy = _userContextService.UserId;
            serviceIndicatorEntity.LastUpdatedDateUtc = DateTime.Now;

            await _unitOfWork.SaveChangeAsync();

            return new ApiResponse().SetCreated(serviceIndicatorEntity.Id);
        }

        public Task<ApiResponse> HardDeleteServiceIndicator(Guid id)
        {
            throw new NotImplementedException();
        }

        private async Task<ApiResponse> GetLatestServiceIndicator(Guid roomId, Guid roomServiceId)
        {
            var serviceIndicatorEntities = await _unitOfWork.ServiceIndicatorRepository
                    .GetAllAsync(si => si.RoomId.Equals(roomId) && 
                                       si.RoomServiceId.Equals(roomServiceId));

            var latestServiceIndicatorEntity = serviceIndicatorEntities
                                                .OrderByDescending(si => si.Year)
                                                .ThenByDescending(si => si.Month)
                                                .FirstOrDefault();

            return new ApiResponse().SetOk(latestServiceIndicatorEntity?.NewIndicator ?? 0);
        }
    }
}
