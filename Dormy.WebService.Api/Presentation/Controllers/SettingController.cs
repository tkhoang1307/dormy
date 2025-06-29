﻿using Dormy.WebService.Api.ApplicationLogic;
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
        private readonly IConfiguration _configuration;
        private readonly ISettingService _settingService;

        public SettingController(ISettingService settingService, IConfiguration configuration)
        {
            _settingService = settingService;
            _configuration = configuration;
        }

        [HttpGet("keyname/{keyname}")]
        [Authorize(Roles = $"{Role.USER}, {Role.ADMIN}")]
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

        [HttpGet("all-datatypes")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> GetAllDataTypeEnums()
        {
            var result = await _settingService.GetAllDataTypeEnums();
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("keyname/{keyname}/hard-delete")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> DeleteSetting(string keyname)
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

            var response = await _settingService.HardDeleteSettingByKeyName(keyname);

            return StatusCode((int)response.StatusCode, response);
        }
    }
}
