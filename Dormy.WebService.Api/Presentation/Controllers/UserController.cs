using Dormy.WebService.Api.Core.Interfaces;
using Dormy.WebService.Api.Models.Constants;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            var result = await _userService.SignUp(request);
            return Ok(result);
        }

        [HttpPost("user/sign-in")]
        [AllowAnonymous]
        public async Task<IActionResult> SignIn([FromBody] LoginRequestModel request)
        {
            var response = await _userService.Login(request);

            return response.IsSuccess ? Ok(response) : BadRequest(response);
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
            return Ok();
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
            return response.IsSuccess ? Ok(response.Result) : NotFound(id);
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
            var response = await _userService.UpdateProfile(id, model);
            return response.IsSuccess ? Ok(response.Result) : NotFound(response);
        }

        [HttpGet("user/id/{id:guid}")]
        [Authorize(Roles = Role.USER)]
        public async Task<IActionResult> GetProfile(Guid id)
        {
            var response = await _userService.GetUserInformation(id);
            return response.IsSuccess ? Ok(response.Result) : NotFound(response);
        }

        [HttpPost("user/batch")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> GetUserBatch([FromBody] GetBatchRequestModel request)
        {
            var response = await _userService.GetUserBatch(request.IsGetAll, request.Ids);
            return response.IsSuccess ? Ok(response.Result) : NotFound(response);
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

            return Ok(response);
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

            return Ok(response);
        }
    }
}
