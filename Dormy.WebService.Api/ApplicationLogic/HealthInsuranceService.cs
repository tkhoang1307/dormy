using Dormy.WebService.Api.Core.Constants;
using Dormy.WebService.Api.Core.Interfaces;
using Dormy.WebService.Api.Core.Utilities;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;
using Dormy.WebService.Api.Presentation.Mappers;
using Dormy.WebService.Api.Startup;
using Microsoft.EntityFrameworkCore;

namespace Dormy.WebService.Api.ApplicationLogic
{
    public class HealthInsuranceService : IHealthInsuranceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContextService _userContextService;
        private HealthInsuranceMapper _healthInsuranceMapper;

        public HealthInsuranceService(IUnitOfWork unitOfWork, IUserContextService userContextService)
        {
            _unitOfWork = unitOfWork;
            _healthInsuranceMapper = new HealthInsuranceMapper();
            _userContextService = userContextService;
        }

        public async Task<ApiResponse> AddHealthInsurance(HealthInsuranceRequestModel model)
        {
            var entity = _healthInsuranceMapper.MapToHealthInsuranceEntity(model);

            entity.CreatedBy = _userContextService.UserId;
            entity.LastUpdatedBy = _userContextService.UserId;

            await _unitOfWork.HealthInsuranceRepository.AddAsync(entity);
            await _unitOfWork.SaveChangeAsync();
            return new ApiResponse().SetCreated(entity.Id);
        }

        public async Task<ApiResponse> GetDetailHealthInsurance(Guid id)
        {
            var entity = await _unitOfWork.HealthInsuranceRepository.GetAsync(x => x.Id == id, 
                                include: x => x.Include(healthInsurance => healthInsurance.User));

            if (entity == null)
            {
                return new ApiResponse().SetNotFound(id, message: string.Format(ErrorMessages.PropertyDoesNotExist, "Health insurance"));
            }

            //if (entity.User.Id != _userContextService.UserId && _userContextService.UserRoles.FirstOrDefault() == "User")
            //{
            //    return new ApiResponse().SetForbidden(id, message: ErrorMessages.AccountDoesNotHavePermission);
            //}

            var healthInsuranceModel = _healthInsuranceMapper.MapToHealthInsuranceResponseModel(entity);

            var (createdUser, lastUpdatedUser) = await _unitOfWork.AdminRepository.GetAuthors(healthInsuranceModel.CreatedBy, healthInsuranceModel.LastUpdatedBy);

            healthInsuranceModel.CreatedByAdminName = UserHelper.ConvertAdminIdToAdminFullname(createdUser);
            healthInsuranceModel.LastUpdatedByAdminName = UserHelper.ConvertAdminIdToAdminFullname(lastUpdatedUser);

            return new ApiResponse().SetOk(healthInsuranceModel);
        }

        public async Task<ApiResponse> UpdateHealthInsurance(HealthInsuranceUpdationRequestModel model)
        {
            var entity = await _unitOfWork.HealthInsuranceRepository.GetAsync(x => x.Id == model.Id,
                                include: healthInsurance => healthInsurance.Include(healthInsurance => healthInsurance.User));

            if (entity == null)
            {
                return new ApiResponse().SetNotFound(model.Id, message: string.Format(ErrorMessages.PropertyDoesNotExist, "Health insurance"));
            }

            //if (entity.User.Id != _userContextService.UserId && _userContextService.UserRoles.FirstOrDefault() == "User")
            //{
            //    return new ApiResponse().SetForbidden(model.Id, message: ErrorMessages.AccountDoesNotHavePermission);
            //}

            entity.InsuranceCardNumber = model.InsuranceCardNumber;
            entity.RegisteredHospital = model.RegisteredHospital;
            entity.ExpirationDate = model.ExpirationDate;
            entity.LastUpdatedBy = _userContextService.UserId;
            entity.LastUpdatedDateUtc = DateTime.UtcNow;

            await _unitOfWork.SaveChangeAsync();

            return new ApiResponse().SetAccepted(entity.Id);
        }
    }
}
