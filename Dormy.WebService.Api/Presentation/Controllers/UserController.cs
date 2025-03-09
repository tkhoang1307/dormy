using Dormy.WebService.Api.Core.Constants;
using Dormy.WebService.Api.Core.Interfaces;
using Dormy.WebService.Api.Models.Constants;
using Dormy.WebService.Api.Models.Enums;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;
using Dormy.WebService.Api.Presentation.Validations;
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
            var modelValidator = await UserValidator.UserRequestModelValidator(request);
            if (!modelValidator.IsSuccess)
            {
                return StatusCode((int)modelValidator.StatusCode, modelValidator);
            }

            var response = await _userService.SignUp(request);

            return StatusCode((int)response.StatusCode, response); 
        }

        [HttpPost("user/sign-in")]
        [AllowAnonymous]
        public async Task<IActionResult> SignIn([FromBody] LoginRequestModel request)
        {
            var modelValidator = await UserValidator.LoginRequestModelValidator(request);
            if (!modelValidator.IsSuccess)
            {
                return StatusCode((int)modelValidator.StatusCode, modelValidator);
            }

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

            var modelValidator = await UserValidator.ChangePasswordRequestModelValidator(model);
            if (!modelValidator.IsSuccess)
            {
                return StatusCode((int)modelValidator.StatusCode, modelValidator);
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

            var modelValidator = await UserValidator.UserUpdateRequestModelValidator(model);
            if (!modelValidator.IsSuccess)
            {
                return StatusCode((int)modelValidator.StatusCode, modelValidator);
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
