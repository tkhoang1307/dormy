using Dormy.WebService.Api.Core.Interfaces;
using Dormy.WebService.Api.Models.Constants;
using Dormy.WebService.Api.Models.RequestModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dormy.WebService.Api.Presentation.Controllers
{
    [Route("api/vehicle-history")]
    [ApiController]
    public class VehicleHistoryController : ControllerBase
    {
        private readonly IVehicleHistoryService _vehicleHistoryService;

        public VehicleHistoryController(IVehicleHistoryService vehicleHistoryService)
        {
            _vehicleHistoryService = vehicleHistoryService;
        }

        [HttpGet("id/{id:guid}")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> GetVehicleHistory(Guid id)
        {
            var result = await _vehicleHistoryService.GetSingleVehicleHistory(id);
            return Ok(result);
        }

        [HttpPost("batch")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> GetVehicleHistoryBatch(GetBatchRequestModel model)
        {
            var result = await _vehicleHistoryService.GetVehicleHistories(model);
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> CreateVehicleHistory(VehicleHistoryRequestModel model)
        {
            var result = await _vehicleHistoryService.CreateVehicleHistory(model);
            return Ok(result);
        }

        [HttpDelete("id/{id:guid}/hard-delete")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> DeleteVehicleHistory(Guid id)
        {
            var result = await _vehicleHistoryService.SoftDeleteVehicleHistory(id);
            return Ok(result);
        }
    }
}
