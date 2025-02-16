using Dormy.WebService.Api.Core.Constants;
using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Core.Interfaces;
using Dormy.WebService.Api.Core.Utilities;
using Dormy.WebService.Api.Models.Constants;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;
using Dormy.WebService.Api.Presentation.Mappers;
using Dormy.WebService.Api.Startup;
using Microsoft.EntityFrameworkCore;

namespace Dormy.WebService.Api.ApplicationLogic
{
    public class GuardianService : IGuardianService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContextService _userContextService;
        private GuardianMapper _guardianMapper;

        public GuardianService(IUnitOfWork unitOfWork, IUserContextService userContextService)
        {
            _unitOfWork = unitOfWork;
            _guardianMapper = new GuardianMapper();
            _userContextService = userContextService;
        }

        public async Task<ApiResponse> AddNewGuardian(GuardianRequestModel model)
        {
            var entity = _guardianMapper.MapToGuardianEntity(model);

            entity.UserId = _userContextService.UserId;
            entity.CreatedBy = _userContextService.UserId;
            entity.LastUpdatedBy = _userContextService.UserId;

            await _unitOfWork.GuardianRepository.AddAsync(entity);
            await _unitOfWork.SaveChangeAsync();
            return new ApiResponse().SetCreated(entity.Id);
        }

        public async Task<ApiResponse> GetAllGuardiansOfUser()
        {
            var entities = await _unitOfWork.GuardianRepository.GetAllAsync(x => x.UserId == _userContextService.UserId);

            var guardianModels = new List<GuardianResponseModel>();

            for (int i = 0; i < guardianModels.Count; i++)
            {
                var guardian = guardianModels[i];

                var (createdUser, lastUpdatedUser) = await _unitOfWork.UserRepository.GetAuthors(guardian.CreatedBy, guardian.LastUpdatedBy);

                guardian.CreatedByUserFullName = UserHelper.ConvertUserIdToUserFullname(createdUser);
                guardian.LastUpdatedByUserFullName = UserHelper.ConvertUserIdToUserFullname(lastUpdatedUser);
            }

            return new ApiResponse().SetOk(guardianModels);
        }

        public async Task<ApiResponse> GetDetailGuardianById(Guid id)
        {
            var entity = await _unitOfWork.GuardianRepository.GetAsync(x => x.Id == id);

            if (entity == null)
            {
                return new ApiResponse().SetNotFound(id, message: string.Format(ErrorMessages.PropertyDoesNotExist, "Guardian"));
            }

            if (_userContextService.UserRoles.FirstOrDefault() == Role.USER && entity.UserId != _userContextService.UserId)
            {
                return new ApiResponse().SetForbidden(message: ErrorMessages.AccountDoesNotHavePermission);
            }

            var guardianModel = _guardianMapper.MapToGuardianResponseModel(entity);

            var (createdUser, lastUpdatedUser) = await _unitOfWork.UserRepository.GetAuthors(guardianModel.CreatedBy, guardianModel.LastUpdatedBy);

            guardianModel.CreatedByUserFullName = UserHelper.ConvertUserIdToUserFullname(createdUser);
            guardianModel.LastUpdatedByUserFullName = UserHelper.ConvertUserIdToUserFullname(lastUpdatedUser);

            return new ApiResponse().SetOk(guardianModel);
        }

        public async Task<ApiResponse> UpdateGuardian(GuardianUpdationRequestModel model)
        {
            var entity = await _unitOfWork.GuardianRepository.GetAsync(x => x.Id == model.Id);

            if (entity == null)
            {
                return new ApiResponse().SetNotFound(model.Id, message: string.Format(ErrorMessages.PropertyDoesNotExist, "Guardian"));
            }

            if (entity.UserId != _userContextService.UserId)
            {
                return new ApiResponse().SetForbidden(message: ErrorMessages.AccountDoesNotHavePermission);
            }  

            entity.Name = model.Name;
            entity.Email = model.Email;
            entity.PhoneNumber = model.PhoneNumber;
            entity.Address = model.Address;
            entity.RelationshipToUser = model.RelationshipToUser;
            entity.LastUpdatedBy = _userContextService.UserId;
            entity.LastUpdatedDateUtc = DateTime.UtcNow;

            await _unitOfWork.SaveChangeAsync();

            return new ApiResponse().SetAccepted(entity.Id);
        }

        public async Task<ApiResponse> HardDeleteParkingSpot(Guid id)
        {
            var entity = await _unitOfWork.GuardianRepository
                .GetAsync(
                    x => x.Id.Equals(id),
                    x => x.Include(x => x.User));

            if (entity == null)
            {
                return new ApiResponse().SetNotFound(id, message: string.Format(ErrorMessages.PropertyDoesNotExist, "Guardian"));
            }

            if (_userContextService.UserRoles.FirstOrDefault() == Role.USER && entity.UserId != _userContextService.UserId)
            {
                return new ApiResponse().SetForbidden(message: ErrorMessages.AccountDoesNotHavePermission);
            }

            await _unitOfWork.GuardianRepository.DeleteByIdAsync(id);

            await _unitOfWork.SaveChangeAsync();

            return new ApiResponse().SetOk(id);
        }

        public async Task<ApiResponse> GetGuardianBatch(GetBatchGuardianRequestModel model)
        {
            var entities = new List<GuardianEntity>();

            if (model.IsGetAll)
            {
                entities = await _unitOfWork.GuardianRepository.GetAllAsync(x => true);
            }
            else
            {
                if (model.UserId != null)
                {
                    entities = await _unitOfWork.GuardianRepository.GetAllAsync(x => x.UserId == model.UserId);
                }
                else
                {
                    entities = await _unitOfWork.GuardianRepository.GetAllAsync(x => model.Ids.Contains(x.Id));

                }
            }

            var guardianModels = entities.Select(x => _guardianMapper.MapToGuardianResponseModel(x)).ToList();

            for (int i = 0; i < guardianModels.Count; i++)
            {
                var guardian = guardianModels[i];

                var (createdUser, lastUpdatedUser) = await _unitOfWork.UserRepository.GetAuthors(guardian.CreatedBy, guardian.LastUpdatedBy);

                guardian.CreatedByUserFullName = UserHelper.ConvertUserIdToUserFullname(createdUser);
                guardian.LastUpdatedByUserFullName = UserHelper.ConvertUserIdToUserFullname(lastUpdatedUser);
            }

            return new ApiResponse().SetOk(guardianModels);
        }
    }
}
