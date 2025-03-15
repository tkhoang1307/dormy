using Dormy.WebService.Api.ApplicationLogic;
using Dormy.WebService.Api.Core.Constants;
using Dormy.WebService.Api.Core.Interfaces;
using Dormy.WebService.Api.Models.Constants;
using Dormy.WebService.Api.Models.Enums;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;
using Dormy.WebService.Api.Presentation.Validations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dormy.WebService.Api.Presentation.Controllers
{
    [Route("api/parking-spot")]
    [ApiController]
    public class ParkingSpotController: ControllerBase
    {
        private readonly IParkingSpotService _parkingSpotService;

        public ParkingSpotController(IParkingSpotService parkingSpotService)
        {
            _parkingSpotService = parkingSpotService;
        }

        [HttpPost]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> CreateNewParkingSpot(ParkingSpotRequestModel model)
        {
            var modelValidator = await ParkingSpotValidator.ParkingSpotRequestModelValidator(model);
            if (!modelValidator.IsSuccess)
            {
                return StatusCode((int)modelValidator.StatusCode, modelValidator);
            }

            var result = await _parkingSpotService.AddNewParkingSpot(model);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> UpdateParkingSpot(ParkingSpotUpdationRequestModel model)
        {
            var modelValidator = await ParkingSpotValidator.ParkingSpotUpdationRequestModelValidator(model);
            if (!modelValidator.IsSuccess)
            {
                return StatusCode((int)modelValidator.StatusCode, modelValidator);
            }

            var result = await _parkingSpotService.UpdateParkingSpot(model);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("update/status")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> UpdateStatusParkingSpot(ParkingSpotUpdateStatusRequestModel model)
        {
            var modelValidator = await ParkingSpotValidator.ParkingSpotUpdateStatusRequestModelValidator(model);
            if (!modelValidator.IsSuccess)
            {
                return StatusCode((int)modelValidator.StatusCode, modelValidator);
            }

            var response = await _parkingSpotService.UpdateStatusParkingSpot(model);

            return StatusCode((int)response.StatusCode, response);
        }

        [HttpGet("id/{id:guid}")]
        [Authorize(Roles = $"{Role.ADMIN}, {Role.USER}")]
        public async Task<IActionResult> GetParkingSpotById(Guid id)
        {
            var result = await _parkingSpotService.GetDetailParkingSpot(id);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("batch")]
        [Authorize(Roles = $"{Role.ADMIN}, {Role.USER}")]
        public async Task<IActionResult> GetParkingSpotBatch(GetBatchRequestModel model)
        {
            var result = await _parkingSpotService.GetParkingSpotBatch(model);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("id/{id:guid}/soft-delete")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> SoftDeleteParkingSpot(Guid id)
        {
            var result = await _parkingSpotService.SoftDeleteParkingSpot(id);

            return StatusCode((int)result.StatusCode, result);
        }
    }
}
