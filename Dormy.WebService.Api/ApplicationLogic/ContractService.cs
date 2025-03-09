using Dormy.WebService.Api.Core.Constants;
using Dormy.WebService.Api.Core.CustomExceptions;
using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Core.Interfaces;
using Dormy.WebService.Api.Models.Constants;
using Dormy.WebService.Api.Models.Enums;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;
using Dormy.WebService.Api.Presentation.Mappers;
using Dormy.WebService.Api.Startup;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace Dormy.WebService.Api.ApplicationLogic
{
    public class ContractService : IContractService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContextService _userContextService;
        private readonly IRoomTypeService _roomTypeService;
        private readonly UserMapper _userMapper;
        private readonly IUserService _userService;
        private readonly IHealthInsuranceService _healthInsuranceService;
        private readonly IGuardianService _guardianService;
        private readonly WorkplaceMapper _workplaceMapper;
        private readonly RoomTypeMapper _roomTypeMapper;
        private readonly ContractMapper _contractMapper;

        public ContractService(IUnitOfWork unitOfWork, 
                               IUserContextService userContextService, 
                               IRoomTypeService roomTypeService,
                               IUserService userService,
                               IHealthInsuranceService healthInsuranceService,
                               IGuardianService guardianService)
        {
            _unitOfWork = unitOfWork;
            _userContextService = userContextService;
            _roomTypeService = roomTypeService;
            _userService = userService;
            _healthInsuranceService =healthInsuranceService;
            _guardianService =guardianService;
            _userMapper = new UserMapper();
            _workplaceMapper = new WorkplaceMapper();
            _roomTypeMapper = new RoomTypeMapper();
            _contractMapper = new ContractMapper();
        }

        public async Task<ApiResponse> Register(RegisterRequestModel model)
        {
            if (model == null)
            {
                return new ApiResponse().SetBadRequest(null, "Model cannot be null");
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

            // Get user gender
            Enum.TryParse<GenderEnum>(model.User.Gender, true, out var gender);
            gender = gender == default ? GenderEnum.OTHER : gender;
            Guid userIdTracking = Guid.Empty;
            Guid healthInsuranceIdTracking = Guid.Empty;
            List<Guid> guardianIdsTracking = new List<Guid>();
            Guid contractIdTracking = Guid.Empty;
            Guid invoiceIdTracking = Guid.Empty;

            using (var scope = new TransactionScope(
                TransactionScopeOption.Required,
                new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                TransactionScopeAsyncFlowOption.Enabled))
            {
                //Create User
                var userCreationRequestModel = new UserRequestModel()
                {
                    FirstName = model.User.FirstName,
                    LastName = model.User.LastName,
                    Email = model.User.Email,
                    UserName = "userTest1",
                    Password = "userTest1",
                    DateOfBirth = model.User.DateOfBirth,
                    PhoneNumber = model.User.PhoneNumber,
                    NationalIdNumber = model.User.NationalIdNumber,
                    Gender = model.User.Gender,
                };

                var responseCreateUser = await _userService.SignUp(userCreationRequestModel);
                if (!responseCreateUser.IsSuccess)
                {
                    return responseCreateUser;
                }    
                userIdTracking = (Guid)responseCreateUser.Result;
                var userEntity = await _unitOfWork.UserRepository.GetAsync(x => x.Id.Equals(userIdTracking));

                //Sign in to have userContext
                var responseLogin = await _userService.Login(new LoginRequestModel()
                {
                    Username = userCreationRequestModel.UserName,
                    Password = userCreationRequestModel.Password,
                });

                // Check if user have any workplace
                if (model.WorkplaceId != null)
                {
                    var workplaceEntity = await _unitOfWork.WorkplaceRepository.GetAsync(x => x.Id == model.WorkplaceId);
                    if (workplaceEntity == null)
                    {
                        return new ApiResponse().SetNotFound(message: "Workplace not found");
                    }

                    userEntity.WorkplaceId = model.WorkplaceId;
                }

                // Add HealthInsurance entity if provided
                if (model.HealthInsurance != null)
                {
                    var responseCreateHealthInsurance = await _healthInsuranceService.AddHealthInsurance(model.HealthInsurance);
                    if (!responseCreateHealthInsurance.IsSuccess)
                    {
                        return responseCreateHealthInsurance;
                    }
                    healthInsuranceIdTracking = (Guid)responseCreateHealthInsurance.Result;
                    userEntity.HealthInsuranceId = healthInsuranceIdTracking;
                }

                // Add Guardians entity if provided
                if (model.Guardians != null && model.Guardians.Count > 0)
                {
                    foreach (var item in model.Guardians)
                    {
                        var guardianCreationRequestModel = item;
                        guardianCreationRequestModel.UserId = userIdTracking;
                        var responseCreateGuardian = await _guardianService.AddNewGuardian(guardianCreationRequestModel);
                        if (!responseCreateGuardian.IsSuccess)
                        {
                            return responseCreateGuardian;
                        }
                        guardianIdsTracking.Add((Guid)responseCreateGuardian.Result);
                    }
                }

                // Create contract
                var contractCreationRequestModel = new ContractRequestModel()
                {
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    RoomId = model.RoomId,
                    UserId = userIdTracking,
                };
                var responseCreateContract = await AddNewContract(contractCreationRequestModel);
                if (!responseCreateContract.IsSuccess)
                {
                    return responseCreateContract;
                }
                contractIdTracking = (Guid)responseCreateContract.Result;

                await _unitOfWork.SaveChangeAsync();

                // Complete transaction
                scope.Complete();
            }
            return new ApiResponse().SetCreated();
        }

        public async Task<ApiResponse> AddNewContract(ContractRequestModel model)
        {
            // Compare StartDate and EndDate

            //

            var userEntity = await _unitOfWork.UserRepository.GetAsync(x => x.Id.Equals(model.UserId));
            if (userEntity == null)
            {
                return new ApiResponse().SetNotFound(model.UserId, message: string.Format(ErrorMessages.PropertyDoesNotExist, "User"));
            }

            var roomEntity = await _unitOfWork.RoomRepository.GetAsync(x => x.Id.Equals(model.RoomId), x => x.Include(x => x.Building));
            if (roomEntity == null)
            {
                return new ApiResponse().SetNotFound(model.RoomId, message: string.Format(ErrorMessages.PropertyDoesNotExist, "Room"));
            }

            if (roomEntity.Building.GenderRestriction != userEntity.Gender)
            {
                return new ApiResponse().SetBadRequest(message: string.Format(ErrorMessages.ConflictGenderWhenChooseRoom, userEntity.Gender.ToString(), roomEntity.Building.GenderRestriction.ToString()));
            }

            if (roomEntity.TotalUsedBed >= roomEntity.TotalAvailableBed)
            {
                return new ApiResponse().SetBadRequest(message: string.Format(ErrorMessages.RoomIsFull));
            }

            var contractEntity = _contractMapper.MapToContractEntity(model);

            contractEntity.CreatedBy = _userContextService.UserId;
            contractEntity.LastUpdatedBy = _userContextService.UserId;

            if (roomEntity.TotalUsedBed + 1 == roomEntity.TotalAvailableBed)
            {
                roomEntity.Status = RoomStatusEnum.FULL;
            }
            roomEntity.TotalUsedBed = roomEntity.TotalUsedBed + 1;

            await _unitOfWork.ContractRepository.AddAsync(contractEntity);
            await _unitOfWork.SaveChangeAsync();

            return new ApiResponse().SetCreated(contractEntity.Id);
        }

        public async Task<ApiResponse> UpdateContractStatus(Guid id, ContractStatusEnum status)
        {
            var userId = _userContextService.UserId;
            if (userId == Guid.Empty)
            {
                return new ApiResponse().SetForbidden(null, "User not authenticated");
            }

            ContractEntity? contractEntity;

            if (_userContextService.UserRoles.Contains(Role.ADMIN))
            {
                contractEntity = await _unitOfWork.ContractRepository.GetAsync(x => x.Id == id, x => x.Include(x => x.Room));
            }
            else
            {
                contractEntity = await _unitOfWork.ContractRepository.GetAsync(x => x.Id == id && x.UserId == userId, x => x.Include(x => x.Room));
            }

            if (contractEntity == null)
            {
                return new ApiResponse().SetNotFound(message: "Contract not found");
            }

            contractEntity.Status = status;
            contractEntity.LastUpdatedBy = userId;
            contractEntity.LastUpdatedDateUtc = DateTime.UtcNow;

            if (status == ContractStatusEnum.REJECTED || status == ContractStatusEnum.TERMINATED || status == ContractStatusEnum.EXPIRED)
            {
                // Release bed and update room status
                contractEntity.Room.TotalAvailableBed += 1;
                contractEntity.Room.TotalUsedBed -= 1;
                contractEntity.Room.Status = contractEntity.Room.TotalAvailableBed == contractEntity.Room.TotalUsedBed ? RoomStatusEnum.FULL : RoomStatusEnum.AVAILABLE;
                contractEntity.Room.LastUpdatedBy = userId;
                contractEntity.Room.LastUpdatedDateUtc = DateTime.UtcNow;
            }

            await _unitOfWork.SaveChangeAsync();

            return new ApiResponse().SetOk();
        }
        private bool IsActiveContractExisted(UserEntity user)
        {
            if (user == null || user.Contracts == null || user.Contracts.Count == 0)
            {
                return false;
            }
            // Check user have available contract
            var isUserHaveActiveContract = user.Contracts.Any(contract =>
                contract.EndDate >= DateTime.UtcNow &&
                (contract.Status == ContractStatusEnum.ACTIVE ||
                contract.Status == ContractStatusEnum.EXTENDED ||
                contract.Status == ContractStatusEnum.PENDING ||
                contract.Status == ContractStatusEnum.WAITING_PAYMENT));

            return isUserHaveActiveContract;
        }

        public async Task<ApiResponse> GetSingleContract(Guid id)
        {
            var userId = _userContextService.UserId;
            if (userId == Guid.Empty)
            {
                return new ApiResponse().SetNotFound("User not found");
            }

            ContractEntity? contractEntity;

            if (_userContextService.UserRoles.Contains(Role.ADMIN))
            {
                contractEntity = await _unitOfWork.ContractRepository.GetAsync(x => x.Id == id, x => x.Include(x => x.Approver).Include(x => x.User).Include(x => x.Room));
            }
            else
            {
                contractEntity = await _unitOfWork.ContractRepository.GetAsync(x => x.Id == id && x.UserId == userId, x => x.Include(x => x.Approver).Include(x => x.User).Include(x => x.Room));
            }

            if (contractEntity == null)
            {
                return new ApiResponse().SetNotFound(message: "Contract not found");
            }

            var response = _contractMapper.MapToContractModel(contractEntity);

            return new ApiResponse().SetOk(response);
        }

        public async Task<ApiResponse> GetContractBatch(GetBatchRequestModel model)
        {
            var userId = _userContextService.UserId;
            if (userId == Guid.Empty)
            {
                return new ApiResponse().SetNotFound("User not found");
            }

            var contractEntities = new List<ContractEntity>();

            if (_userContextService.UserRoles.Contains(Role.ADMIN))
            {
                if (model.IsGetAll)
                {
                    contractEntities = await _unitOfWork.ContractRepository.GetAllAsync(x => true, x => x.Include(x => x.Approver).Include(x => x.User).Include(x => x.Room));
                }
                else
                {
                    contractEntities = await _unitOfWork.ContractRepository.GetAllAsync(x => model.Ids.Contains(x.Id), x => x.Include(x => x.Approver).Include(x => x.User).Include(x => x.Room));
                }
            }
            else
            {
                if (model.IsGetAll)
                {
                    contractEntities = await _unitOfWork.ContractRepository.GetAllAsync(x => x.UserId == userId, x => x.Include(x => x.Approver).Include(x => x.User).Include(x => x.Room));
                }
                else
                {
                    contractEntities = await _unitOfWork.ContractRepository.GetAllAsync(x => model.Ids.Contains(x.Id) && x.UserId == userId, x => x.Include(x => x.Approver).Include(x => x.User).Include(x => x.Room));
                }
            }

            if (!model.IsGetAll)
            {
                if (contractEntities.Count != model.Ids.Count)
                {
                    // Find the missing request IDs
                    var foundRequestIds = contractEntities.Select(r => r.Id).ToList();
                    var missingRequestIds = model.Ids.Except(foundRequestIds).ToList();

                    // Return with error message listing the missing request IDs
                    var errorMessage = $"Request(s) not found: {string.Join(", ", missingRequestIds)}";
                    return new ApiResponse().SetNotFound(message: errorMessage);
                }
            }

            var response = contractEntities.Select(x => _contractMapper.MapToContractModel(x)).ToList();

            return new ApiResponse().SetOk(response);
        }
    }
}
