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

            return StatusCode((int)result.StatusCode, result);
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

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("id/{id:guid}")]
        [Authorize(Roles = $"{Role.ADMIN}, {Role.USER}")]
        public async Task<IActionResult> GetGuardianById(Guid id)
        {
            var result = await _guardianService.GetDetailGuardianById(id);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("all")]
        [Authorize(Roles = Role.USER)]
        public async Task<IActionResult> GetAllGuardiansOfUser()
        {
            var result = await _guardianService.GetAllGuardiansOfUser();

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("batch")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> GetBatchGuardiansByAdmin(GetBatchGuardianRequestModel model)
        {
            var result = await _guardianService.GetGuardianBatch(model);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("hard-delete/id/{id:guid}")]
        [Authorize(Roles = Role.USER)]
        public async Task<IActionResult> HardDeleteGuardian(Guid id)
        {
            var result = await _guardianService.HardDeleteParkingSpot(id);

            return StatusCode((int)result.StatusCode, result);
        }
    }
}
