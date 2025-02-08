using Dormy.WebService.Api.Core.Constants;
using Dormy.WebService.Api.Core.Interfaces;
using Dormy.WebService.Api.Models.Constants;
using Dormy.WebService.Api.Models.Enums;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;
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
        public async Task<IActionResult> GetBuildingBatch([FromBody] GetBatchRequestModel request)
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
        public async Task<IActionResult> CreateBuilding(BuildingCreationRequestModel model)
        {
            if (string.IsNullOrEmpty(model.Name))
            {
                return UnprocessableEntity(new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Name))));
            }

            if (string.IsNullOrEmpty(model.GenderRestriction))
            {
                return UnprocessableEntity(new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.GenderRestriction))));
            }

            if (model?.TotalFloors == null)
            {
                return UnprocessableEntity(new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.TotalFloors))));
            }

            if (!Enum.TryParse(model.GenderRestriction, out GenderEnum result))
            {
                return StatusCode(412, new ApiResponse().SetPreconditionFailed(message:
                    string.Format(ErrorMessages.ValueDoesNotExistInEnum, model.GenderRestriction, nameof(GenderEnum))));
            }    

            if (model.TotalFloors <= 0)
            {
                return StatusCode(412, new ApiResponse().SetPreconditionFailed(message:
                    string.Format(ErrorMessages.PropertyMustBeMoreThan0, nameof(model.TotalFloors))));
            }

            var response = await _buildingService.CreateBuilding(model);

            return StatusCode(201, response);
        }

        [HttpPost("create/batch")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> CreateBuildingBatch(List<BuildingCreationRequestModel> models)
        {
            foreach(var model in models)
            {
                if (string.IsNullOrEmpty(model.Name))
                {
                    return UnprocessableEntity(new ApiResponse().SetUnprocessableEntity(message:
                        string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Name))));
                }

                if (string.IsNullOrEmpty(model.GenderRestriction))
                {
                    return UnprocessableEntity(new ApiResponse().SetUnprocessableEntity(message:
                        string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.GenderRestriction))));
                }

                if (model?.TotalFloors == null)
                {
                    return UnprocessableEntity(new ApiResponse().SetUnprocessableEntity(message:
                        string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.TotalFloors))));
                }

                if (!Enum.TryParse(model.GenderRestriction, out GenderEnum result))
                {
                    return StatusCode(412, new ApiResponse().SetPreconditionFailed(message:
                        string.Format(ErrorMessages.ValueDoesNotExistInEnum, model.GenderRestriction, nameof(GenderEnum))));
                }

                if (model.TotalFloors <= 0)
                {
                    return StatusCode(412, new ApiResponse().SetPreconditionFailed(message:
                        string.Format(ErrorMessages.PropertyMustBeMoreThan0, nameof(model.TotalFloors))));
                }
            }
            var response = await _buildingService.CreateBuildingBatch(models);

            return StatusCode(201, response);
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
