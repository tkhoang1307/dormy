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
    public class RoomTypeController : ControllerBase
    {
        private readonly IRoomTypeService _roomTypeService;

        public RoomTypeController(IRoomTypeService roomTypeService)
        {
            _roomTypeService = roomTypeService;
        }

        [HttpGet("all")]
        [Authorize(Roles = $"{Role.ADMIN}, {Role.USER}")]
        public async Task<IActionResult> GetAllRoomType()
        {
            var response = await _roomTypeService.GetRoomTypes();
            return Ok(response);
        }

        [HttpGet("id/{id:guid}")]
        [Authorize(Roles = $"{Role.ADMIN}, {Role.USER}")]
        public async Task<IActionResult> GetRoomTypeById(Guid id)
        {
            var response = await _roomTypeService.GetRoomTypeById(id);
            return response.IsSuccess ? Ok(response) : NotFound(response.Result);
        }

        [HttpPost]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> CreateRoomType([FromBody] RoomTypeRequestModel model)
        {
            if (string.IsNullOrEmpty(model.RoomTypeName))
            {
                return UnprocessableEntity(new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.RoomTypeName))));
            }

            if (model?.Capacity == null)
            {
                return UnprocessableEntity(new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Capacity))));
            }

            if (model?.Price == null)
            {
                return UnprocessableEntity(new ApiResponse().SetUnprocessableEntity(message:
                    string.Format(ErrorMessages.RequiredFieldErrorMessage, nameof(model.Price))));
            }

            if (model.Capacity <= 0)
            {
                return StatusCode(412, new ApiResponse().SetPreconditionFailed(message:
                    string.Format(ErrorMessages.PropertyMustBeMoreThan0, nameof(model.Capacity))));
            }

            if (model.Price < 0)
            {
                return StatusCode(412, new ApiResponse().SetPreconditionFailed(message:
                    string.Format(ErrorMessages.PropertyMustBeMoreThanOrEqual0, nameof(model.Price))));
            }

            var response = await _roomTypeService.CreateRoomType(model);
            return StatusCode(201, response);
        }

        [HttpPut]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> UpdateRoomType([FromBody] RoomTypeUpdateRequestModel model)
        {
            var response = await _roomTypeService.UpdateRoomType(model);
            if (response.IsSuccess)
            {
                return StatusCode(202, response);
            }
            return NotFound();
        }

        [HttpDelete("id/{id:guid}")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> SoftDeleteRoomType(Guid id)
        {
            var response = await _roomTypeService.SoftDeleteRoomType(id);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return NotFound();
        }
    }
}
