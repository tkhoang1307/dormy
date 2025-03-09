using Dormy.WebService.Api.Core.Constants;
using Dormy.WebService.Api.Core.Interfaces;
using Dormy.WebService.Api.Models.Constants;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;
using Dormy.WebService.Api.Presentation.Validations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dormy.WebService.Api.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkplaceController : ControllerBase
    {
        private readonly IWorkplaceService _workplaceService;

        public WorkplaceController(IWorkplaceService workplaceService)
        {
            _workplaceService = workplaceService;
        }

        [HttpGet("id/{id:guid}")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> GetWorkplace(Guid id)
        {
            var response = await _workplaceService.GetSingleWorkplaceById(id);

            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPost("batch")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> GetWorkplaceBatch([FromBody] List<Guid> ids)
        {
            var response = await _workplaceService.GetWorkplaceBatch(ids);

            return StatusCode((int)response.StatusCode, response);
        }

        [HttpGet("all")]
        [Authorize(Roles = $"{Role.ADMIN}, {Role.USER}")]
        public async Task<IActionResult> GetAllWorkplace(int pageIndex = 1, int pageSize = 25)
        {
            var response = await _workplaceService.GetAllWorkplace(pageIndex, pageSize);

            return StatusCode((int)response.StatusCode, response);
        }


        [HttpPost]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> CreateWorkplace([FromBody] WorkplaceRequestModel model)
        {
            var modelValidator = await WorkplaceValidator.WorkplaceRequestModelValidator(model);
            if (!modelValidator.IsSuccess)
            {
                return StatusCode((int)modelValidator.StatusCode, modelValidator);
            }

            var response = await _workplaceService.CreateWorkplace(model);

            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPut]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> UpdateWorkplace([FromBody] WorkplaceUpdateRequestModel model)
        {
            var modelValidator = await WorkplaceValidator.WorkplaceUpdateRequestModelValidator(model);
            if (!modelValidator.IsSuccess)
            {
                return StatusCode((int)modelValidator.StatusCode, modelValidator);
            }

            var response = await _workplaceService.UpdateWorkplace(model);

            return StatusCode((int)response.StatusCode, response);
        }

        [HttpDelete("id/{id:guid}")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> DeleteWorkplace(Guid id)
        {
            var response = await _workplaceService.SoftDeleteWorkplace(id);

            return StatusCode((int)response.StatusCode, response);
        }

        [HttpDelete("id/hard-delete/{id:guid}")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> HardDeleteWorkplace(Guid id)
        {
            var response = await _workplaceService.HardDeleteWorkplace(id);

            return StatusCode((int)response.StatusCode, response);
        }
    }
}
