using Dormy.WebService.Api.Core.Interfaces;
using Dormy.WebService.Api.Models.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dormy.WebService.Api.Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly IEmailService _emailService;
        private readonly ILogger<AdminController> _logger;
        private readonly ITokenRetriever _tokenRetriever;

        public AdminController(
            ILogger<AdminController> logger,
            IAdminService adminService,
            ITokenRetriever tokenRetriever,
            IEmailService emailService)
        {
            _logger = logger;
            _adminService = adminService;
            _tokenRetriever = tokenRetriever;
            _emailService = emailService;
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
            if (result.IsSuccess)
            {
                return Ok(result.Result);
            }
            return NotFound();
        }
    }
}
