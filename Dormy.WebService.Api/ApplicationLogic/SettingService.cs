using Dormy.WebService.Api.Core.Interfaces;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;
using Dormy.WebService.Api.Presentation.Mappers;
using Dormy.WebService.Api.Startup;
using Microsoft.EntityFrameworkCore;

namespace Dormy.WebService.Api.ApplicationLogic
{
    public class SettingService : ISettingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly SettingMapper _mapper;
        private readonly IUserContextService _userContextService;

        public SettingService(IUnitOfWork unitOfWork, IUserContextService userContextService)
        {
            _unitOfWork = unitOfWork;
            _mapper = new();
            _userContextService = userContextService;
        }

        public async Task<ApiResponse> CreateSetting(SettingRequestModel model)
        {
            var entity = _mapper.MapToSettingEntity(model);
            entity.CreatedBy = _userContextService.UserId;
            entity.AdminId = _userContextService.UserId;

            await _unitOfWork.SettingRepository.AddAsync(entity);
            await _unitOfWork.SaveChangeAsync();
            return new ApiResponse().SetCreated(entity.Id);
        }

        public async Task<ApiResponse> GetSettingById(Guid id)
        {
            var entity = await _unitOfWork.SettingRepository.GetAsync(x => x.Id == id, x => x.Include(x => x.Admin));

            if (entity == null)
            {
                return new ApiResponse().SetNotFound(id);
            }

            var result = _mapper.MapToSettingResponseModel(entity);

            var (createdAuthor, lastUpdateAuthor) = await _unitOfWork.AdminRepository.GetAuthors(entity.CreatedBy, entity.LastUpdatedBy);

            result.CreatedByCreator = createdAuthor?.UserName ?? string.Empty;
            result.LastUpdatedByUpdater = lastUpdateAuthor?.UserName ?? string.Empty;

            return new ApiResponse().SetOk(result);
        }

        public async Task<ApiResponse> GetSettings()
        {
            var entities = await _unitOfWork.SettingRepository.GetAllAsync(x => true, x => x.Include(x => x.Admin));

            if (entities is null || entities.Count == 0)
            {
                return new ApiResponse().SetOk(new List<SettingResponseModel>());
            }

            var result = entities.Select(x => _mapper.MapToSettingResponseModel(x)).ToList();

            for (int i = 0; i < result.Count; i++)
            {
                var model = result[i];
                var (createdAuthor, lastUpdateAuthor) = await _unitOfWork.AdminRepository.GetAuthors(model.CreatedBy, model.LastUpdatedBy);
                model.CreatedByCreator = createdAuthor?.UserName ?? string.Empty;
                model.LastUpdatedByUpdater = lastUpdateAuthor?.UserName ?? string.Empty;
            }

            return new ApiResponse().SetOk(result);
        }

        public async Task<ApiResponse> SoftDeleteSetting(Guid id)
        {
            var entity = await _unitOfWork.SettingRepository.GetAsync(x => x.Id == id);

            if (entity == null)
            {
                return new ApiResponse().SetNotFound(id);
            }

            entity.IsDeleted = true;
            
            await _unitOfWork.SaveChangeAsync();

            return new ApiResponse().SetOk(entity.Id);
        }

        public async Task<ApiResponse> UpdateSetting(SettingUpdateRequestModel model)
        {
            var entity = await _unitOfWork.SettingRepository.GetAsync(x => x.Id == model.Id);

            if (entity == null)
            {
                return new ApiResponse().SetNotFound(model.Id);
            }

            entity.LastUpdatedBy = _userContextService.UserId;
            entity.LastUpdatedDateUtc = DateTime.UtcNow;
            entity.AdminId = _userContextService.UserId;
            entity.Name = model.Name;
            entity.ParameterDate = model.ParameterDate;
            entity.ParameterBool = model.ParameterBool;

            await _unitOfWork.SaveChangeAsync();

            return new ApiResponse().SetOk(model.Id);
        }
    }
}
