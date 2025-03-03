using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Core.Interfaces;
using Dormy.WebService.Api.Models.Enums;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;
using Dormy.WebService.Api.Startup;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace Dormy.WebService.Api.ApplicationLogic
{
    public class ContractService : IContractService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContextService _userContextService;

        public ContractService(IUnitOfWork unitOfWork, IUserContextService userContextService)
        {
            _unitOfWork = unitOfWork;
            _userContextService = userContextService;
        }

        public async Task<ApiResponse> Register(RegisterRequestModel model)
        {
            if (model == null)
            {
                return new ApiResponse().SetBadRequest(null, "Model cannot be null");
            }

            var userId = _userContextService.UserId;

            if (userId == Guid.Empty)
            {
                return new ApiResponse().SetForbidden(null, "User not authenticated");
            }

            // Validate User
            if (model.User == null)
            {
                return new ApiResponse().SetBadRequest(null, "User information is required");
            }

            // Validate startDate cannot be after enddate
            if (model.StartDate > model.EndDate)
            {
                return new ApiResponse().SetUnprocessableEntity(null, "Start date cannot be after end date");
            }

            // Retrieve existing User entity
            var userEntity = await _unitOfWork.UserRepository.GetAsync(x => x.Id == userId);
            if (userEntity == null)
            {
                return new ApiResponse().SetNotFound(null, "User not found");
            }

            // Get user gender
            Enum.TryParse<GenderEnum>(model.User.Gender, true, out var gender);
            gender = gender == default ? GenderEnum.OTHER : gender;

            using (var scope = new TransactionScope(
                TransactionScopeOption.Required,
                new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                TransactionScopeAsyncFlowOption.Enabled))
            {
                var healthInsuranceId = Guid.NewGuid();

                // Check if there is any available room for user gender
                var roomAvailables = await _unitOfWork.RoomRepository
                    .GetAllAsync(x =>
                        x.RoomTypeId == model.RoomTypeId &&
                        x.Status == RoomStatusEnum.AVAILABLE &&
                        x.TotalAvailableBed > 0 &&
                        x.Building.GenderRestriction != gender,
                        x => x.Include(x => x.RoomType).Include(x => x.Building));

                if (roomAvailables.Count == 0)
                {
                    return new ApiResponse().SetUnprocessableEntity(null, "Out of available room");
                }

                // Check user have available contract
                var contracts = await _unitOfWork.ContractRepository.GetAllAsync(x =>
                    x.UserId == userId &&
                    x.EndDate >= DateTime.UtcNow &&
                    (x.Status == ContractStatusEnum.ACTIVE ||
                    x.Status == ContractStatusEnum.EXTENDED ||
                    x.Status == ContractStatusEnum.PENDING ||
                    x.Status == ContractStatusEnum.WAITING_PAYMENT));

                if (contracts.Count > 0)
                {
                    return new ApiResponse().SetBadRequest(null, "User is still have an available contract");
                }

                // Get the first available room
                var bookedRoom = await _unitOfWork.RoomRepository.GetAsync(x => x.Id == roomAvailables.First().Id);

                if (bookedRoom == null)
                {
                    return new ApiResponse().SetNotFound(null, "Room not found");
                }

                // Update room
                bookedRoom.Status = bookedRoom.TotalAvailableBed == bookedRoom.TotalUsedBed ? RoomStatusEnum.FULL : RoomStatusEnum.AVAILABLE;
                bookedRoom.LastUpdatedBy = userId;
                bookedRoom.LastUpdatedDateUtc = DateTime.UtcNow;
                bookedRoom.TotalAvailableBed = bookedRoom.TotalAvailableBed - 1;
                bookedRoom.TotalUsedBed = bookedRoom.TotalUsedBed + 1;
                await _unitOfWork.SaveChangeAsync();

                // Create Contract entity
                var contract = new ContractEntity()
                {
                    UserId = userId,
                    RoomId = bookedRoom.Id,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    SubmissionDate = DateTime.Now,
                    Status = ContractStatusEnum.PENDING,
                    CreatedBy = userId,
                    CreatedDateUtc = DateTime.Now,
                };

                await _unitOfWork.ContractRepository.AddAsync(contract);
                await _unitOfWork.SaveChangeAsync();

                if (model.WorkplaceId != null)
                {
                    var workplaceEntity = await _unitOfWork.WorkplaceRepository.GetAsync(x => x.Id == model.WorkplaceId);
                    if (workplaceEntity == null)
                    {
                        return new ApiResponse().SetNotFound(null, "Workplace not found");
                    }
                }

                // Create and add Guardian entity if provided
                if (model.Guardian != null)
                {
                    var guardianEntity = new GuardianEntity
                    {
                        Name = model.Guardian.Name,
                        Email = model.Guardian.Email,
                        PhoneNumber = model.Guardian.PhoneNumber,
                        Address = model.Guardian.Address,
                        RelationshipToUser = model.Guardian.RelationshipToUser,
                        UserId = userId,
                        CreatedBy = userId,
                        CreatedDateUtc = DateTime.UtcNow,
                    };
                    await _unitOfWork.GuardianRepository.AddAsync(guardianEntity);
                    await _unitOfWork.SaveChangeAsync();
                }

                // Create and add HealthInsurance entity if provided
                if (model.HealthInsurance != null)
                {
                    var healthInsuranceEntity = new HealthInsuranceEntity
                    {
                        Id = healthInsuranceId,
                        InsuranceCardNumber = model.HealthInsurance.InsuranceCardNumber,
                        RegisteredHospital = model.HealthInsurance.RegisteredHospital,
                        ExpirationDate = model.HealthInsurance.ExpirationDate,
                        CreatedBy = userId,
                        CreatedDateUtc = DateTime.UtcNow,
                    };
                    await _unitOfWork.HealthInsuranceRepository.AddAsync(healthInsuranceEntity);
                    await _unitOfWork.SaveChangeAsync();
                }

                // Update User entity
                userEntity.FirstName = model.User.FirstName;
                userEntity.LastName = model.User.LastName;
                userEntity.Email = model.User.Email;
                userEntity.DateOfBirth = model.User.DateOfBirth;
                userEntity.PhoneNumber = model.User.PhoneNumber;
                userEntity.NationalIdNumber = model.User.NationalIdNumber;
                userEntity.WorkplaceId = model.WorkplaceId;
                userEntity.Gender = gender;
                userEntity.HealthInsuranceId = model.HealthInsurance != null ? healthInsuranceId : null;
                userEntity.LastUpdatedBy = userId;
                userEntity.LastUpdatedDateUtc = DateTime.UtcNow;

                await _unitOfWork.SaveChangeAsync();

                // Create and add Vehicle entity if provided
                if (model.Vehicle != null)
                {
                    var vehicleEntity = new VehicleEntity
                    {
                        LicensePlate = model.Vehicle.LicensePlate,
                        VehicleType = model.Vehicle.VehicleType,
                        UserId = userId,
                        CreatedBy = userId,
                        CreatedDateUtc = DateTime.UtcNow,
                    };
                    await _unitOfWork.VehicleRepository.AddAsync(vehicleEntity);
                    await _unitOfWork.SaveChangeAsync();
                }

                // Complete transaction
                scope.Complete();
            }
            return new ApiResponse().SetOk();
        }
    }
}
