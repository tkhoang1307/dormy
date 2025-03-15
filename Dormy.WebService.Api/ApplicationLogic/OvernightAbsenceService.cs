using Dormy.WebService.Api.Core.Constants;
using Dormy.WebService.Api.Core.CustomExceptions;
using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Core.Interfaces;
using Dormy.WebService.Api.Models.Constants;
using Dormy.WebService.Api.Models.Enums;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;
using Dormy.WebService.Api.Presentation.Validations;
using Dormy.WebService.Api.Startup;
using Microsoft.EntityFrameworkCore;

namespace Dormy.WebService.Api.ApplicationLogic
{
    public class OvernightAbsenceService : IOvernightAbsenceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContextService _userContextService;

        public OvernightAbsenceService(IUnitOfWork unitOfWork, IUserContextService userContextService)
        {
            _unitOfWork = unitOfWork;
            _userContextService = userContextService;
        }

        public async Task<ApiResponse> AddOvernightAbsence(OvernightAbsentRequestModel model)
        {
            var userId = _userContextService.UserId;
            if (userId == Guid.Empty)
            {
                throw new EntityNotFoundException(message: "User not found");
            }

            var entity = new OvernightAbsenceEntity
            {
                UserId = userId,
                Reason = model.Reason,
                StartDateTime = model.StartDateTime,
                EndDateTime = model.EndDateTime,
                Status = OvernightAbsenceStatusEnum.SUBMITTED,
                CreatedBy = userId,
                LastUpdatedBy = userId,
                CreatedDateUtc = DateTime.UtcNow,
                LastUpdatedDateUtc = DateTime.UtcNow,
            };

            await _unitOfWork.OvernightAbsenceRepository.AddAsync(entity);
            await _unitOfWork.SaveChangeAsync();

            return new ApiResponse().SetCreated(entity.Id);
        }

        public async Task<ApiResponse> GetDetailOvernightAbsence(Guid id)
        {
            var entity = await _unitOfWork.OvernightAbsenceRepository.GetAsync(x => x.Id == id, x => x.Include(x => x.User), isNoTracking: true);
            if (entity == null)
            {
                return new ApiResponse().SetNotFound(id, message: string.Format(ErrorMessages.PropertyDoesNotExist, "Overnight absence"));
            }

            var result = new OvernightAbsentModel
            {
                Id = entity.Id,
                UserId = entity.UserId,
                Reason = entity.Reason,
                StartDateTime = entity.StartDateTime,
                EndDateTime = entity.EndDateTime,
                Status = entity.Status.ToString(),
                FirstName = entity.User.FirstName,
                LastName = entity.User.LastName,
                Email = entity.User.Email,
                UserName = entity.User.UserName,
                DateOfBirth = entity.User.DateOfBirth,
                PhoneNumber = entity.User.PhoneNumber,
                NationalIdNumber = entity.User.NationalIdNumber,
                IsDeleted = entity.IsDeleted
            };

            return new ApiResponse().SetOk(result);
        }

        public async Task<ApiResponse> GetOvernightAbsenceBatch(GetBatchRequestModel model)
        {
            var userId = _userContextService.UserId;
            if (userId == Guid.Empty)
            {
                return new ApiResponse().SetNotFound(message: "User not found");
            }

            var entities = new List<OvernightAbsenceEntity>();

            if (_userContextService.UserRoles.Contains(Role.ADMIN))
            {
                if (model.IsGetAll)
                {
                    entities =
                    await _unitOfWork.OvernightAbsenceRepository
                    .GetAllAsync(x => true, x => x
                        .Include(x => x.User),
                    isNoTracking: true);
                }
                else
                {
                    entities =
                    await _unitOfWork.OvernightAbsenceRepository
                    .GetAllAsync(x => model.Ids.Contains(x.Id), x => x
                        .Include(x => x.User),
                    isNoTracking: true);
                }
            }
            else
            {
                if (model.IsGetAll)
                {
                    entities =
                    await _unitOfWork.OvernightAbsenceRepository
                    .GetAllAsync(x => x.UserId == userId, x => x
                        .Include(x => x.User),
                    isNoTracking: true);
                }
                else
                {
                    entities =
                    await _unitOfWork.OvernightAbsenceRepository
                    .GetAllAsync(x => x.UserId == userId && model.Ids.Contains(x.Id), x => x
                        .Include(x => x.User),
                    isNoTracking: true);
                }
            }

            if (!model.IsGetAll)
            {
                if (entities.Count != model.Ids.Count)
                {
                    // Find the missing request IDs
                    var foundRequestIds = entities.Select(r => r.Id).ToList();
                    var missingRequestIds = model.Ids.Except(foundRequestIds).ToList();

                    // Return with error message listing the missing request IDs
                    var errorMessage = $"Overnight Absence(s) not found: {string.Join(", ", missingRequestIds)}";
                    return new ApiResponse().SetNotFound(message: errorMessage);
                }
            }

            var response = entities.Select(entity => new OvernightAbsentModel
            {
                Id = entity.Id,
                UserId = entity.UserId,
                Reason = entity.Reason,
                StartDateTime = entity.StartDateTime,
                EndDateTime = entity.EndDateTime,
                Status = entity.Status.ToString(),
                FirstName = entity.User.FirstName,
                LastName = entity.User.LastName,
                Email = entity.User.Email,
                UserName = entity.User.UserName,
                DateOfBirth = entity.User.DateOfBirth,
                PhoneNumber = entity.User.PhoneNumber,
                NationalIdNumber = entity.User.NationalIdNumber,
                IsDeleted = entity.IsDeleted
            }).ToList();

            return new ApiResponse().SetOk(response);
        }

