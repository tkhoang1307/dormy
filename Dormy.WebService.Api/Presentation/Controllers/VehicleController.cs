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
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;

        public VehicleController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        [HttpPost]
        [Authorize(Roles = Role.USER)]
        public async Task<IActionResult> CreateNewVehicle(VehicleRequestModel model)
        {
            if (string.IsNullOrEmpty(model.LicensePlate))
            {
                return UnprocessableEntity(new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.LicensePlate))));
            }

            if (string.IsNullOrEmpty(model.VehicleType))
            {
                return UnprocessableEntity(new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.VehicleType))));
            }

            var result = await _vehicleService.AddNewVehicle(model);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut]
        [Authorize(Roles = Role.USER)]
        public async Task<IActionResult> UpdateVehicle(VehicleUpdationRequestModel model)
        {
            if (model?.Id == null)
            {
                return UnprocessableEntity(new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Id))));
            }

            if (string.IsNullOrEmpty(model.LicensePlate))
            {
                return UnprocessableEntity(new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.LicensePlate))));
            }

            if (string.IsNullOrEmpty(model.VehicleType))
            {
                return UnprocessableEntity(new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.VehicleType))));
            }

            var result = await _vehicleService.UpdateVehicle(model);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("id/{id:guid}")]
        [Authorize(Roles = $"{Role.ADMIN}, {Role.USER}")]
        public async Task<IActionResult> GetVehicleById(Guid id)
        {
            var result = await _vehicleService.GetDetailVehicleById(id);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("batch")]
        [Authorize(Roles = $"{Role.ADMIN}, {Role.USER}")]
        public async Task<IActionResult> GetBatchVechiles(GetBatchVehicleRequestModel model)
        {
            var result = await _vehicleService.GetVehicleBatch(model);

            return StatusCode((int)result.StatusCode, result);
        }
    }
}
