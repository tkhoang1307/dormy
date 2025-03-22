using Dormy.WebService.Api.Core.Constants;
using Dormy.WebService.Api.Core.Interfaces;
using Dormy.WebService.Api.Models.Constants;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;
using Dormy.WebService.Api.Presentation.Validations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dormy.WebService.Api.Presentation.Controllers
{
    [Route("api/setting")]
    [ApiController]
    public class SettingController : ControllerBase
    {
        private readonly ISettingService _settingService;

        public SettingController(ISettingService settingService)
        {
            _settingService = settingService;
        }

        [HttpGet("keyname/{keyname}")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> GetSettingByKeyname(string keyname)
        {
            var response = await _settingService.GetSettingByKeyName(keyname);

            return StatusCode((int)response.StatusCode, response);
        }

        [HttpGet("all")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> GetAllSettings()
        {
            var response = await _settingService.GetAllSettings();

            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPost]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> CreateSetting(SettingRequestModel model)
        {
            var modelValidator = await SettingValidator.SettingRequestModelValidator(model);
            if (!modelValidator.IsSuccess)
            {
                return StatusCode((int)modelValidator.StatusCode, modelValidator);
            }

            var response = await _settingService.CreateSetting(model);

            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPut]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> UpdateSetting(SettingUpdateValueRequestModel model)
        {
            var modelValidator = await SettingValidator.SettingUpdateValueRequestModelValidator(model);
            if (!modelValidator.IsSuccess)
            {
                return StatusCode((int)modelValidator.StatusCode, modelValidator);
            }

            var response = await _settingService.UpdateSetting(model);

            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPut("turn-on-off")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> TurnOnOffSettingByKeyname(SettingTurnOnOffRequestModel model)
        {
            var modelValidator = await SettingValidator.SettingTurnOnOffRequestModelValidator(model);
            if (!modelValidator.IsSuccess)
            {
                return StatusCode((int)modelValidator.StatusCode, modelValidator);
            }

            var response = await _settingService.TurnOnOrTurnOffSetting(model);

            return StatusCode((int)response.StatusCode, response);
        }

        [HttpDelete("keyname/{keyname}/hard-delete")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> DeleteSetting(string keyname)
        {
            var response = await _settingService.HardDeleteSettingByKeyName(keyname);

            return StatusCode((int)response.StatusCode, response);
        }
    }
}
