using Dormy.WebService.Api.Core.Interfaces;
using Dormy.WebService.Api.Models.Constants;
using Dormy.WebService.Api.Models.RequestModels;
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

        [HttpPost]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> CreateRoomType([FromBody] RoomTypeRequestModel model)
        {
            var response = await _roomTypeService.CreateRoomType(model);
            return Ok(response);
        }

        [HttpPut]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> UpdateRoomType([FromBody] RoomTypeUpdateRequestModel model)
        {
            var response = await _roomTypeService.UpdateRoomType(model);
            if (response.IsSuccess)
            {
                return Ok(response);
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
