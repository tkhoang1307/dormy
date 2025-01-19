using Dormy.WebService.Api.Core.Interfaces;
using Dormy.WebService.Api.Models.Constants;
using Dormy.WebService.Api.Models.RequestModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dormy.WebService.Api.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuildingController : ControllerBase
    {
        private readonly IBuildingService _buildingService;

        public BuildingController(IBuildingService buildingService)
        {
            _buildingService = buildingService;
        }

        [HttpGet("id/{id:guid}")]
        [Authorize(Roles = $"{Role.ADMIN}, {Role.USER}")]
        public async Task<IActionResult> GetSingleBuilding(Guid id)
        {
            var response = await _buildingService.GetBuildingById(id);

            if (response.IsSuccess)
            {
                return Ok(response.Result);
            }

            return NotFound(response.Result);
        }

        [HttpPost("batch")]
        [Authorize(Roles = $"{Role.ADMIN}, {Role.USER}")]
        public async Task<IActionResult> GetSingleBuilding([FromBody] BuildingBatchRequestModel request)
        {
            var response = await _buildingService.GetBuildingBatch(request.Ids, request.IsGetAll);

            if (response.IsSuccess)
            {
                return Ok(response.Result);
            }

            return NotFound(response.Result);
        }

        [HttpPost]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> CreateBuilding(BuildingRequestModel model)
        {
            var response = await _buildingService.CreateBuilding(model);

            return Ok(response.Result);
        }

        [HttpDelete("id/{id:guid}")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> SoftDeleteBuilding(Guid id)
        {
            var response = await _buildingService.SoftDeleteBuildingById(id);
            return Ok(response.Result);
        }
    }
}
