using Dormy.WebService.Api.Core.Constants;
using Dormy.WebService.Api.Core.CustomExceptions;
using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Core.Interfaces;
using Dormy.WebService.Api.Infrastructure.TokenRetriever;
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

        public AdminService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _adminMapper = new AdminMapper();
        }

        public async Task<ApiResponse> ChangeAdminPassword(Guid id, string newPassword)
        {
            var adminAccount = await _unitOfWork.AdminRepository.GetAsync(x => x.Id.Equals(id));

            if (adminAccount == null)
            {
                return new ApiResponse().SetNotFound();
            }

            if (EncryptHelper.VerifyPassword(newPassword, adminAccount.Password))
            {
                throw new DuplicatedPasswordUpdateException(ErrorMessages.DuplicatedErrorMessage);
            }

            adminAccount.Password = EncryptHelper.HashPassword(newPassword);
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

        public async Task<AdminEntity?> Login(LoginRequestModel model)
        {
            var accountEntity = await _unitOfWork.AdminRepository
                .GetAsync(x => x.UserName.ToLower().Equals(model.Username.ToLower()));

            if (accountEntity != null && EncryptHelper.VerifyPassword(model.Password, accountEntity.Password))
            {
                return accountEntity;
            }

            return null;
        }
    }
}
