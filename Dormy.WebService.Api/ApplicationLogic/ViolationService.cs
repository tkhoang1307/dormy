using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Core.Interfaces;
using Dormy.WebService.Api.Models.Constants;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;
using Dormy.WebService.Api.Startup;
using Microsoft.EntityFrameworkCore;

namespace Dormy.WebService.Api.ApplicationLogic
{
    public class ViolationService : IViolationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContextService _userContextService;

        public ViolationService(IUnitOfWork unitOfWork, IUserContextService userContextService)
        {
            _unitOfWork = unitOfWork;
            _userContextService = userContextService;
        }

        public async Task<ApiResponse> CreateViolation(ViolationRequestModel model)
        {
            // check user exist, then save the violation
            var userEntity = await _unitOfWork.UserRepository.GetAsync(x => x.Id == model.UserId);
            if (userEntity == null)
            {
                return new ApiResponse().SetNotFound("User not found");
            }

            var violationEntity = new ViolationEntity
            {
                UserId = model.UserId,
                Description = model.Description,
                Penalty = model.Penalty,
                ViolationDate = model.ViolationDate,
                CreatedBy = _userContextService.UserId,
                CreatedDateUtc = DateTime.UtcNow,
            };

            await _unitOfWork.ViolationRepository.AddAsync(violationEntity);
            await _unitOfWork.SaveChangeAsync();

            return new ApiResponse().SetCreated(violationEntity.Id);
        }

        public async Task<ApiResponse> GetSingleViolation(Guid id)
        {
            var violationEntity = await _unitOfWork.ViolationRepository.GetAsync(x => x.Id == id, x => x.Include(x => x.User), isNoTracking: true);
            if (violationEntity == null)
            {
                return new ApiResponse().SetNotFound("Violation not found");
            }

            var result = new ViolationResponseModel
            {
                Id = violationEntity.Id,
                UserId = violationEntity.UserId,
                Description = violationEntity.Description,
                Penalty = violationEntity.Penalty,
                ViolationDate = violationEntity.ViolationDate,
                DateOfBirth = violationEntity.User.DateOfBirth,
                FirstName = violationEntity.User.FirstName,
                LastName = violationEntity.User.LastName,
                Email = violationEntity.User.Email,
                UserName = violationEntity.User.UserName,
                PhoneNumber = violationEntity.User.PhoneNumber,
                NationalIdNumber = violationEntity.User.NationalIdNumber,
                IsDeleted = violationEntity.IsDeleted
            };

            return new ApiResponse().SetOk(result);
        }

        public async Task<ApiResponse> GetViolationBatch(GetBatchRequestModel model)
        {

            List<ViolationEntity> violationEntities = [];

            if (_userContextService.UserRoles.Contains(Role.ADMIN))
            {
                if (model.IsGetAll)
                {
                    violationEntities = await _unitOfWork.ViolationRepository.GetAllAsync(x => true, x => x.Include(x => x.User), isNoTracking: true);
                }
                else
                {
                    violationEntities = await _unitOfWork.ViolationRepository.GetAllAsync(x => model.Ids.Contains(x.Id), x => x.Include(x => x.User), isNoTracking: true);
                }
            }
            else
            {
                if (model.IsGetAll)
                {
                    violationEntities = await _unitOfWork.ViolationRepository.GetAllAsync(x => x.UserId == _userContextService.UserId, x => x.Include(x => x.User), isNoTracking: true);
                }
                else
                {
                    violationEntities = await _unitOfWork.ViolationRepository.GetAllAsync(x => x.UserId == _userContextService.UserId && model.Ids.Contains(x.Id), x => x.Include(x => x.User), isNoTracking: true);
                }
            }

            if (!model.IsGetAll)
            {
                if (violationEntities.Count != model.Ids.Count)
                {
                    // Find the missing request IDs
                    var foundRequestIds = violationEntities.Select(r => r.Id).ToList();
                    var missingRequestIds = model.Ids.Except(foundRequestIds).ToList();

                    // Return with error message listing the missing request IDs
                    var errorMessage = $"Violation(s) not found: {string.Join(", ", missingRequestIds)}";
                    return new ApiResponse().SetNotFound(message: errorMessage);
                }
            }

            var result = violationEntities.Select(violationEntity => new ViolationResponseModel
            {
                Id = violationEntity.Id,
                UserId = violationEntity.UserId,
                Description = violationEntity.Description,
                Penalty = violationEntity.Penalty,
                ViolationDate = violationEntity.ViolationDate,
                DateOfBirth = violationEntity.User.DateOfBirth,
                FirstName = violationEntity.User.FirstName,
                LastName = violationEntity.User.LastName,
                Email = violationEntity.User.Email,
                UserName = violationEntity.User.UserName,
                PhoneNumber = violationEntity.User.PhoneNumber,
                NationalIdNumber = violationEntity.User.NationalIdNumber,
                IsDeleted = violationEntity.IsDeleted
            }).ToList();

            return new ApiResponse().SetOk(result);
        }

        public async Task<ApiResponse> SoftDeleteViolation(Guid id)
        {
            var violationEntity = await _unitOfWork.ViolationRepository.GetAsync(x => x.Id == id);
            if (violationEntity == null)
            {
                return new ApiResponse().SetNotFound("Violation not found");
            }

            violationEntity.IsDeleted = true;
            violationEntity.LastUpdatedBy = _userContextService.UserId;
            violationEntity.LastUpdatedDateUtc = DateTime.UtcNow;

            await _unitOfWork.SaveChangeAsync();

            return new ApiResponse().SetOk();
        }

        public async Task<ApiResponse> UpdateViolation(Guid id, ViolationRequestModel model)
        {
            var violationEntity = await _unitOfWork.ViolationRepository.GetAsync(x => x.Id == id);
            if (violationEntity == null)
            {
                return new ApiResponse().SetNotFound("Violation not found");
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

            await _unitOfWork.SaveChangeAsync();

            return new ApiResponse().SetOk();
        }
    }
}
