using Dormy.WebService.Api.Core.Interfaces;
using Dormy.WebService.Api.Models.Constants;
using Dormy.WebService.Api.Models.RequestModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dormy.WebService.Api.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingController : ControllerBase
    {
        private readonly ISettingService _settingService;

        public SettingController(ISettingService settingService)
        {
            _settingService = settingService;
        }

        [HttpGet("id/{id:guid}")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> GetSettingById(Guid id)
        {
            var response = await _settingService.GetSettingById(id);

            return StatusCode((int)response.StatusCode, response);
        }

        [HttpGet("all")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> GetSettings()
        {
            var response = await _settingService.GetSettings();

            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPost]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> CreateSetting(SettingRequestModel model)
        {
            var response = await _settingService.CreateSetting(model);

            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPut]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> UpdateSetting(SettingUpdateRequestModel model)
        {
            var response = await _settingService.UpdateSetting(model);

            return StatusCode((int)response.StatusCode, response);
        }

        [HttpDelete("id/{id:guid}")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> DeleteSetting(Guid id)
        {
            var response = await _settingService.SoftDeleteSetting(id);

            return StatusCode((int)response.StatusCode, response);
        }
    }
}
