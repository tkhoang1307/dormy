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

            return StatusCode((int)response.StatusCode, response);
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

            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPut]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> UpdateBuilding(BuildingUpdationRequestModel model)
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