        public async Task<ApiResponse> SoftDeleteOvernightAbsence(Guid id)
        {
            var entity = await _unitOfWork.OvernightAbsenceRepository.GetAsync(x => x.Id == id, isNoTracking: false);
            if (entity == null)
            {
                return new ApiResponse().SetNotFound(id, message: string.Format(ErrorMessages.PropertyDoesNotExist, "Overnight absence"));
            }

            entity.IsDeleted = true;
            entity.LastUpdatedBy = _userContextService.UserId;
            entity.LastUpdatedDateUtc = DateTime.UtcNow;
            await _unitOfWork.SaveChangeAsync();

            return new ApiResponse().SetAccepted(entity.Id);
        }

        public async Task<ApiResponse> UpdateOvernightAbsence(OvernightAbsentUpdationRequestModel model)
        {
            var entity = await _unitOfWork.OvernightAbsenceRepository.GetAsync(x => x.Id == model.Id, isNoTracking: false);
            if (entity == null)
            {
                return new ApiResponse().SetNotFound(model.Id, message: string.Format(ErrorMessages.PropertyDoesNotExist, "Overnight absence"));
            }

            if (_userContextService.UserId != entity.UserId)
            {
                return new ApiResponse().SetForbidden(message: string.Format(ErrorMessages.AccountDoesNotHavePermissionEntity, "overnight absence"));
            }

            if (entity.Status != OvernightAbsenceStatusEnum.SUBMITTED)
            {
                return new ApiResponse().SetBadRequest(message: string.Format(ErrorMessages.UpdateEntityConflict, "overnight absence"));
            }

            entity.Reason = model.Reason;
            entity.StartDateTime = model.StartDateTime;
            entity.EndDateTime = model.EndDateTime;
            entity.LastUpdatedBy = _userContextService.UserId;
            entity.LastUpdatedDateUtc = DateTime.UtcNow;

            await _unitOfWork.SaveChangeAsync();

            return new ApiResponse().SetOk(entity.Id);
        }

        public async Task<ApiResponse> UpdateStatusOvernightAbsence(Guid id, OvernightAbsenceStatusEnum status)
        {
            var entity = await _unitOfWork.OvernightAbsenceRepository.GetAsync(x => x.Id == id, isNoTracking: false);
            if (entity == null)
            {
                return new ApiResponse().SetNotFound(id, message: string.Format(ErrorMessages.PropertyDoesNotExist, "Overnight absence"));
            }

            if (_userContextService.UserRoles.Contains(Role.USER) && _userContextService.UserId !=  entity.UserId)
            {
                return new ApiResponse().SetForbidden(message: string.Format(ErrorMessages.AccountDoesNotHavePermissionEntity, "overnight absence"));
            }    

            var (isError, errorMessage) = OvernightAbsenceStatusChangeValidator.VerifyOvernightAbsenceStatusChangeValidator(entity.Status, status);
            if (isError)
            {
                return new ApiResponse().SetConflict(id, message: string.Format(errorMessage, "Overnight absence"));
            }

            if (status == OvernightAbsenceStatusEnum.APPROVED || status == OvernightAbsenceStatusEnum.REJECTED)
            {
                entity.ApproverId = _userContextService.UserId;
            }

            entity.Status = status;
            entity.LastUpdatedDateUtc = DateTime.UtcNow;
            entity.LastUpdatedBy = _userContextService.UserId;
            await _unitOfWork.SaveChangeAsync();

            return new ApiResponse().SetAccepted(entity.Id);
        }
    }
}
