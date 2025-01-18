using Dormy.WebService.Api.Core.Constants;
using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Core.Interfaces;
using Dormy.WebService.Api.Models.Constants;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;
using Dormy.WebService.Api.Presentation.Mappers;
using Dormy.WebService.Api.Startup;
using Microsoft.EntityFrameworkCore;

namespace Dormy.WebService.Api.ApplicationLogic
{
    public class WorkplaceService : IWorkplaceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContextService _userContextService;
        private readonly WorkplaceMapper _workplaceMapper;

        public WorkplaceService(IUnitOfWork unitOfWork, IUserContextService userContextService)
        {
            _unitOfWork = unitOfWork;
            _userContextService = userContextService;
            _workplaceMapper = new WorkplaceMapper();
        }

        public async Task<ApiResponse> CreateWorkplace(WorkplaceRequestModel model)
        {
            var entity = _workplaceMapper.MapToWorkplaceEntity(model);
            entity.CreatedBy = _userContextService.UserId;

            await _unitOfWork.WorkplaceRepository.AddAsync(entity);
            await _unitOfWork.SaveChangeAsync();

            return new ApiResponse().SetCreated(entity.Id);
        }

        public async Task<ApiResponse> GetAllWorkplace(int pageIndex = 1, int pageSize = 25)
        {
            var entities = new List<WorkplaceEntity>();

            var isAdmin = _userContextService.UserRoles.Any(x => x.Trim().ToLower().Equals(Role.ADMIN.ToLower()));
            if (isAdmin)
            {
                entities = await _unitOfWork.WorkplaceRepository
                    .GetAllAsync(
                        filter: x => true,
                        include: x => x.Include(workplace => workplace.Users),
                        pageIndex: pageIndex,
                        pageSize: pageSize,
                        isPaging: true);
            }
            else
            {
                entities = await _unitOfWork.WorkplaceRepository
                    .GetAllAsync(
                        filter: x => x.isDeleted == false,
                        include: x => x.Include(workplace => workplace.Users),
                        pageIndex: pageIndex,
                        pageSize: pageSize,
                        isPaging: true);
            }

            if (entities == null)
            {
                return new ApiResponse().SetOk(new List<WorkplaceResponseModel>());
            }

            var response = entities.Select(entity => _workplaceMapper.MapToWorkplaceResponseModel(entity)).ToList();

            for (var i = 0; i < response.Count; i++)
            {
                var workplace = response[i];
                var author = await _unitOfWork.AdminRepository.GetAsync(x => x.Id.Equals(workplace.CreatedBy));

                workplace.CreatedByAdminName = author?.UserName ?? string.Empty;

                if (workplace.LastUpdatedBy != null)
                {
                    var updatedAdmin = await _unitOfWork.AdminRepository.GetAsync(x => x.Id.Equals(workplace.LastUpdatedBy));
                    workplace.LastUpdatedByAdminName = updatedAdmin?.UserName ?? string.Empty;
                }
            }

            return new ApiResponse().SetOk(response);
        }

        public async Task<ApiResponse> GetSingleWorkplaceById(Guid id)
        {
            var entity = await _unitOfWork.WorkplaceRepository
                .GetAsync(
                    x => x.Id.Equals(id),
                    x => x.Include(workplace => workplace.Users));

            if (entity == null)
            {
                return new ApiResponse().SetNotFound(id);
            }

            var response = _workplaceMapper.MapToWorkplaceResponseModel(entity);

            var author = await _unitOfWork.AdminRepository.GetAsync(x => x.Id.Equals(response.CreatedBy));

            response.CreatedByAdminName = author?.UserName ?? string.Empty;

            if (entity.LastUpdatedBy != null)
            {
                var updatedAdmin = await _unitOfWork.AdminRepository.GetAsync(x => x.Id.Equals(response.LastUpdatedBy));
                response.LastUpdatedByAdminName = updatedAdmin?.UserName ?? string.Empty;
            }

            return new ApiResponse().SetOk(response);
        }

        public async Task<ApiResponse> GetWorkplaceBatch(List<Guid> ids)
        {
            var entities = await _unitOfWork.WorkplaceRepository
                 .GetAllAsync(
                     x => ids.Contains(x.Id),
                     x => x.Include(workplace => workplace.Users),
                    isPaging: false);

            if (entities == null)
            {
                return new ApiResponse().SetOk(new List<WorkplaceResponseModel>());
            }

            var response = entities.Select(entity => _workplaceMapper.MapToWorkplaceResponseModel(entity)).ToList();

            for (var i = 0; i < response.Count; i++)
            {
                var workplace = response[i];
                var author = await _unitOfWork.AdminRepository.GetAsync(x => x.Id.Equals(workplace.CreatedBy));

                workplace.CreatedByAdminName = author?.UserName ?? string.Empty;

                if (workplace.LastUpdatedBy != null)
                {
                    var updatedAdmin = await _unitOfWork.AdminRepository.GetAsync(x => x.Id.Equals(workplace.LastUpdatedBy));
                    workplace.LastUpdatedByAdminName = updatedAdmin?.UserName ?? string.Empty;
                }
            }

            return new ApiResponse().SetOk(response);
        }

        public async Task<ApiResponse> HardDeleteWorkplace(Guid id)
        {
            var entity = await _unitOfWork.WorkplaceRepository
                .GetAsync(
                    x => x.Id.Equals(id),
                    x => x.Include(x => x.Users));

            if (entity == null)
            {
                return new ApiResponse().SetNotFound(id);
            }

            if (entity.Users.Any())
            {
                return new ApiResponse().SetConflict(id, ErrorMessages.CanNotDeleteNotEmptyEntity);
            }

            await _unitOfWork.WorkplaceRepository.DeleteByIdAsync(entity.Id);

            await _unitOfWork.SaveChangeAsync();

            return new ApiResponse().SetOk(entity.Id);
        }

        public async Task<ApiResponse> SoftDeleteWorkplace(Guid id)
        {
            var entity = await _unitOfWork.WorkplaceRepository
                .GetAsync(
                    x => x.Id.Equals(id),
                    x => x.Include(x => x.Users));

            if (entity == null)
            {
                return new ApiResponse().SetNotFound(id);
            }

            if (entity.Users.Any())
            {
                return new ApiResponse().SetConflict(id, ErrorMessages.CanNotDeleteNotEmptyEntity);
            }

            entity.isDeleted = true;

            entity.LastUpdatedDateUtc = DateTime.UtcNow;
            entity.LastUpdatedBy = _userContextService.UserId;

            await _unitOfWork.SaveChangeAsync();

            return new ApiResponse().SetOk(entity.Id);
        }

        public async Task<ApiResponse> UpdateWorkplace(WorkplaceUpdateRequestModel model)
        {
            var entity = await _unitOfWork.WorkplaceRepository.GetAsync(x => x.Id.Equals(model.Id));

            if (entity == null)
            {
                return new ApiResponse().SetNotFound(model.Id);
            }

            entity.Abbrevation = model.Abbrevation;
            entity.Name = model.Name;
            entity.Address = model.Address;
            entity.LastUpdatedBy = _userContextService.UserId;
            entity.LastUpdatedDateUtc = DateTime.UtcNow;

            await _unitOfWork.SaveChangeAsync();
            return new ApiResponse().SetOk();
        }
    }
}
