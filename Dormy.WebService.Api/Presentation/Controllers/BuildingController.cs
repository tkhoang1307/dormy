using Dormy.WebService.Api.Core.Interfaces;
using Dormy.WebService.Api.Models.Constants;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Presentation.Validations;
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

            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPost("batch")]
        [Authorize(Roles = $"{Role.ADMIN}, {Role.USER}")]
        public async Task<IActionResult> GetBuildingBatch([FromBody] GetBatchRequestModel request)
        {
            var response = await _buildingService.GetBuildingBatch(request.Ids, request.IsGetAll);

            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPost]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> CreateBuilding(BuildingCreationRequestModel model)
        {
            var modelValidator = await BuildingValidator.BuildingCreationRequestModelValidator(model);
            if (!modelValidator.IsSuccess)
            {
                return StatusCode((int)modelValidator.StatusCode, modelValidator);
            }

            var response = await _buildingService.CreateBuilding(model);

            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPost("create/batch")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> CreateBuildingBatch(List<BuildingCreationRequestModel> models)
        {
            foreach(var model in models)
            {
                var modelValidator = await BuildingValidator.BuildingCreationRequestModelValidator(model);
                if (!modelValidator.IsSuccess)
                {
                    return StatusCode((int)modelValidator.StatusCode, modelValidator);
                }
            }
            var response = await _buildingService.CreateBuildingBatch(models);

            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPut]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> UpdateBuilding(BuildingUpdationRequestModel model)
        {
            var modelValidator = await BuildingValidator.BuildingUpdationRequestModelValidator(model);
            if (!modelValidator.IsSuccess)
            {
                return StatusCode((int)modelValidator.StatusCode, modelValidator);
            }

            var response = await _buildingService.UpdateBuilding(model);

            return StatusCode((int)response.StatusCode, response);
        }

        [HttpDelete("id/{id:guid}")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> SoftDeleteBuilding(Guid id)
        {
            var response = await _buildingService.SoftDeleteBuildingById(id);

            return StatusCode((int)response.StatusCode, response);
        }
    }
}
