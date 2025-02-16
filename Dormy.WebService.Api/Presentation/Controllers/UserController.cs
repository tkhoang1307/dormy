using Dormy.WebService.Api.Core.Constants;
using Dormy.WebService.Api.Core.Interfaces;
using Dormy.WebService.Api.Models.Constants;
using Dormy.WebService.Api.Models.Enums;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace Dormy.WebService.Api.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;
        private readonly ILogger<UserController> _logger;
        private readonly ITokenRetriever _tokenRetriever;

        public UserController(
            ILogger<UserController> logger,
            IUserService userService,
            ITokenRetriever tokenRetriever,
            IEmailService emailService)
        {
            _logger = logger;
            _userService = userService;
            _tokenRetriever = tokenRetriever;
            _emailService = emailService;
        }

        [HttpPost("user/sign-up")]
        [AllowAnonymous]
        public async Task<IActionResult> SignUp(UserRequestModel request)
        {
            if (string.IsNullOrEmpty(request.FirstName))
            {
                return UnprocessableEntity(new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(request.FirstName))));
            }

            if (string.IsNullOrEmpty(request.LastName))
            {
                return UnprocessableEntity(new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(request.LastName))));
            }

            if (string.IsNullOrEmpty(request.Email))
            {
                return UnprocessableEntity(new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(request.Email))));
            }

            if (string.IsNullOrEmpty(request.UserName))
            {
                return UnprocessableEntity(new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(request.UserName))));
            }

            if (string.IsNullOrEmpty(request.Password))
            {
                return UnprocessableEntity(new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(request.Password))));
            }

            if (request?.DateOfBirth != null)
            {
                return UnprocessableEntity(new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(request.DateOfBirth))));
            }

            if (string.IsNullOrEmpty(request.PhoneNumber))
            {
                return UnprocessableEntity(new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(request.PhoneNumber))));
            }

            if (string.IsNullOrEmpty(request.NationalIdNumber))
            {
                return UnprocessableEntity(new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(request.NationalIdNumber))));
            }

            if (!Enum.TryParse(request.Gender, out GenderEnum result))
            {
                return StatusCode(412, new ApiResponse().SetPreconditionFailed(message:
                    string.Format(ErrorMessages.ValueDoesNotExistInEnum, request.Gender, nameof(GenderEnum))));
            }

            var response = await _userService.SignUp(request);

            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPost("user/sign-in")]
        [AllowAnonymous]
        public async Task<IActionResult> SignIn([FromBody] LoginRequestModel request)
        {
            var response = await _userService.Login(request);

            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPut("user/change-password")]
        [Authorize(Roles = Role.USER)]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequestModel model)
        {
            var userClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userClaim == null)
            {
                return NotFound(new ApiResponse().SetNotFound("invalid_user"));
            }
            var isValidUserGuidId = Guid.TryParse(userClaim.Value, out Guid id);

            if (!isValidUserGuidId)
            {
                return BadRequest(new ApiResponse().SetNotFound("User ID is invalid type - Guid required"));
            }

            model.Id = id;

            var response = await _userService.ChangePassword(model);

            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPut("user/reset-password")]
        [Authorize(Roles = Role.USER)]
        public async Task<IActionResult> ResetPassword()
        {
            var userClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userClaim == null)
            {
                return NotFound(new ApiResponse().SetNotFound("invalid_user"));
            }
            var isValidUserGuidId = Guid.TryParse(userClaim.Value, out Guid id);

            if (!isValidUserGuidId)
            {
                return BadRequest(new ApiResponse().SetNotFound("User ID is invalid type - Guid required"));
            }
            var response = await _userService.ResetPassword(id);

            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPut("user/update-profile")]
        [Authorize(Roles = Role.USER)]
        public async Task<IActionResult> UpdateProfile(UserUpdateRequestModel model)
        {
            var userClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userClaim == null)
            {
                return NotFound(new ApiResponse().SetNotFound("invalid_user"));
            }
            var isValidUserGuidId = Guid.TryParse(userClaim.Value, out Guid id);

            if (!isValidUserGuidId)
            {
                return BadRequest(new ApiResponse().SetNotFound("User ID is invalid type - Guid required"));
            }

            if (string.IsNullOrEmpty(model.FirstName))
            {
                return UnprocessableEntity(new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.FirstName))));
            }

            if (string.IsNullOrEmpty(model.LastName))
            {
                return UnprocessableEntity(new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.LastName))));
            }

            if (string.IsNullOrEmpty(model.Email))
            {
                return UnprocessableEntity(new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Email))));
            }

            if (model?.DateOfBirth == null)
            {
                return UnprocessableEntity(new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.DateOfBirth))));
            }

            if (string.IsNullOrEmpty(model.PhoneNumber))
            {
                return UnprocessableEntity(new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.PhoneNumber))));
            }

            if (string.IsNullOrEmpty(model.NationalIdNumber))
            {
                return UnprocessableEntity(new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.NationalIdNumber))));
            }

            if (!Enum.TryParse(model.Gender, out GenderEnum result))
            {
                return StatusCode(412, new ApiResponse().SetPreconditionFailed(message:
                    string.Format(ErrorMessages.ValueDoesNotExistInEnum, model.Gender, nameof(GenderEnum))));
            }

            var response = await _userService.UpdateProfile(id, model);

            return StatusCode((int)response.StatusCode, response);
        }

        [HttpGet("user/id/{id:guid}")]
        [Authorize(Roles = Role.USER)]
        public async Task<IActionResult> GetProfile(Guid id)
        {
            var response = await _userService.GetUserInformation(id);

            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPost("user/batch")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> GetUserBatch([FromBody] GetBatchRequestModel request)
        {
            var response = await _userService.GetUserBatch(request.IsGetAll, request.Ids);

            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPut("user/update-workplace/workplaceId/{workplaceId:guid}")]
        [Authorize(Roles = Role.USER)]
        public async Task<IActionResult> UpdateUserWorkplace([FromRoute] Guid workplaceId)
        {
            var userClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userClaim == null)
            {
                return NotFound(new ApiResponse().SetNotFound("invalid_user"));
            }
            var isValidUserGuidId = Guid.TryParse(userClaim.Value, out Guid id);

            if (!isValidUserGuidId)
            {
                return BadRequest(new ApiResponse().SetNotFound("User ID is invalid type - Guid required"));
            }

            var response = await _userService.UpdateUserWorkplace(workplaceId, id);

            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPut("user/update-health-insurance")]
        [Authorize(Roles = Role.USER)]
        public async Task<IActionResult> UpdateUserHealthInsurance([FromBody] HealthInsuranceRequestModel? model)
        {
            var userClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userClaim == null)
            {
                return NotFound(new ApiResponse().SetNotFound("invalid_user"));
            }
            var isValidUserGuidId = Guid.TryParse(userClaim.Value, out Guid id);

            if (!isValidUserGuidId)
            {
                return BadRequest(new ApiResponse().SetNotFound("User ID is invalid type - Guid required"));
            }

            var response = await _userService.UpdateUserHealthInsurance(model, id);

            return StatusCode((int)response.StatusCode, response);
        }
    }
}
