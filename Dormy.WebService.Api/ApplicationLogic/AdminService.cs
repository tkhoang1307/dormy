using Dormy.WebService.Api.Core.Constants;
using Dormy.WebService.Api.Core.CustomExceptions;
using Dormy.WebService.Api.Core.Interfaces;
using Dormy.WebService.Api.Infrastructure.TokenRetriever;
using Dormy.WebService.Api.Models.Constants;
using Dormy.WebService.Api.Models.Enums;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;
using Dormy.WebService.Api.Presentation.Mappers;
using Dormy.WebService.Api.Startup;

namespace Dormy.WebService.Api.ApplicationLogic
{
    public class AdminService : IAdminService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AdminMapper _adminMapper;
        private readonly ITokenRetriever _tokenRetriever;

        public AdminService(IUnitOfWork unitOfWork, ITokenRetriever tokenRetriever)
        {
            _unitOfWork = unitOfWork;
            _adminMapper = new AdminMapper();
            _tokenRetriever = tokenRetriever;
        }

        public async Task<ApiResponse> ChangeAdminPassword(ChangePasswordRequestModel model)
        {
            if (model is null)
            {
                return new ApiResponse().SetBadRequest();
            }

            var adminAccount = await _unitOfWork.AdminRepository.GetAsync(x => x.Id.Equals(model.Id));

            if (adminAccount == null)
            {
                return new ApiResponse().SetNotFound();
            }

            if (!EncryptHelper.VerifyPassword(model.OldPassword, adminAccount.Password))
            {
                throw new DuplicatedPasswordUpdateException(ErrorMessages.PasswordDoesNotMatchErrorMessage);
            }

            if (EncryptHelper.VerifyPassword(model.NewPassword, adminAccount.Password))
            {
                throw new DuplicatedPasswordUpdateException(ErrorMessages.DuplicatedErrorMessage);
            }

            adminAccount.Password = EncryptHelper.HashPassword(model.NewPassword);
            await _unitOfWork.SaveChangeAsync();
            return new ApiResponse().SetOk();
        }

        public async Task<ApiResponse> CreateAdminAccount(AdminRequestModel model)
        {
            var adminEntity = _adminMapper.MapToAdminEntity(model);

            var existedAccountByUsername = await _unitOfWork.AdminRepository.GetAsync(x => x.UserName.ToLower().Equals(model.UserName.ToLower()));
            if (existedAccountByUsername != null)
            {
                throw new UsernameIsExistedException(ErrorMessages.UsernameIsExisted);
            }

            await _unitOfWork.AdminRepository.AddAsync(adminEntity);
            await _unitOfWork.SaveChangeAsync();

            return new ApiResponse().SetOk(true);
        }

        public async Task<ApiResponse> GetAdminAccount(Guid id)
        {
            var accountEntity = await _unitOfWork.AdminRepository.GetAsync(x => x.Id == id);
            if (accountEntity == null)
            {
                return new ApiResponse().SetNotFound();
            }
            var account = _adminMapper.MapToAdminResponseModel(accountEntity);
            return new ApiResponse().SetOk(account);
        }

        public async Task<List<AdminResponseModel>> GetAllUser()
        {
            var adminEntities = await _unitOfWork.AdminRepository.GetAllAsync(x => true);
            var results = adminEntities
                .Select(x => _adminMapper.MapToAdminResponseModel(x))
                .ToList();
            return results;
        }

        public async Task<ApiResponse> GetDashboardInformation()
        {
            var registrations = await _unitOfWork.ContractRepository.GetAllAsync(x => true, isPaging: false);
            var extendRegistrations = await _unitOfWork.ContractExtensionRepository.GetAllAsync(x => true, isPaging: false);
            var rooms = await _unitOfWork.RoomRepository.GetAllAsync(x => true, isPaging: false);
            var users = await _unitOfWork.UserRepository.GetAllAsync(x => true, isPaging: false);
            var requests = await _unitOfWork.RequestRepository.GetAllAsync(x => true, isPaging: false);
            var parkingRequests = await _unitOfWork.ParkingRequestRepository.GetAllAsync(x => true, isPaging: false);

            // Calculate total contracts per month for the last 12 months
            var now = DateTime.UtcNow;
            var lastYear = now.AddMonths(-12);

            var totalContractsPerMonth = registrations
                .Where(c => c.SubmissionDate >= lastYear) // Filter contracts from the last 12 months
                .GroupBy(c => new { c.SubmissionDate.Year, c.SubmissionDate.Month }) // Group by year and month
                .OrderBy(g => g.Key.Year).ThenBy(g => g.Key.Month) // Order by year and month
                .Select(g => new
                {
                    Month = g.Key.Month,
                    Year = g.Key.Year,
                    TotalContracts = g.Count()
                })
                .ToList();

            // Ensure all 12 months are included, even if there are no contracts for some months
            var totalContractsForLast12Months = Enumerable.Range(0, 12)
                .Select(i =>
                {
                    var date = now.AddMonths(-i);
                    var month = date.Month;
                    var year = date.Year;

                    var totalContracts = totalContractsPerMonth
                        .FirstOrDefault(x => x.Month == month && x.Year == year)?.TotalContracts ?? 0;

                    return totalContracts;
                })
                .Reverse() // Reverse to get the months in chronological order
                .ToList();

            var result = new AdminDashboardResponseModel();
            result.TotalRequests = requests.Count;
            result.TotalParkingRequests = parkingRequests.Count;
            result.TotalUnResovledRequests = requests.Where(x => x.Status == RequestStatusEnum.SUBMITTED).ToList().Count;
            result.TotalUnResovledParkingRequests = parkingRequests.Where(x => x.Status == RequestStatusEnum.SUBMITTED).ToList().Count;
            result.TotalUsers = users.Count;
            result.TotalCurrentUsers = users.Where(x => x.Status == UserStatusEnum.ACTIVE).ToList().Count;
            result.TotalRegistrations = registrations.Count;
            result.TotalEmptyBeds = rooms.Sum(x => x.TotalAvailableBed);
            result.TotalUsedBeds = rooms.Sum(x => x.TotalUsedBed);
            result.TotalBeds = result.TotalEmptyBeds + result.TotalUsedBeds;
            result.TotalMaleUsers = users.Where(x => x.Gender == GenderEnum.MALE).ToList().Count;
            result.TotalFemaleUsers = users.Where(x => x.Gender == GenderEnum.FEMALE).ToList().Count;
            result.ContractsPerMonths = totalContractsForLast12Months;


            return new ApiResponse().SetOk(result);
        }

        public async Task<ApiResponse> Login(LoginRequestModel model)
        {
            var accountEntity = await _unitOfWork.AdminRepository
                .GetAsync(x => x.UserName == model.Username);

            if (accountEntity == null)
            {
                return new ApiResponse().SetBadRequest("Username is not existed");
            }

            if (!EncryptHelper.VerifyPassword(model.Password, accountEntity.Password))
            {
                return new ApiResponse().SetBadRequest("Password is not correct");
            }

            var accessToken = _tokenRetriever.CreateToken(new JwtResponseModel()
            {
                UserId = accountEntity.Id,
                Email = accountEntity.Email,
                FirstName = accountEntity.FirstName,
                LastName = accountEntity.LastName,
                Role = Role.ADMIN,
                UserName = accountEntity.UserName,
            });

            var adminResponseModel = _adminMapper.MapToAdminResponseModel(accountEntity);

            var dataResponse = new AdminLoginResponseModel
            {
                AccessToken = accessToken,
                AdminInformation = adminResponseModel
            };

            return new ApiResponse().SetOk(dataResponse);
        }
    }
}
