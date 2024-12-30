using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Startup;
using Microsoft.AspNetCore.Mvc;

namespace Dormy.WebService.Api.Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly ILogger<AdminController> _logger;

        public AdminController(ILogger<AdminController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            AdminEntity admin1 = new()
            {
                Email = "admin1@gmail.com",
                Id = Guid.NewGuid(),
                DateOfBirth = DateTime.Now,
                FirstName = "Admin",
                Gender = Models.Enums.Gender.MALE,
                JobTitle = "Admin position"
            };

            await _unitOfWork.AdminRepository.AddAsync(admin1);
            await _unitOfWork.SaveChangeAsync();

            var result = await _unitOfWork.AdminRepository.GetAllAsync(x => true);
            return Ok(result); 
        }
    }
}
