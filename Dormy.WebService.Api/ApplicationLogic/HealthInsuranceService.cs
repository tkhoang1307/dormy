﻿using Dormy.WebService.Api.Core.Constants;
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

            if (_userContextService.UserRoles.FirstOrDefault() == Role.USER && entity.CreatedBy != _userContextService.UserId)
            {
                return new ApiResponse().SetForbidden(message: ErrorMessages.AccountDoesNotHavePermission);
            }

            var healthInsuranceModel = _healthInsuranceMapper.MapToHealthInsuranceResponseModel(entity);

            var (createdAdmin, lastUpdatedAdmin) = await _unitOfWork.AdminRepository.GetAuthors(healthInsuranceModel.CreatedBy, healthInsuranceModel.LastUpdatedBy);
            var (createdUser, lastUpdatedUser) = await _unitOfWork.UserRepository.GetAuthors(healthInsuranceModel.CreatedBy, healthInsuranceModel.LastUpdatedBy);

            if (createdAdmin != null)
            {
                healthInsuranceModel.CreatedByAdminName = UserHelper.ConvertAdminIdToAdminFullname(createdAdmin);
            }
            else
            {
                if (createdUser != null)
                {
                    healthInsuranceModel.CreatedByAdminName = UserHelper.ConvertUserIdToUserFullname(createdUser);
                }
            }

            if (lastUpdatedAdmin != null)
            {
                healthInsuranceModel.LastUpdatedByAdminName = UserHelper.ConvertAdminIdToAdminFullname(lastUpdatedAdmin);
            }
            else
            {
                if (lastUpdatedUser != null)
                {
                    healthInsuranceModel.LastUpdatedByAdminName = UserHelper.ConvertUserIdToUserFullname(lastUpdatedUser);
                }
            }

            return new ApiResponse().SetOk(healthInsuranceModel);
        }

        public async Task<ApiResponse> GetHealthInsuranceBatch(GetBatchRequestModel model)
        {
            var entities = new List<HealthInsuranceEntity>();

            if (model.IsGetAll)
            {
                entities = await _unitOfWork.HealthInsuranceRepository.GetAllAsync(x => true);
            }
            else
            {
                entities = await _unitOfWork.HealthInsuranceRepository.GetAllAsync(x => model.Ids.Contains(x.Id));
            }

            var healthInsuranceModels = entities.Select(x => _healthInsuranceMapper.MapToHealthInsuranceResponseModel(x)).ToList();

            for (int i = 0; i < healthInsuranceModels.Count; i++)
            {
                var healthInsuranceModel = healthInsuranceModels[i];

                var (createdAdmin, lastUpdatedAdmin) = await _unitOfWork.AdminRepository.GetAuthors(healthInsuranceModel.CreatedBy, healthInsuranceModel.LastUpdatedBy);
                var (createdUser, lastUpdatedUser) = await _unitOfWork.UserRepository.GetAuthors(healthInsuranceModel.CreatedBy, healthInsuranceModel.LastUpdatedBy);

                if (createdAdmin != null)
                {
                    healthInsuranceModel.CreatedByAdminName = UserHelper.ConvertAdminIdToAdminFullname(createdAdmin);
                }
                else
                {
                    if (createdUser != null)
                    {
                        healthInsuranceModel.CreatedByAdminName = UserHelper.ConvertUserIdToUserFullname(createdUser);
                    }
                }

                if (lastUpdatedAdmin != null)
                {
                    healthInsuranceModel.LastUpdatedByAdminName = UserHelper.ConvertAdminIdToAdminFullname(lastUpdatedAdmin);
                }
                else
                {
                    if (lastUpdatedUser != null)
                    {
                        healthInsuranceModel.LastUpdatedByAdminName = UserHelper.ConvertUserIdToUserFullname(lastUpdatedUser);
                    }
                }
            }

            return new ApiResponse().SetOk(healthInsuranceModels);
        }

        public async Task<ApiResponse> UpdateHealthInsurance(HealthInsuranceUpdationRequestModel model)
        {
            var entity = await _unitOfWork.HealthInsuranceRepository.GetAsync(x => x.Id == model.Id,
                                include: healthInsurance => healthInsurance.Include(healthInsurance => healthInsurance.User));

            if (entity == null)
            {
                return new ApiResponse().SetNotFound(model.Id, message: string.Format(ErrorMessages.PropertyDoesNotExist, "Health insurance"));
            }

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
