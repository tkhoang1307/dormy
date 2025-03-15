using Dormy.WebService.Api.Core.Constants;
using Dormy.WebService.Api.Core.CustomExceptions;
using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Core.Interfaces;
using Dormy.WebService.Api.Core.Utilities;
using Dormy.WebService.Api.Infrastructure.TokenRetriever;
using Dormy.WebService.Api.Models.Constants;
using Dormy.WebService.Api.Models.Enums;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;
using Dormy.WebService.Api.Presentation.Mappers;
using Dormy.WebService.Api.Startup;
using Microsoft.EntityFrameworkCore;

namespace Dormy.WebService.Api.ApplicationLogic
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenRetriever _tokenRetriever;
        private readonly UserMapper _userMapper;

        public UserService(IUnitOfWork unitOfWork, ITokenRetriever tokenRetriever)
        {
            _unitOfWork = unitOfWork;
            _userMapper = new UserMapper();
            _tokenRetriever = tokenRetriever;
        }

        public async Task<ApiResponse> SignUp(UserRequestModel model)
        {
            var userMapperModel = new UserMapperRequestModel()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.PhoneNumber,
                Password = UserHelper.BuildPasswordForUser(model.DateOfBirth),
                Email = model.Email,
                DateOfBirth = model.DateOfBirth,
                PhoneNumber = model.PhoneNumber,
                NationalIdNumber = model.NationalIdNumber,
                Gender = model.Gender,
            };
            var userEntity = _userMapper.MapToUserEntity(userMapperModel);
            userEntity.Status = UserStatusEnum.ACTIVE;

            var existedAccountByUsername = await _unitOfWork.UserRepository.GetAsync(x => x.UserName.ToLower().Equals(model.PhoneNumber.ToLower()));
            if (existedAccountByUsername != null)
            {
                throw new UsernameIsExistedException(ErrorMessages.PhoneNumberIsExisted);
            }

            await _unitOfWork.UserRepository.AddAsync(userEntity);
            await _unitOfWork.SaveChangeAsync();

            return new ApiResponse().SetOk(userEntity.Id);
        }

        public async Task<ApiResponse> Login(LoginRequestModel model)
        {
            var accountEntity = await _unitOfWork.UserRepository
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
                Role = Role.USER,
                UserName = accountEntity.UserName,
            });

            var userResponseModel = _userMapper.MapToUserResponseModel(accountEntity);

            var dataResponse = new UserLoginResponseModel
            {
                AccessToken = accessToken,
                UserInformation = userResponseModel
            };

            return new ApiResponse().SetOk(dataResponse);
        }

        public async Task<ApiResponse> ChangePassword(ChangePasswordRequestModel model)
        {
            if (model is null)
            {
                return new ApiResponse().SetBadRequest();
            }

            var userAccount = await _unitOfWork.UserRepository.GetAsync(x => x.Id.Equals(model.Id));

            if (userAccount == null)
            {
                return new ApiResponse().SetNotFound();
            }

            if (!EncryptHelper.VerifyPassword(model.OldPassword, userAccount.Password))
            {
                throw new DuplicatedPasswordUpdateException(ErrorMessages.PasswordDoesNotMatchErrorMessage);
            }

            if (EncryptHelper.VerifyPassword(model.NewPassword, userAccount.Password))
            {
                throw new DuplicatedPasswordUpdateException(ErrorMessages.DuplicatedErrorMessage);
            }

            userAccount.Password = EncryptHelper.HashPassword(model.NewPassword);
            await _unitOfWork.SaveChangeAsync();
            return new ApiResponse().SetOk();
        }

        public async Task<ApiResponse> ResetPassword(Guid id)
        {
            var userAccount = await _unitOfWork.UserRepository.GetAsync(x => x.Id.Equals(id));

            if (userAccount == null)
            {
                return new ApiResponse().SetNotFound();
            }

            var newPassword = GeneratePassword();

            userAccount.Password = EncryptHelper.HashPassword(newPassword);
            await _unitOfWork.SaveChangeAsync();
            return new ApiResponse().SetOk(newPassword);
        }

        public async Task<ApiResponse> UpdateProfile(Guid id, UserUpdateRequestModel model)
        {
            var userEntity = await _unitOfWork.UserRepository.GetAsync(x => x.Id == id);

            if (userEntity is null)
            {
                return new ApiResponse().SetNotFound(id);
            }

            userEntity.FirstName = model.FirstName;
            userEntity.LastName = model.LastName;
            userEntity.Email = model.Email;
            userEntity.PhoneNumber = model.PhoneNumber;
            userEntity.DateOfBirth = model.DateOfBirth;
            userEntity.Gender = (GenderEnum)Enum.Parse(typeof(GenderEnum), model.Gender);
            userEntity.NationalIdNumber = model.NationalIdNumber;

            await _unitOfWork.SaveChangeAsync();

            return new ApiResponse().SetOk(id);
        }

        public async Task<ApiResponse> GetUserBatch(bool isGetAll, List<Guid> ids)
        {
            List<UserEntity> users = [];

            if (isGetAll)
            {
                users = await _unitOfWork.UserRepository.GetAllAsync(x => true, x => x.Include(y => y.Workplace)?.Include(z => z.Guardians));
            }
            else
            {
                users = await _unitOfWork.UserRepository.GetAllAsync(x => ids.Contains(x.Id), x => x.Include(y => y.Workplace)?.Include(z => z.Guardians));
            }

            var result = users.Select(x => _userMapper.MapToUserResponseModel(x)).ToList();

            return new ApiResponse().SetOk(result);
        }

        public async Task<ApiResponse> GetUserInformation(Guid id)
        {
            var userEntity = await _unitOfWork.UserRepository.GetAsync(x => x.Id == id, x => x.Include(y => y.Guardians)?.Include(z => z.Workplace)?.Include(w => w.HealthInsurance));
            if (userEntity == null)
            {
                return new ApiResponse().SetNotFound(id);
            }
            return new ApiResponse().SetOk(_userMapper.MapToUserResponseModel(userEntity));
        }

        private static string GeneratePassword(int length = 8)
        {
            const string validChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();

            // Ensure at least one character from each required set
            string password = "";
            password += GetRandomChar(random, "ABCDEFGHIJKLMNOPQRSTUVWXYZ"); // At least one uppercase
            password += GetRandomChar(random, "abcdefghijklmnopqrstuvwxyz"); // At least one lowercase
            password += GetRandomChar(random, "0123456789"); // At least one digit

            // Fill the remaining length with random characters
            for (int i = password.Length; i < length; i++)
            {
                password += validChars[random.Next(validChars.Length)];
            }

            // Shuffle the password to randomize character positions
            return new string(password.ToCharArray().OrderBy(s => random.NextDouble()).ToArray());
        }

        private static char GetRandomChar(Random random, string charSet)
        {
            return charSet[random.Next(charSet.Length)];
        }

        public async Task<ApiResponse> UpdateUserWorkplace(Guid workplaceId, Guid userId)
        {
            var userEntity = await _unitOfWork.UserRepository.GetAsync(x => x.Id == userId);

            if (userEntity == null)
            {
                return new ApiResponse().SetNotFound(userId);
            }

            if (workplaceId == Guid.Empty)
            {
                userEntity.WorkplaceId = null;
                await _unitOfWork.SaveChangeAsync();
                return new ApiResponse().SetOk();
            }

            var workPlaceEntity = await _unitOfWork.WorkplaceRepository.GetAsync(x => x.Id == workplaceId);

            if (workPlaceEntity == null)
            {
                return new ApiResponse().SetNotFound(workplaceId);
            }

            userEntity.WorkplaceId = workPlaceEntity.Id;
            await _unitOfWork.SaveChangeAsync();
            return new ApiResponse().SetOk();
        }

        public async Task<ApiResponse> UpdateUserHealthInsurance(HealthInsuranceRequestModel? model, Guid userId)
        {
            var userEntity = await _unitOfWork.UserRepository.GetAsync(x => x.Id == userId, x => x.Include(x => x.HealthInsurance));

            if (userEntity == null)
            {
                return new ApiResponse().SetNotFound(userId);
            }

            if (model == null)
            {
                userEntity.HealthInsurance = null;
                await _unitOfWork.SaveChangeAsync();
                return new ApiResponse().SetOk();
            }

            if (userEntity.HealthInsurance != null)
            {
                await _unitOfWork.HealthInsuranceRepository.DeleteByIdAsync(userEntity.HealthInsurance.Id);
                await _unitOfWork.SaveChangeAsync();
            }

            var healthInsurance = new HealthInsuranceEntity()
            {
                Id = Guid.NewGuid(),
                ExpirationDate = model.ExpirationDate,
                RegisteredHospital = model.RegisteredHospital,
                InsuranceCardNumber = model.InsuranceCardNumber,
                CreatedBy = userId,
                CreatedDateUtc = DateTime.UtcNow,
            };

            await _unitOfWork.HealthInsuranceRepository.AddAsync(healthInsurance);
            await _unitOfWork.SaveChangeAsync();

            userEntity.HealthInsuranceId = healthInsurance.Id;

            await _unitOfWork.SaveChangeAsync();
            return new ApiResponse().SetOk(healthInsurance.Id);
        }
    }
}
