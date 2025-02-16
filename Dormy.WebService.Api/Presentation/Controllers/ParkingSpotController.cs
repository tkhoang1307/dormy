using Dormy.WebService.Api.ApplicationLogic;
using Dormy.WebService.Api.Core.Constants;
using Dormy.WebService.Api.Core.Interfaces;
using Dormy.WebService.Api.Models.Constants;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dormy.WebService.Api.Presentation.Controllers
{
    [Route("api/[controller]")]
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
            if (string.IsNullOrEmpty(model.ParkingSpotName))
            {
                return UnprocessableEntity(new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.ParkingSpotName))));
            }

            if (model?.CapacitySpots == null)
            {
                return UnprocessableEntity(new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.CapacitySpots))));
            }

            if (model.CapacitySpots <= 0)
            {
                return StatusCode(412, new ApiResponse().SetPreconditionFailed(message:
                    string.Format(ErrorMessages.PropertyMustBeMoreThan0, nameof(model.CapacitySpots))));
            }

            var result = await _parkingSpotService.AddNewParkingSpot(model);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> UpdateParkingSpot(ParkingSpotUpdationRequestModel model)
        {
            if (model?.Id == null)
            {
                return UnprocessableEntity(new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Id))));
            } 
                
            if (string.IsNullOrEmpty(model.ParkingSpotName))
            {
                return UnprocessableEntity(new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.ParkingSpotName))));
            }

            if (model?.CapacitySpots == null)
            {
                return UnprocessableEntity(new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.CapacitySpots))));
            }

            if (model.CapacitySpots <= 0)
            {
                return StatusCode(412, new ApiResponse().SetPreconditionFailed(message:
                    string.Format(ErrorMessages.PropertyMustBeMoreThan0, nameof(model.CapacitySpots))));
            }

            var result = await _parkingSpotService.UpdateParkingSpot(model);

            return StatusCode((int)result.StatusCode, result);
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

        [HttpDelete("soft-delete/id/{id:guid}")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> SoftDeleteParkingSpot(Guid id)
        {
            var result = await _parkingSpotService.SoftDeleteParkingSpot(id);

            return StatusCode((int)result.StatusCode, result);
        }
    }
}
