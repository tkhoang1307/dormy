using Dormy.WebService.Api.Core.Constants;
using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Core.Interfaces;
using Dormy.WebService.Api.Models.Constants;
using Dormy.WebService.Api.Models.Enums;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;
using Dormy.WebService.Api.Presentation.Mappers;
using Dormy.WebService.Api.Startup;
using Microsoft.EntityFrameworkCore;

namespace Dormy.WebService.Api.ApplicationLogic
{
    public class ViolationService : IViolationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContextService _userContextService;
        private readonly ViolationMapper _violationMapper;

        public ViolationService(IUnitOfWork unitOfWork, IUserContextService userContextService)
        {
            _unitOfWork = unitOfWork;
            _userContextService = userContextService;
            _violationMapper = new ViolationMapper();
        }

        public async Task<ApiResponse> CreateViolation(ViolationRequestModel model)
        {
            // check user exist, then save the violation
            var userEntity = await _unitOfWork.UserRepository.GetAsync(x => x.Id == model.UserId);
            if (userEntity == null)
            {
                return new ApiResponse().SetNotFound("User not found");
            }

            var violationEntity = _violationMapper.MapToViolationEntity(model);

            violationEntity.CreatedBy = _userContextService.UserId;
            violationEntity.LastUpdatedBy = _userContextService.UserId;

            await _unitOfWork.ViolationRepository.AddAsync(violationEntity);

            var user = await _unitOfWork.UserRepository.GetAsync(x => x.Id == model.UserId, isNoTracking: true);
            var admin = await _unitOfWork.AdminRepository.GetAsync(x => x.Id == _userContextService.UserId, isNoTracking: true);

            NotificationEntity notificationEntity = new NotificationEntity()
            {
                Title = NotificationMessages.CreateViolationTitle,
                Content = string.Format(NotificationMessages.CreateViolationContent, DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")),
                Date = DateTime.UtcNow,
                IsRead = false,
                UserId = model.UserId,
                AdminId = _userContextService.UserId,
                NotificationType = NotificationTypeEnum.VIOLATION_CREATION,
                CreatedBy =_userContextService.UserId,
            };

            await _unitOfWork.NotificationRepository.AddAsync(notificationEntity);

            await _unitOfWork.SaveChangeAsync();

            return new ApiResponse().SetCreated(violationEntity.Id);
        }

        public async Task<ApiResponse> GetSingleViolation(Guid id)
        {
            var violationEntity = await _unitOfWork.ViolationRepository.GetAsync(x => x.Id == id, x => x.Include(x => x.User), isNoTracking: true);
            if (violationEntity == null)
            {
                return new ApiResponse().SetNotFound(id, message: string.Format(ErrorMessages.PropertyDoesNotExist, "Violation"));
            }

            var result = _violationMapper.MapToViolationResponseModel(violationEntity);

            return new ApiResponse().SetOk(result);
        }

        public async Task<ApiResponse> GetViolationBatch(GetBatchRequestModel model)
        {

            List<ViolationEntity> violationEntities = [];

            if (_userContextService.UserRoles.Contains(Role.ADMIN))
            {
                violationEntities = await _unitOfWork.ViolationRepository.GetAllAsync(x => true, x => x.Include(x => x.User), isNoTracking: true);
            }
            else
            {
                violationEntities = await _unitOfWork.ViolationRepository.GetAllAsync(x => x.UserId == _userContextService.UserId, x => x.Include(x => x.User), isNoTracking: true);
            }

            if (model.Ids.Count > 0)
            {
                violationEntities = violationEntities.Where(x => model.Ids.Contains(x.Id)).ToList();
            }

            var result = violationEntities.Select(violationEntity => _violationMapper.MapToViolationResponseModel(violationEntity)).ToList();

            return new ApiResponse().SetOk(result);
        }

        public async Task<ApiResponse> SoftDeleteViolation(Guid id)
        {
            var violationEntity = await _unitOfWork.ViolationRepository.GetAsync(x => x.Id == id);
            if (violationEntity == null)
            {
                return new ApiResponse().SetNotFound(id, message: string.Format(ErrorMessages.PropertyDoesNotExist, "Violation"));
            }

            violationEntity.IsDeleted = true;
            violationEntity.LastUpdatedBy = _userContextService.UserId;
            violationEntity.LastUpdatedDateUtc = DateTime.UtcNow;

            await _unitOfWork.SaveChangeAsync();

            return new ApiResponse().SetOk(violationEntity.Id);
        }

        public async Task<ApiResponse> UpdateViolation(ViolationUpdationRequestModel model)
        {
            var violationEntity = await _unitOfWork.ViolationRepository.GetAsync(x => x.Id == model.Id);
            if (violationEntity == null)
            {
                return new ApiResponse().SetNotFound(model.Id, message: string.Format(ErrorMessages.PropertyDoesNotExist, "Violation"));
            }

            var userEntity = await _unitOfWork.UserRepository.GetAsync(x => x.Id == model.UserId);
            if (userEntity == null)
            {
                return new ApiResponse().SetNotFound("User not found");
            }

            violationEntity.Description = model.Description;
            violationEntity.Penalty = model.Penalty;
            violationEntity.ViolationDate = model.ViolationDate;
            violationEntity.UserId = model.UserId;
            violationEntity.LastUpdatedBy = _userContextService.UserId;
            violationEntity.LastUpdatedDateUtc = DateTime.UtcNow;

            await _unitOfWork.SaveChangeAsync();

            return new ApiResponse().SetOk(violationEntity.Id);
        }
    }
}
