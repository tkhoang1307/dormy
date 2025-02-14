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
    public class GuardianController : ControllerBase
    {
        private readonly IGuardianService _guardianService;

        public GuardianController(IGuardianService guardianService)
        {
            _guardianService = guardianService;
        }

        [HttpPost]
        [Authorize(Roles = Role.USER)]
        public async Task<IActionResult> CreateNewGuardian(GuardianRequestModel model)
        {
            if (string.IsNullOrEmpty(model.Name))
            {
                return UnprocessableEntity(new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Name))));
            }

            if (string.IsNullOrEmpty(model.PhoneNumber))
            {
                return UnprocessableEntity(new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.PhoneNumber))));
            }

            if (string.IsNullOrEmpty(model.Address))
            {
                return UnprocessableEntity(new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Address))));
            }

            if (string.IsNullOrEmpty(model.RelationshipToUser))
            {
                return UnprocessableEntity(new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.RelationshipToUser))));
            }

            var result = await _guardianService.AddNewGuardian(model);

            return StatusCode(201, result);
        }

        [HttpPut]
        [Authorize(Roles = Role.USER)]
        public async Task<IActionResult> UpdateGuardian(GuardianUpdationRequestModel model)
        {
            if (model?.Id == null)
            {
                return UnprocessableEntity(new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Id))));
            }

            if (string.IsNullOrEmpty(model.Name))
            {
                return UnprocessableEntity(new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Name))));
            }

            if (string.IsNullOrEmpty(model.PhoneNumber))
            {
                return UnprocessableEntity(new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.PhoneNumber))));
            }

            if (string.IsNullOrEmpty(model.Address))
            {
                return UnprocessableEntity(new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Address))));
            }

            if (string.IsNullOrEmpty(model.RelationshipToUser))
            {
                return UnprocessableEntity(new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.RelationshipToUser))));
            }

            var result = await _guardianService.UpdateGuardian(model);

            if (result.IsSuccess)
            {
                return StatusCode(202, result);
            }
            return NotFound(result);
        }

        [HttpGet("id/{id:guid}")]
        [Authorize(Roles = $"{Role.ADMIN}, {Role.USER}")]
        public async Task<IActionResult> GetGuardianById(Guid id)
        {
            var result = await _guardianService.GetDetailGuardianById(id);

            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return NotFound(result);
        }

        [HttpGet("all")]
        [Authorize(Roles = Role.USER)]
        public async Task<IActionResult> GetAllGuardiansOfUser()
        {
            var result = await _guardianService.GetAllGuardiansOfUser();

            return Ok(result);
        }

        [HttpDelete("hard-delete/id/{id:guid}")]
        [Authorize(Roles = Role.USER)]
        public async Task<IActionResult> HardDeleteGuardian(Guid id)
        {
            var result = await _guardianService.HardDeleteParkingSpot(id);

            if (result.IsSuccess)
            {
                return StatusCode(200, result);
            }
            return NotFound(result);
        }
    }
}
