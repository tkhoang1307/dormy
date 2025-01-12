using Dormy.WebService.Api.Core.Interfaces;
using Dormy.WebService.Api.Models.Constants;
using Dormy.WebService.Api.Models.RequestModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Dormy.WebService.Api.Presentation.Controllers
{
    [Route("api/[controller]")]
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

        [HttpPost("admin/create-account")]
        //[Authorize(Roles = Role.ADMIN)]
        [AllowAnonymous]
        public async Task<IActionResult> CreateAdminAccount(AdminRequestModel request)
        {
            var result = await _adminService.CreateAdminAccount(request);
            return Ok(result);
        }

        [HttpPost("admin/sign-in")]
        public async Task<IActionResult> AdminSignIn(LoginRequestModel request)
        {
            var account = await _adminService.Login(request);
            if (account != null)
            {
                var token = _tokenRetriever.CreateToken(account.Id, account.UserName, account.Email, Role.ADMIN);
                return Ok(token);
            }
            return NotFound();
        }

        [HttpPut("admin/change-password")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequestModel model)
        {
            var userClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userClaim == null)
            {
                return NotFound();
            }
            var isValidUserGuidId = Guid.TryParse(userClaim.Value, out Guid id);

            if (!isValidUserGuidId)
            {
                return BadRequest("User ID is invalid type - Guid required");
            }
            var response = await _adminService.ChangeAdminPassword(id, model.NewPassword);
            return Ok();
        }

        [HttpPut("admin/change-password/account")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> ChangePasswordAccount(ChangePasswordRequestModel model)
        {
            var response = await _adminService.ChangeAdminPassword(model.Id ?? Guid.Empty, model.NewPassword);
            return Ok();
        }

        [HttpPost("verify-email")]
        [Authorize(Roles = $"{Role.ADMIN}, {Role.USER}")]
        public async Task<IActionResult> SendVerifyEmail(VerifyEmailRequest request)
        {
            var verificationCode = await _emailService.SendVerifyEmailAsync(request.Email);
            return Ok(verificationCode);
        }
    }
}
