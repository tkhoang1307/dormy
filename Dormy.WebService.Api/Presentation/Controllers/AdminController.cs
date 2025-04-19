using Dormy.WebService.Api.ApplicationLogic;
using Dormy.WebService.Api.Core.Interfaces;
using Dormy.WebService.Api.Models.Constants;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Presentation.Validations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dormy.WebService.Api.Presentation.Controllers
{
    [ApiController]
    [Route("api/admin")]
    public class AdminController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IAdminService _adminService;
        private readonly IEmailService _emailService;
        private readonly ILogger<AdminController> _logger;
        private readonly ITokenRetriever _tokenRetriever;
        private readonly IUserService _userService;

        public AdminController(IConfiguration configuration,
            ILogger<AdminController> logger,
            IAdminService adminService,
            ITokenRetriever tokenRetriever,
            IEmailService emailService,
            IUserService userService)
        {
            _configuration = configuration;
            _logger = logger;
            _adminService = adminService;
            _tokenRetriever = tokenRetriever;
            _emailService = emailService;
            _userService = userService;
        }

        [HttpGet]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> GetAllUser()
        {
            var result = await _adminService.GetAllUser();
            return Ok(result);
        }

        [HttpGet("id/{id:guid}")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> GetAdminAccount(Guid id)
        {
            var result = await _adminService.GetAdminAccount(id);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("create-account")]
        public async Task<IActionResult> CreateAdminAccount(AdminRequestModel request)
        {
            var secretKeyFromHeader = HttpContext.Request.Headers["SecretKey"].FirstOrDefault();

            // Check if the SecretKey exists in the header
            if (string.IsNullOrEmpty(secretKeyFromHeader))
            {
                return Unauthorized(new { message = "SecretKey is missing in the header." });
            }

            // Retrieve the SecretKey from appsettings
            var secretKeyFromAppSettings = _configuration["SecretKeyToCreateAdmin"];

            if (secretKeyFromHeader != secretKeyFromAppSettings)
            {
                return Unauthorized(new { message = "Invalid SecretKey." });
            }

            var modelValidator = await AdminValidator.AdminRequestModelValidator(request);
            if (!modelValidator.IsSuccess)
            {
                return StatusCode((int)modelValidator.StatusCode, modelValidator);
            }

            var result = await _adminService.CreateAdminAccount(request);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("dashboard")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> GetDashBoardInformation()
        {
            var response = await _adminService.GetDashboardInformation();
            return StatusCode((int)response.StatusCode, response.Result);
        }

        [HttpGet("userId/{id:guid}")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> GetUserProfileByUseridForAdmin(Guid id)
        {
            var response = await _userService.GetUserProfileByUseridForAdmin(id);

            return StatusCode((int)response.StatusCode, response);
        }
    }
}
