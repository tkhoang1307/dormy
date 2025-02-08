using Dormy.WebService.Api.Core.Constants;
using Dormy.WebService.Api.Core.CustomExceptions;
using Dormy.WebService.Api.Core.Interfaces;
using Dormy.WebService.Api.Infrastructure.TokenRetriever;
using Dormy.WebService.Api.Models.Constants;
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
