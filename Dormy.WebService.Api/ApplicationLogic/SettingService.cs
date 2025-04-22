using Dormy.WebService.Api.Core.Constants;
using Dormy.WebService.Api.Core.Interfaces;
using Dormy.WebService.Api.Core.Utilities;
using Dormy.WebService.Api.Models.Enums;
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
            var settingEntity = await _unitOfWork.SettingRepository.GetAsync(x => x.KeyName == model.KeyName);
            if (settingEntity != null)
            {
                return new ApiResponse().SetConflict(model.KeyName, message: string.Format(ErrorMessages.KeyNameIsExistedInSystem, model.KeyName));
            }

            var entity = _mapper.MapToSettingEntity(model);

            entity.CreatedBy = _userContextService.UserId;
            entity.LastUpdatedBy = _userContextService.UserId;

            await _unitOfWork.SettingRepository.AddAsync(entity);
            await _unitOfWork.SaveChangeAsync();
            return new ApiResponse().SetCreated(entity.KeyName);
        }

        public async Task<ApiResponse> UpdateSetting(SettingUpdateValueRequestModel model)
        {
            var entity = await _unitOfWork.SettingRepository.GetAsync(x => x.KeyName == model.KeyName);

            if (entity == null)
            {
                return new ApiResponse().SetNotFound(model.KeyName, message: string.Format(ErrorMessages.PropertyDoesNotExist, model.KeyName));
            }

            
            entity.Value = model.Value;
            entity.DataType = (SettingDataTypeEnum)Enum.Parse(typeof(SettingDataTypeEnum), model.DataType);
            entity.LastUpdatedBy = _userContextService.UserId;
            entity.LastUpdatedDateUtc = DateTime.UtcNow;

            await _unitOfWork.SaveChangeAsync();

            return new ApiResponse().SetAccepted(model.KeyName);
        }

        public async Task<ApiResponse> TurnOnOrTurnOffSetting(SettingTurnOnOffRequestModel model)
        {
            var entity = await _unitOfWork.SettingRepository.GetAsync(x => x.KeyName == model.KeyName);

            if (entity == null)
            {
                return new ApiResponse().SetNotFound(model.KeyName, message: string.Format(ErrorMessages.PropertyDoesNotExist, model.KeyName));
            }


            entity.IsApplied = model.IsApplied;
            entity.LastUpdatedBy = _userContextService.UserId;
            entity.LastUpdatedDateUtc = DateTime.UtcNow;

            await _unitOfWork.SaveChangeAsync();

            return new ApiResponse().SetAccepted(model.KeyName);
        }

        public async Task<ApiResponse> GetSettingByKeyName(string keyname)
        {
            var entity = await _unitOfWork.SettingRepository.GetAsync(x => x.KeyName == keyname);

            if (entity == null)
            {
                return new ApiResponse().SetNotFound(keyname, message: string.Format(ErrorMessages.PropertyDoesNotExist, keyname));
            }

            var result = _mapper.MapToSettingResponseModel(entity);

            var (createdAuthor, lastUpdateAuthor) = await _unitOfWork.AdminRepository.GetAuthors(entity.CreatedBy, entity.LastUpdatedBy);

            result.CreatedByCreator = UserHelper.ConvertAdminIdToAdminFullname(createdAuthor);
            result.LastUpdatedByUpdater = UserHelper.ConvertAdminIdToAdminFullname(lastUpdateAuthor);

            return new ApiResponse().SetOk(result);
        }

        public async Task<ApiResponse> GetAllSettings()
        {
            var entities = await _unitOfWork.SettingRepository.GetAllAsync(x => true);

            if (entities is null || entities.Count == 0)
            {
                return new ApiResponse().SetOk(new List<SettingResponseModel>());
            }

            var result = entities.Select(x => _mapper.MapToSettingResponseModel(x)).ToList();

            for (int i = 0; i < result.Count; i++)
            {
                var model = result[i];
                var (createdAuthor, lastUpdateAuthor) = await _unitOfWork.AdminRepository.GetAuthors(model.CreatedBy, model.LastUpdatedBy);

                model.CreatedByCreator = UserHelper.ConvertAdminIdToAdminFullname(createdAuthor);
                model.LastUpdatedByUpdater = UserHelper.ConvertAdminIdToAdminFullname(lastUpdateAuthor);
            }

            return new ApiResponse().SetOk(result);
        }

        public async Task<ApiResponse> HardDeleteSettingByKeyName(string keyname)
        {
            var entity = await _unitOfWork.SettingRepository.GetAsync(x => x.KeyName == keyname);

            if (entity == null)
            {
                return new ApiResponse().SetNotFound(keyname, message: string.Format(ErrorMessages.PropertyDoesNotExist, keyname));
            }

            await _unitOfWork.SettingRepository.DeleteByIdAsync(entity.Id);
            await _unitOfWork.SaveChangeAsync();

            return new ApiResponse().SetOk(keyname);
        }

        public async Task<ApiResponse> GetAllDataTypeEnums()
        {
            var dataTypeEnums = EnumHelper.GetAllEnumDescriptions<SettingDataTypeEnum>();

            return new ApiResponse().SetOk(dataTypeEnums);
        }
    }
}
