using Dormy.WebService.Api.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dormy.WebService.Api.Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly ILogger<AdminController> _logger;
        private readonly ITokenRetriever _tokenRetriever;

        public AdminController(ILogger<AdminController> logger, IAdminService adminService, ITokenRetriever tokenRetriever)
        {
            _logger = logger;
            _adminService = adminService;
            _tokenRetriever = tokenRetriever;
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAllUser()
        {
            var result = await _adminService.GetAllUser();
            return Ok(result); 
        }

        [HttpGet("token")]
        [AllowAnonymous]
        public IActionResult GetToken(string username, string email)
        {
            var token = _tokenRetriever.CreateToken(Guid.NewGuid(), username, email);
            return Ok(token);
        }
    }
}
