using Dormy.WebService.Api.Core.CustomExceptions;
using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Core.Interfaces;
using Dormy.WebService.Api.Models.Constants;
using Dormy.WebService.Api.Models.Enums;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;
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

            VerifyDates(model.StartDateTime, model.EndDateTime);

            var entity = new OvernightAbsenceEntity
            {
                UserId = userId,
                Reason = model.Reason,
                StartDateTime = model.StartDateTime,
                EndDateTime = model.EndDateTime,
                Status = OvernightAbsenceStatusEnum.SUBMITTED,
                CreatedBy = userId,
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
                throw new EntityNotFoundException(message: "Overnight absence not found");
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
                throw new EntityNotFoundException(message: "Overnight absence not found");
            }

            entity.IsDeleted = true;
            entity.LastUpdatedBy = _userContextService.UserId;
            entity.LastUpdatedDateUtc = DateTime.UtcNow;
            await _unitOfWork.SaveChangeAsync();

            return new ApiResponse().SetAccepted(entity.Id);
        }

        public async Task<ApiResponse> UpdateOvernightAbsence(Guid id, OvernightAbsentRequestModel model)
        {
            var entity = await _unitOfWork.OvernightAbsenceRepository.GetAsync(x => x.Id == id, isNoTracking: false);
            if (entity == null)
            {
                throw new EntityNotFoundException(message: "Overnight absence not found");
            }

            VerifyDates(model.StartDateTime, model.EndDateTime);

            if (entity.Status != OvernightAbsenceStatusEnum.SUBMITTED)
            {
                throw new BadRequestException(message: "Overnight absence cannot be updated because it is not in submitted status");
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
                throw new EntityNotFoundException(message: "Overnight absence not found");
            }

            switch (status)
            {
                case OvernightAbsenceStatusEnum.APPROVED:
                case OvernightAbsenceStatusEnum.REJECTED:
                    {
                        if (entity.Status != OvernightAbsenceStatusEnum.SUBMITTED)
                        {
                            throw new BadRequestException(message: "Overnight absence is not available for approve/ reject");
                        }
                    }
                    break;
                case OvernightAbsenceStatusEnum.CANCELLED:
                    {
                        if (entity.Status == OvernightAbsenceStatusEnum.APPROVED || entity.Status == OvernightAbsenceStatusEnum.REJECTED)
                        {
                            throw new BadRequestException(message: "Overnight absence is already approved or rejected");
                        }
                    }
                    break;
            }

            entity.Status = status;
            entity.LastUpdatedDateUtc = DateTime.UtcNow;
            entity.LastUpdatedBy = _userContextService.UserId;
            await _unitOfWork.SaveChangeAsync();

            return new ApiResponse().SetAccepted(entity.Id);
        }

        private void VerifyDates(DateTime startDateTime, DateTime endDateTime)
        {
            if (startDateTime < DateTime.Now)
            {
                throw new BadRequestException(message: "Start date time must be in the future");
            }

            if (endDateTime < DateTime.Now)
            {
                throw new BadRequestException(message: "End date time must be in the future");
            }

            if (startDateTime > endDateTime)
            {
                throw new BadRequestException(message: "Start date time must be before end date time");
            }
        }
    }
}
