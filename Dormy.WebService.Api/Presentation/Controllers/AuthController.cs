using Dormy.WebService.Api.Core.Interfaces;
using Dormy.WebService.Api.Models.Constants;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Dormy.WebService.Api.Presentation.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly IEmailService _emailService;
        private readonly ILogger<AuthController> _logger;
        private readonly ITokenRetriever _tokenRetriever;

        public AuthController(
            ILogger<AuthController> logger,
            IAdminService adminService,
            ITokenRetriever tokenRetriever,
            IEmailService emailService)
        {
            _logger = logger;
            _adminService = adminService;
            _tokenRetriever = tokenRetriever;
            _emailService = emailService;
        }

        [HttpPost("admin/sign-in")]
        public async Task<IActionResult> SignInAdmin([FromBody] LoginRequestModel request)
        {
            var response = await _adminService.Login(request);

            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPut("admin/change-password")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequestModel model)
        {
            var userClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userClaim == null)
            {
                //return NotFound();
                return NotFound(new ApiResponse().SetNotFound("invalid_user"));
            }
            var isValidUserGuidId = Guid.TryParse(userClaim.Value, out Guid id);

            if (!isValidUserGuidId)
            {
                return BadRequest(new ApiResponse().SetNotFound("User ID is invalid type - Guid required"));
            }

            model.Id = id;

            var response = await _adminService.ChangeAdminPassword(model);

            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPut("admin/change-password/account")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> ChangePasswordAccount(ChangePasswordRequestModel model)
        {
            var response = await _adminService.ChangeAdminPassword(model);

            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPost("verify-email")]
        [AllowAnonymous]
        public async Task<IActionResult> SendVerifyEmail(VerifyEmailRequest request)
        {
            var verificationCode = await _emailService.SendVerifyEmailAsync(request.Email);
            return Ok(verificationCode);
        }

        [HttpPost("forgot-password")]
        [AllowAnonymous]
        public async Task<IActionResult> SendForgotPasswordEmail([FromBody] ForgotPasswordRequestModel  model)
        {
            var response = await _emailService.SendForgotPasswordEmailAsync(model.Email);
            return StatusCode((int)response.StatusCode, response);
        }
    }
}
